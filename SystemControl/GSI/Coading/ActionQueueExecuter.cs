using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Coading
{
    /// <summary>
    /// Implements a function queue executer that allows functions to be executed in an external thread.
    /// </summary>
    public class ActionQueueExecuter
    {
        public ActionQueueExecuter()
        {
            ActionsQueue = new Queue<Action>();
            IsWaitForOverflow = true;
            OverflowCount = 100;
        }

        #region members

        /// <summary>
        /// The queue of actions to preform.
        /// </summary>
        public Queue<Action> ActionsQueue { get; private set; }

        public bool IsThreadRunning { get; private set; }

        /// <summary>
        /// The current pending action count.
        /// </summary>
        public int PendingCount { get { return ActionsQueue.Count; } }

        /// <summary>
        /// The number of items that constitue an overflow in the device. 
        /// If WaitForOverflow = true, then trying to add antoher action will case 
        /// the thread to wait until not in overflow, PendingCount less then OverflowCount.
        /// </summary>
        public int OverflowCount { get; set; }

        /// <summary>
        /// If true, when trying to add another worker, the thread will wait until PendingCount is less then OverflowCount.
        /// </summary>
        public bool IsWaitForOverflow { get; set; }

        /// <summary>
        /// If true then in overflow, if WaitForOverflow when trying to add another worker, the thread will wait until PendingCount is less then OverflowCount.
        /// </summary>
        public bool IsInOverflow
        {
            get { return OverflowCount < PendingCount; }
        }

        #endregion

        #region threading

        /// <summary>
        /// Runs the thread that executes the pending commands.
        /// </summary>
        protected virtual void ValidateThreadRunning()
        {
            if (IsThreadRunning)
                return;
            IsThreadRunning = true;
            Task.Run(() =>
            {
                while (this.PendingCount > 0)
                {
                    this.ActionsQueue.Dequeue()();
                }
                IsThreadRunning = false;
            });
        }

        /// <summary>
        /// Waits for the pending count to be below OverflowCount.
        /// </summary>
        public void WaitForOverflow()
        {
            WaitForOverflow(OverflowCount);
        }

        /// <summary>
        /// Waits for the pending count to be below count.
        /// </summary>
        /// <param name="count"></param>
        public void WaitForOverflow(int count)
        {
            if (count <= 0)
                count = 1;
            while (PendingCount >= count)
            {
                System.Threading.Thread.Sleep(10);
                ValidateThreadRunning();
            }
        }

        /// <summary>
        /// Waits until all the actions have been finished.
        /// </summary>
        public void WaitForAllActionsToCompleate()
        {
            while(IsThreadRunning)
            {
                System.Threading.Thread.Sleep(10);
            }
        }

        #endregion

        #region operations

        /// <summary>
        /// Adds a new action to the pending.
        /// </summary>
        /// <param name="a"></param>
        public virtual void AddAction(Action a)
        {
            if (IsWaitForOverflow)
                WaitForOverflow();
            ActionsQueue.Enqueue(a);
            ValidateThreadRunning();
        }

        #endregion
    }
}
