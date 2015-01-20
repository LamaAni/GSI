using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSI.Stage.Piror
{
    /// <summary>
    /// Implements a proscan command server that controls a pro scan stage.
    /// </summary>
    public class ProScan : IPositionControl
    {
        /// <summary>
        /// Create a new pro scan.
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        public ProScan(string portName)
            : this(portName, 38400)
        {
        }

        /// <summary>
        /// Create a new pro scan.
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        public ProScan(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
            : this(new SerialPort(portName, baudRate, parity, dataBits, stopBits))
        {
        }

        /// <summary>
        /// Create a new pro scan from a port.
        /// </summary>
        /// <param name="port"></param>
        public ProScan(SerialPort port)
        {
            // validates that the current port is in the correct baoud rate.
            port.NewLine = "\r";
            PreformBoudRateValidation(port);
            Watch = new Stopwatch();
            IdleCommands = new List<StageCommand>();
            InitializeStageIdleCommands();
            MinDeltaX = 2;
            MinDeltaY = 2;
            Server = new StageServer(port, IdleCommands);
            Server.LineValidator = (rsp) => { 
                return rsp.Trim() != "R"; 
            };

            // called to update params.
            UpdateCurrentParams();

            // send idle commands to record the current stage state.
            Server.PopulateIdleCommands();

            // stoping the stage.
            StopStage();
        }

        /// <summary>
        /// Call to update the stage params (info, deltax, delta y) exc...
        /// </summary>
        public void UpdateCurrentParams()
        {

            Server.AppendCommand(new StopAtEndStageCommand("?", (rsp) =>
            {
                this.ProInfo = rsp.Replace("\r", "\n");
            }));

            Server.AppendCommand(new StopAtEndStageCommand("STAGE", (rsp) =>
            {
                this.StageInfo = rsp.Replace("\r", "\n");
            }));

            Server.AppendCommand(new StageCommand("RES s", 1, (rsp) =>
            {
                double val;
                if(double.TryParse(rsp,out val))
                {
                    Resolution = val;
                }
            }));
        }

        ~ProScan()
        {
            StopServer();
        }

        #region boud rate validation on port

        public static int[] ValidBaudRate = new int[] { 9600, 19200, 38400 };
        public static string[] ValidBaudRateCommad = new string[] { "BAUD 96", "BAUD 19", "BAUD 38" };

        /// <summary>
        /// Preforms a boud rate validation for the current port. 
        /// if the boud rate is diffrent then required then change the boud rate.
        /// </summary>
        void PreformBoudRateValidation(SerialPort port)
        {
            int oidx = -1;
            for (int i = 0; i < ValidBaudRate.Length;i++ )
            {
                if (port.BaudRate == ValidBaudRate[i])
                {
                    oidx = i;
                    break;
                }
            }

            if (oidx < 0)
                throw new Exception("Boud rate invalid for pro scan, " + port.BaudRate);

            bool wasOpen=port.IsOpen;
            if (port.IsOpen)
                port.Close();
            // cehck if current boud rate is good.
            if (IsBaudRateValid(port, port.BaudRate))
                return;

            // find valid boud rate.
            for (int i = 0; i < ValidBaudRate.Length; i++)
            {
                if (!IsBaudRateValid(port, ValidBaudRate[i]))
                    continue;

                port.BaudRate = ValidBaudRate[i];
                if (port.IsOpen)
                    port.Close();
                // change the boud rate to prefered.
                port.Open();
                port.WriteLine(ValidBaudRateCommad[oidx]);
                //string returnLine = port.ReadLine();
                port.Close();
                break;
            }

            // set the prefered to the port.
            port.BaudRate = ValidBaudRate[oidx];

            if (wasOpen)
                port.Open();
        }

        /// <summary>
        /// Checks if the current boud rate is valid.
        /// </summary>
        /// <param name="port"></param>
        bool IsBaudRateValid(SerialPort port, int baudRate)
        {
           // port.WriteTimeout = 100;
            port.BaudRate = baudRate;
            port.Open();
            port.ReadExisting();
            port.WriteLine("K");
            System.Threading.Thread.Sleep(200);
            string existing = port.ReadExisting();
            port.Close();
            if (existing != "R\r")
                return false;
            return true;
        }

        #endregion

        #region members

        /// <summary>
        /// The stopwatch to allow the recording of the current time,
        /// relative to the last stage start. Null if not set.
        /// </summary>
        public Stopwatch Watch { get; private set; }

        /// <summary>
        /// The current zero time where the clock was set.
        /// </summary>
        public DateTime ZeroTime { get; private set; }

        /// <summary>
        /// The information of the devices, as returned for "?"
        /// </summary>
        public string StageInfo { get; private set; }

        /// <summary>
        /// The information of the devices, as returned for "?"
        /// </summary>
        public string ProInfo { get; private set; }

        /// <summary>
        /// The resolution in microns.
        /// </summary>
        public double Resolution { get; private set; }

        /// <summary>
        /// The stage server that receives and sends commands.
        /// </summary>
        public StageServer Server { get; private set; }

        public List<StageCommand> IdleCommands  { get; private set; }

        /// <summary>
        /// True if the server is running.
        /// </summary>
        public bool IsRunning { get { return Server.IsRunning; } }

        /// <summary>
        /// If true, reading without a delay between the reads.
        /// </summary>
        public bool IsInFastMode { get { return !Server.DoDelayBetweenIdleCalls; } }

        /// <summary>
        /// The confidence in x direction.
        /// </summary>
        public double MinDeltaX { get; set; }

        /// <summary>
        /// The confindence in y direction.
        /// </summary>
        public double MinDeltaY { get; private set; }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the stage idle commands.
        /// </summary>
        protected virtual void InitializeStageIdleCommands()
        {
            IdleCommands.Add(new StageCommand("PS", 1, (rsp) =>
                {
                    if (rsp.Trim() == "R")
                        return;
                    double x;
                    double y;
                    string[] args = rsp.Split(',');
                    if (args.Length != 2)
                        return;
                    if (!double.TryParse(args[0], out x) || !double.TryParse(args[1], out y))
                        return;
                    readPosition(x, y);
                }));
        }

        #endregion

        #region Stage commands

        /// <summary>
        /// Do the command rotation if any.
        /// </summary>
        /// <param name="vx">The vector x</param>
        /// <param name="vy">The vector y</param>
        /// <param name="vxt">The rotated vector x</param>
        /// <param name="vyt">The rotated vector y</param>
        /// <param name="angle">In radians, the rotation angle.</param>
        public static void Rotate(double vx, double vy, out double vxt, out double vyt, double angle, double resolution)
        {
            // rotation in space according to the rotation matrix.
            vxt = vx * Math.Cos(angle) - vy * Math.Sin(angle);
            vyt = vx * Math.Sin(angle) + vy * Math.Cos(angle);

            vxt = Math.Round(vxt / resolution) * resolution;
            vyt = Math.Round(vyt / resolution) * resolution;
        }

        /// <summary>
        /// Do the command rotation if any.
        /// </summary>
        /// <param name="vx">The vector x</param>
        /// <param name="vy">The vector y</param>
        /// <param name="angle">In radians, the rotation angle.</param>
        /// <returns>The rotated vector (x,y)</returns>
        public static Tuple<double, double> Rotate(double vx, double vy, double angle, double resolution)
        {
            // rotation in space according to the rotation matrix.
            double vxt, vyt;
            Rotate(vx, vy, out vxt, out vyt, angle,resolution);
            return new Tuple<double, double>(vxt, vyt);
        }

        /// <summary>
        /// Sets the stage speed, in um/sec.
        /// </summary>
        /// <param name="sx">X speed</param>
        /// <param name="sy">Y speed</param>
        public void SetSpeed(double sx, double sy)
        {
            // sending the command and expecting end line.
            Tuple<double, double> rotated = Rotate(sx, sy, Angle, Resolution);
            Server.AppendCommand("VS " + rotated.Item1 + "," + rotated.Item2, 0);
        }

        /// <summary>
        /// Set the absolute position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosition(double x, double y, bool async = true)
        {
            Tuple<double, double> rotated = Rotate(x, y, Angle, Resolution);
            Server.AppendCommand("G " + rotated.Item1 / Resolution + "," + rotated.Item2 / Resolution, 0);
            if (!async)
                this.WaitUntilPositionByMove(x, y);
        }

        /// <summary>
        /// Tops any stage movements while keeping the current velocity.
        /// </summary>
        public void StopStage()
        {
            // sending the command and expecting end line.
            Server.AppendCommand("I", 0);
        }

        /// <summary>
        /// Starts the server, and connects to the port.
        /// </summary>
        public void StartServer()
        {
            Server.StartServer();
        }

        /// <summary>
        /// Stops the server and disconnects from the port.
        /// </summary>
        public void StopServer()
        {
            Server.StopServer();
        }

        /// <summary>
        /// Sends a specific command and waits for responce if any.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="getResponce"></param>
        /// <param name="responceLines"></param>
        /// <returns></returns>
        public void SendCommand(string cmd)
        {
            SendCommand(cmd, 0);
        }

        /// <summary>
        /// Sends a specific command and waits for responce if any.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="getResponce"></param>
        /// <param name="responceLines"></param>
        /// <returns></returns>
        public string SendCommandUntilEnd(string cmd, int timeout = 3000)
        {
            bool recivedResponce = false;
            string responce = "";

            StageCommand scmnd = new StageCommand(cmd, 0, (rsp) =>
                {
                    responce = rsp;
                    recivedResponce = true;
                });

            scmnd.StopOnResponse = (rsp) =>
            {
                return rsp.Trim().ToLower() == "end";
            };

            this.Server.AppendCommand(scmnd);

            DateTime start = DateTime.Now;

            while (!recivedResponce)
            {
                System.Threading.Thread.Sleep(1);
                if ((DateTime.Now - start).TotalMilliseconds > timeout)
                {
                    responce = "[Error, timed out]";
                    break;
                }
            }
            
            return responce;
        }


        /// <summary>
        /// Sends a specific command and waits for responce if any.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="getResponce"></param>
        /// <param name="responceLines"></param>
        /// <returns></returns>
        public string SendCommand(string cmd, uint responceLines)
        {
            bool recivedResponce = false;
            string responce = "";
            this.Server.AppendCommand(
                new StageCommand(cmd, responceLines, (rsp) =>
                {
                    responce = rsp;
                    recivedResponce = true;
                }));

            if (responceLines > 0)
            {
                while (!recivedResponce)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
            return responce;
        }

        /// <summary>
        /// If true, the time delay between reading idle commands (Position, ect...) 
        /// will be be set to zero. Fastest reading as possible.
        /// </summary>
        /// <param name="on"></param>
        public void SetFastMode(bool on)
        {
            Server.DoDelayBetweenIdleCalls = !on;
        }

        /// <summary>
        /// Set baud rate, also resets the port.
        /// </summary>
        /// <param name="baud"></param>
        public void SetBaudRate(ProScanBaudRates baud)
        {
            this.SendCommand("baud " + (int)baud, 1);
            this.Server.Port.Close();
            this.Server.Port.BaudRate = int.Parse(baud.ToString().Substring(1));
            this.Server.Port.Open();
        }

        #endregion

        #region IPositionReader Members


        public void SetAsHome()
        {
            this.SendCommand("PS 0,0");
        }

        void readPosition(double x, double y)
        {
            AbsolutePositionX = x * Resolution;
            AbsolutePositionY = y * Resolution;
            double rotatedX, rotatedY;
            Rotate(AbsolutePositionX, AbsolutePositionY, out rotatedX, out rotatedY, -Angle, Resolution);
            PositionX = rotatedX;
            PositionY = rotatedY;
            if (OnRecivedPosition != null)
                OnRecivedPosition(this,
                    new PositionRecivedEventArgs(PositionX, PositionY, ZeroTime + Watch.Elapsed, ZeroTime));
        }

        /// <summary>
        /// Called when a position is recived by the device.
        /// <para>NOTE! if this action takes a long time if will stop the reading thread.</para>
        /// </summary>
        public event EventHandler<PositionRecivedEventArgs> OnRecivedPosition;

        /// <summary>
        /// The position in the X direction.
        /// </summary>
        public double PositionX { get; private set; }

        /// <summary>
        /// The position in the Y direction.
        /// </summary>
        public double PositionY { get; private set; }

        /// <summary>
        /// The position of the stage without the rotation.
        /// </summary>
        public double AbsolutePositionX
        {
            get;
            private set;
        }

        /// <summary>
        /// The position of the stage without the rotation.
        /// </summary>
        public double AbsolutePositionY
        {
            get;
            private set;
        }


        /// <summary>
        /// The rotation angle in radians, to be taken into account when
        /// sending position commands to the stage.
        /// </summary>
        public double Angle
        {
            get;
            set;
        }

        #endregion

        #region Complex commands

        /// <summary>
        /// Waits until the position has reached. (Using delta)
        /// </summary>
        public void WaitUntilPositionByMove(double x, double y)
        {
            // event to wait for compleated.
            AutoResetEvent waitForMe = new AutoResetEvent(false);

            Func<double, double, bool> checkPositionReched = (X, Y) =>
            {
                bool xOK = X <= x + MinDeltaX && X >= x - MinDeltaX;
                bool yOk = Y <= y + MinDeltaY && Y >= y - MinDeltaY;
                return xOK && yOk;
            };

            if (checkPositionReched(PositionX, PositionY))
                return;

            // check if reached position
            EventHandler<PositionRecivedEventArgs> checkReached = (s, e) =>
            {
                if (checkPositionReched(e.X, e.Y))
                {
                    waitForMe.Set();
                }
            };

            OnRecivedPosition += checkReached;
            waitForMe.WaitOne();
            OnRecivedPosition -= checkReached;
        }

                /// <summary>
        /// Waits until the position has reached. (Using delta) 
        /// <param name="accurate">IF true, then uses up cpu time.</param>
        /// </summary>
        public void WaitUntilAnyMovement(bool accurate = true)
        {
            double x = PositionX;
            double y = PositionY;
            while (true)
            {
                if (x != PositionY || y != PositionY)
                    break;
                if (!accurate)
                    System.Threading.Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Waits until the position has reached. (Using delta)
        /// </summary>
        public void WaitUntilPositionBySpeed(double x, double y, double vx, double vy)
        {
            double directionX = Math.Sign(vx);
            double directionY = Math.Sign(vy);

            // event to wait for compleated.
            AutoResetEvent waitForMe = new AutoResetEvent(false);

            // check if reached position
            EventHandler<PositionRecivedEventArgs> checkReached = (s, e) =>
            {
                bool xOK = vx == 0 || directionX * (x - e.X) < 0;
                bool yOk = vy == 0 || directionY * (y - e.Y) < 0;
                if (xOK && yOk)
                {
                    waitForMe.Set();
                }
            };

            OnRecivedPosition += checkReached;
            waitForMe.WaitOne();
            OnRecivedPosition -= checkReached;
        }
        
        /// <summary>
        /// Called to execute a path. The path is defined by the speed
        /// and the positions.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="vx"></param>
        /// <param name="vy"></param>
        /// <param name="asynced">True if runs in another thread.</param>
        /// <param name="ended">Called when started.</param>
        /// <param name="started">Called when edned</param>
        public void DoPath(double startX, double startY, double endX, double endY,
            double vx, double vy, bool asynced,bool useSpeedupOffsets, Action started, Action ended)
        {
            Action dopath = () =>
            {
                double speedOffsetY = useSpeedupOffsets ? vy * 2 : 0;
                double seeedOffsetX = useSpeedupOffsets ? vx * 2 : 0;

                // going to start position.
                this.StopStage();
                // some issues with the stage.
                System.Threading.Thread.Sleep(100);
                // case of no offset 
                this.SetPosition(startX - seeedOffsetX, startY - speedOffsetY);
                this.WaitUntilPositionByMove(startX - seeedOffsetX, startY - speedOffsetY);
                // some issues with the stage.
                System.Threading.Thread.Sleep(400);
                this.SetSpeed(vx, vy);
                if (useSpeedupOffsets)
                    this.WaitUntilPositionBySpeed(startX, startY, vx, vy);
                if (started != null)
                    started();
                this.WaitUntilPositionBySpeed(endX, endY, vx, vy);
                if (ended != null)
                    ended();
                this.StopStage();
            };

            if (asynced)
                Task.Run(dopath);
            else dopath();
        }

        #endregion

        #region ITimeKeeper Members

        public void SetZeroTime()
        {
            ZeroTime = DateTime.Now;
            Watch.Reset();
            Watch.Start();
        }

        #endregion


    }

    public enum ProScanBaudRates { 
        B9600 = 96, 
        B19200 = 19, 
        B38400 = 38
    };
}
