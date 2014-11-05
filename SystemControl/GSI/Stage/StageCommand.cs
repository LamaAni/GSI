using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Stage
{
    /// <summary>
    /// Implements a single stage command, to be executed at the serial port. 
    /// The stage command allows for the definition of the function to execute when the command finishes.
    /// </summary>
    public class StageCommand
    {
        /// <summary>
        /// The identification of a stage command.
        /// </summary>
        /// <param name="cmnd">The command to send to the serial port</param>
        /// <param name="doOnResponce">The function to execute when the response arrives from the port. 
        /// Null means do nothing.</param>
        public StageCommand(string cmnd, uint linesToRead = 0, Action<string> doOnResponce = null)
            : this(cmnd, new TimeSpan(0), new TimeSpan(0), linesToRead, doOnResponce)
        {
        }

        /// <summary>
        /// The identification of a stage command.
        /// </summary>
        /// <param name="cmnd">The command to send to the serial port</param>
        /// <param name="doOnResponce">The function to execute when the response arrives from the port. 
        /// Null means do nothing.</param>
        public StageCommand(string cmnd, TimeSpan waitAfterCommand,
            TimeSpan waitForDump, uint linesToRead = 0, Action<string> doOnResponce = null)
        {
            Command = cmnd;
            LinesToRead = linesToRead;
            DoOnResponse = doOnResponce;
            WaitAfterCommand = waitAfterCommand;
            WaitForDump = waitForDump;
            MaximalNoBytesAtBufferTimeout = new TimeSpan(0, 0, 1);
        }

        /// <summary>
        /// The command to send to the port.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// The function to execute when the serial port responds.
        /// </summary>
        public Action<string> DoOnResponse { get; private set; }

        /// <summary>
        /// Stops the reading of the lines according to a predict function.
        /// </summary>
        public Func<string, bool> StopOnResponse { get; set; }

        /// <summary>
        /// If true this command was timeout.
        /// </summary>
        public bool TimedOut { get; private set; }

        /// <summary>
        /// Calls the response function if not null. Resurns true if called.
        /// </summary>
        /// <param name="response">The response string</param>
        public bool CallResponseIfAny(string response)
        {
            if (DoOnResponse == null)
                return false;

            // Calling the function.
            DoOnResponse(response);

            return true;
        }

        /// <summary>
        /// Sets the Timout property to true.
        /// </summary>
        public void Invalidate()
        {
            TimedOut = true;
        }

        /// <summary>
        /// Reset the command to allow reuse.
        /// </summary>
        public void Reset()
        {
            TimedOut = false;
        }

        /// <summary>
        /// The number of lines we are expecting at the buffer after the command has been compleated.
        /// </summary>
        public uint LinesToRead
        {
            get;
            set;
        }

        /// <summary>
        /// The time to wait after the command was given to read the command data.
        /// </summary>
        public TimeSpan WaitAfterCommand { get; private set; }

        /// <summary>
        /// The time to wait between the end of the command and the read dump after the command has read.
        /// </summary>
        public TimeSpan WaitForDump { get; set; }

        /// <summary>
        /// The timeout for the command in case there is no bytes at buffer. 
        /// </summary>
        public TimeSpan MaximalNoBytesAtBufferTimeout { get; set; }
    }
}
