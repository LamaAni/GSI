using GSI.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Stage
{
    /// <summary>
    /// A position reader that can return a measurement position.
    /// Holds information about the current positon, and handles 
    /// the position read event.
    /// </summary>
    public interface IPositionControl : ITimeKeeper
    {
        /// <summary>
        /// Called when a position is recived by the device.
        /// <para>NOTE! if this action takes a long time if will stop the reading thread.</para>
        /// </summary>
        event EventHandler<PositionRecivedEventArgs> OnRecivedPosition;

        /// <summary>
        /// The x position.
        /// </summary>
        double PositionX { get; }

        /// <summary>
        /// The y position.
        /// </summary>
        double PositionY { get; }

        /// <summary>
        /// The position of the stage without the rotation.
        /// </summary>
        double AbsolutePositionX { get;}

        /// <summary>
        /// The position of the stage without the rotation.
        /// </summary>
        double AbsolutePositionY { get;}

        /// <summary>
        /// Sets the position.
        /// </summary>
        void SetPosition(double x, double y, bool async = true);

        /// <summary>
        /// Sets the speed.
        /// </summary>
        /// <param name="vx"></param>
        /// <param name="vy"></param>
        /// <param name="async"></param>
        void SetSpeed(double vx, double vy);

        /// <summary>
        /// Run through the specific path using the specified parameters.
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="async">If true the the command is executed async.</param>
        /// <param name="ended">Called when started.</param>
        /// <param name="started">Called when edned</param>
        void DoPath(double x0, double y0, double x1, double y1,
            double vx, double vy,
            bool async, bool useSpeedupOffsets,
            Action started, Action ended);

        /// <summary>
        /// The rotated plain agnle, in radians, to be taken into account when sending commands to the 
        /// stage.
        /// </summary>
        double Angle { get; set; }

        /// <summary>
        /// Sets the current position as the home position;
        /// </summary>
        void SetAsHome();
    }

    public class PositionRecivedEventArgs : EventArgs
    {
        public PositionRecivedEventArgs(double x, double y, DateTime stamp, DateTime zeroTime)
        {
            X = x;
            Y = y;
            TimeStamp = stamp;
            Elapsed = stamp - zeroTime;
        }

        /// <summary>
        /// The time span since the last sync command.
        /// </summary>
        public TimeSpan Elapsed { get; private set; }

        /// <summary>
        /// The time stamp for the position read.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// The event X position.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// The event Y position.
        /// </summary>
        public double Y { get; private set; }
    }
}
