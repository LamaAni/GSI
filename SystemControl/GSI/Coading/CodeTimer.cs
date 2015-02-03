using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace GSI.Coding
{
    public class CodeTimer : Collections.OrderedDictionary<string, TimeSpan>
    {
        public CodeTimer(bool autoStart=true)
        {
            watch = new System.Diagnostics.Stopwatch();
            if (autoStart)
                watch.Start();
        }

        System.Diagnostics.Stopwatch watch;

        TimeSpan offset = new TimeSpan();

        TimeSpan m_lastMark;

        public TimeSpan LastMark
        {
            get { return m_lastMark; }
        }

        public TimeSpan FromLastMark
        {
            get { return Elapsed - LastMark; }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return watch.Elapsed - offset;
            }
        }

        public bool IsActive { get { return watch.IsRunning; } }


        protected virtual void StopTimer()
        {
            watch.Stop();
        }

        public virtual void Start()
        {
            watch.Start();
        }

        public virtual void Reset()
        {
            Reset(true);
        }

        public virtual void Reset(bool start)
        {
            this.Clear();
            offset = watch.Elapsed;
            if (start)
                watch.Start();
        }

        public virtual void Stop()
        {
            StopTimer();
        }

        public TimeSpan Mark(string key)
        {
            return Mark(key, false);
        }

        public TimeSpan Mark(string key, bool fromStart)
        {
            if (fromStart)
            {
                this[key] = this.Elapsed;
            }
            else
            {
                this[key] = FromLastMark;
                m_lastMark = Elapsed;
            }

            return this[key];
        }

        public TimeSpan Mark()
        {
            return Mark("now");
        }

        public void ToTraceHtml(StringWriter wr, string title)
        {
            if (title != null)
                wr.WriteLine("title");
            bool isFirst = true;
            foreach (string mark in this.Keys)
            {
                if (!isFirst)
                {
                    wr.Write("<br>");
                }
                else isFirst = false;
                wr.Write("&nbsp;&nbsp" + mark + " : " + this[mark].TotalMilliseconds.ToString("#0.0") + " [ms]");
            }
            wr.Write("');");
        }

        public string ToTraceString(string title = null, Func<TimeSpan,string> makeTimestap = null)
        {
            StringWriter wr = new StringWriter();
            makeTimestap = makeTimestap == null ? (ts) => ts.TotalMilliseconds.ToString("#0.0") + " [ms]" : makeTimestap;
            if (title != null)
                wr.WriteLine(title);
            foreach (string mark in this.Keys)
            {
                wr.WriteLine("  " + mark + " : " + makeTimestap(this[mark]));
            }
            string rslt = wr.ToString();
            wr.Dispose();
            return rslt;
        }

        public IEnumerable<KeyValuePair<string, TimeSpan>> GetOrderedByTime()
        {
            return this.OrderBy(kvp => -kvp.Value.Ticks);
        }
    }
}