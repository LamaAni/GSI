using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Context
{
    /// <summary>
    /// An async pending event queue, used for calling events from a diffrent 
    /// thread. To call events directly use callSynced.
    /// </summary>
    /// <typeparam name="T">The event args type</typeparam>
    public class AsyncPendingEventQueue<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invokeEvent">The invoke command.</param>
        /// <param name="callSynced">If true call synced.</param>
        public AsyncPendingEventQueue(Action<T> invokeEvent, bool callSynced = false)
        {
            InvokeEvent = invokeEvent;
            CallSyncd = callSynced;
        }

        #region members

        /// <summary>
        /// The pending events collection.
        /// </summary>
        Queue<T> m_pendingEvents = new Queue<T>();

        /// <summary>
        /// If true the current is invoking pending events.
        /// </summary>
        public bool IsInvoking { get; private set; }

        /// <summary>
        /// Call to invoke the event.
        /// </summary>
        public Action<T> InvokeEvent { get; private set; }

        /// <summary>
        /// The number of oending events.
        /// </summary>
        public int PendingEventCount { get { return m_pendingEvents.Count; } }

        /// <summary>
        /// If true calles the events sync. If currently invoking in thread then
        /// the event will be added to the pull. Otherwise calls sync.
        /// </summary>
        public bool CallSyncd { get; private set; }

        #endregion

        #region methods

        /// <summary>
        /// Called to invoke any pending events. 
        /// The invokcation method depends on CallSync parameter.
        /// If currently invoking in thread then exits without doing anything.
        /// </summary>
        protected virtual void InvokePending()
        {
            if (IsInvoking)
                return;
            IsInvoking = true;
            Action doInvoke = () =>
            {
                while (m_pendingEvents.Count > 0)
                    InvokeEvent(m_pendingEvents.Dequeue());
                IsInvoking = false;
            };

            if (CallSyncd)
                doInvoke();
            else Task.Run(doInvoke);
        }

        /// <summary>
        /// Push a new event into the pull.
        /// </summary>
        /// <param name="ev"></param>
        public void PushEvent(T ev)
        {
            m_pendingEvents.Enqueue(ev);
            InvokePending();
        }

        #endregion
    }
}
