using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Stage
{
    /// <summary>
    /// Implements a command class for input and output of stage commands from
    /// the serial port. The stage server has an idel command block.
    /// </summary>
    public class StageServer
    {
        /// <summary>
        /// Creates a stage sever from the serial port.
        /// </summary>
        /// <param name="port">The port for the server. (Should be closed, if not the port will be closed).</param>
        /// <param name="idleCommands">A list of commands to execute with the server is idle. 
        /// Idle commands are executed repeatedly by order when no other commands are pending in the server. </param>
        public StageServer(SerialPort port, IEnumerable<StageCommand> idleCommands = null)
        {
            Port = port;
            IdleCommands = idleCommands;
            PendingCommands = new Queue<StageCommand>();
            PendingLines = new Queue<string>();

            if (Port.IsOpen)
                Port.Close();

            IsRunning = false;
            TimeDelayBetweenIdleCalls = 1;
            DoDelayBetweenIdleCalls = true;
            ReadDataCollector = new Queue<string>();

            EOLChar = '\r';
        }

        ~StageServer()
        {
            try
            {
                StopServer();
            }
            catch
            {
            }
        }

        #region members

        /// <summary>
        /// The total number of reads in the server since created.
        /// </summary>
        public int NumberOfReads { get; private set; }

        /// <summary>
        /// <para>Returns true if the current line is valid. If not true, the current line is ignored.</para>
        /// <para>Overrided this function to also allow translation of command lines sent from the stage.
        /// For this case return always false, make sure not to hold the responce since reading the port will
        /// not contrinue until the function has returned.</para>
        /// </summary>
        public Func<string,bool> LineValidator { get; set; }

        /// <summary>
        /// The serial port associated with the server.
        /// </summary>
        public SerialPort Port { get; private set; }

        /// <summary>
        /// A set of idle commands to be executed when there are no pending commands to be exuected. 
        /// </summary>
        public IEnumerable<StageCommand> IdleCommands { get; set; }

        /// <summary>
        /// A queue of pending commands, to be executed on the port.
        /// </summary>
        public Queue<StageCommand> PendingCommands { get; private set; }

        /// <summary>
        /// True if service process is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// The time in ms to wait between idle calls.
        /// </summary>
        public int TimeDelayBetweenIdleCalls { get; set; }

        /// <summary>
        /// True if we need to delay between idle calls. When no delay, this is fast mode
        /// where the data is read as fast as possible.
        /// </summary>
        public bool DoDelayBetweenIdleCalls { get; set; }

        /// <summary>
        /// A collection of maximal last 100 commands. Exists only in debug mode.
        /// </summary>
        public Queue<string> ReadDataCollector { get; private set; }

        /// <summary>
        /// The end of line charecter to expect.
        /// </summary>
        public char EOLChar { get; set; }

        /// <summary>
        /// A collection of lines pending for the commands. The lines 
        /// that are pending are validated lines.
        /// </summary>
        public Queue<string> PendingLines { get; private set; }

        #endregion

        #region Command methods

        /// <summary>
        /// Adds a new command to the server pending list.
        /// </summary>
        /// <param name="cmnd"></param>
        public void AppendCommand(StageCommand cmnd)
        {
            PendingCommands.Enqueue(cmnd);
        }

        /// <summary>
        /// Creates a new server command and appends it to the server.
        /// </summary>
        /// <param name="cmnd"></param>
        /// <param name="ac"></param>
        /// <returns>The new command</returns>
        public StageCommand AppendCommand(string cmnd, uint lines = 0, Action<string> ac = null)
        {
            StageCommand c = new StageCommand(cmnd, lines, ac);
            AppendCommand(c);
            return c;
        }

        #endregion

        #region Service methods

        /// <summary>
        /// If true, stop immidiatly.
        /// </summary>
        bool m_abort = false;

        /// <summary>
        /// Starts the server process, and sends all pending commands to the port,
        /// by the command order.
        /// </summary>
        public void StartServer()
        {
            if (IsRunning)
                throw new Exception("Server is already running!");
            m_abort = false;
            IsRunning = true;

            if (!Port.IsOpen)
                Port.Open();

            // clearing the current pending lines.
            PendingLines.Clear();

            // Run the function in a difrrent thread, and return. Do not wait for funtion to complete.
            Task.Run(() => { DoServiceReading(); });
            Task.Run(() => { DoServriceProcessing(); });
        }
        
        /// <summary>
        /// Stops the running server,
        /// <param name="ignorePendingCommands">If true, the service stops immidiatly ignoring any commands pending.</param>
        /// </summary>
        public void StopServer(bool ignorePendingCommands = true)
        {
            if (ignorePendingCommands)
                m_abort = true;
            IsRunning = false;
            if (Port.IsOpen)
                Port.Close();
        }

        void DoServiceReading()
        {
            string pendingLine="";
            if (Port.BytesToRead > 0)
                ReadExisting("Existing at port:");
            while (IsRunning)
            {
                if (!Port.IsOpen)
                {
                    System.Threading.Thread.Sleep(10);
                    continue;
                }
                if (Port.BytesToRead == 0)
                {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }
                pendingLine += ReadExisting();
                string[] lines = pendingLine.Split(EOLChar);
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    if (LineValidator != null || LineValidator(lines[i]))
                    {
                        PendingLines.Enqueue(lines[i]);
                        NumberOfReads += 1;
                    }
                }
                pendingLine = lines[lines.Length - 1];
                //int idx = existing.IndexOf(EOLChar);
                //if (idx > -1)
                //{
                //    // found end of line, adding to line.
                //    pendingLine += existing.Substring(0, idx);
                //    if (LineValidator != null && LineValidator(pendingLine))
                //    {
                //        PendingLines.Enqueue(pendingLine);
                //    };
                //    idx += 1;
                //    pendingLine = existing.Length > idx ? existing.Substring(idx) : "";
                //}
                //else pendingLine += existing;
            }
        }

        /// <summary>
        /// Internal function, sends commands to the port and waits for responce. If no commands are
        /// present sends the idle command list to the pending commands.
        /// </summary>
        void DoServriceProcessing()
        {
            while (IsRunning)
            {
                if (!Port.IsOpen)
                {
                    System.Threading.Thread.Sleep(10);
                    continue;
                }
                if (PendingCommands.Count == 0)
                {
                    // clearing pending lines, no commands.
                    PendingLines.Clear();

                    // Wait before contiuing.
                    if (DoDelayBetweenIdleCalls)
                        System.Threading.Thread.Sleep(TimeDelayBetweenIdleCalls);

                    // check if any commands were added while we slept.
                    if (PendingCommands.Count != 0)
                        continue;

                    // If no commands present add the idle commands if any to the pending commands.
                    PopulateIdleCommands();

                    continue;
                }

                // remove the top command.
                StageCommand cmnd = PendingCommands.Dequeue();

                // write the command to the serial port.
                Port.WriteLine(cmnd.Command);

                DateTime start = DateTime.Now;
                while (Port.BytesToWrite > 0)
                {
                    if (DateTime.Now - start > cmnd.MaximalNoBytesAtBufferTimeout)
                    {
                        cmnd.Invalidate();
                        break;
                    }
                    continue;
                }

                if (cmnd.TimedOut)
                    continue;

                if (cmnd.WaitAfterCommand.TotalMilliseconds > 0)
                    System.Threading.Thread.Sleep(Convert.ToInt32(cmnd.WaitAfterCommand.TotalMilliseconds));

                // reading the response, until we get the line end command.
                string response = "";
                start = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now - start > cmnd.MaximalNoBytesAtBufferTimeout)
                    {
                        cmnd.Invalidate();
                        break;
                    }
                    else if (cmnd.StopOnResponse != null)
                    {
                        // do stop on responce counting. 
                        bool isStopped=false;
                        bool isFirst = true;
                        while (PendingLines.Count > 0)
                        {
                            string rsp = PendingLines.Dequeue();
                            if (!isFirst)
                                response += "\n";
                            response += rsp;
                            isFirst = false;
                            if (cmnd.StopOnResponse(rsp))
                            {
                                isStopped = true;
                                break;
                            }
                        }
                        if (isStopped)
                            break;
                    }
                    else if (PendingLines.Count >= cmnd.LinesToRead)
                    {
                        bool isFirst = true;
                        for (int i = 0; i < cmnd.LinesToRead; i++)
                        {
                            if (!isFirst)
                                response += "\n";
                            isFirst = false;
                            response += PendingLines.Dequeue();
                        }
                        break;
                    }
                    System.Threading.Thread.Sleep(1);
                }

                if (cmnd.WaitForDump.TotalMilliseconds > 0)
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(cmnd.WaitAfterCommand.TotalMilliseconds));
                }

                // call the responce function.
                cmnd.CallResponseIfAny(response);
            }
        }

        /// <summary>
        /// Sends the idle commands collection to the port.
        /// </summary>
        public void PopulateIdleCommands()
        {
            if (IdleCommands != null)
            {
                foreach (StageCommand c in IdleCommands)
                {
                    c.Reset();
                    PendingCommands.Enqueue(c);
                }
            }
        }

        string ReadExisting(string addon = null)
        {
            string existing = addon == null ? Port.ReadExisting() : addon + Port.ReadExisting();

#if DEBUG
            ReadDataCollector.Enqueue(existing);
            if (ReadDataCollector.Count > 100)
                ReadDataCollector.Dequeue();
#endif

            return existing;
        }

#if DEBUG

        /// <summary>
        /// Retuns in debug mode the last 100 reads.
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public string GetReadDataCollectorString(string splitter = "\n")
        {
            while (true)
                try
                {
                    return string.Join(splitter, ReadDataCollector.ToArray().Select(s => s));
                }
                catch (Exception ex)
                {
                    continue;
                }
        }
#endif

        #endregion
    }
}
