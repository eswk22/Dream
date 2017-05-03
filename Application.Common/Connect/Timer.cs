using System.Threading;
namespace ExecutionEngine.Common.Connect
{
    public class Timer : System.Threading.ThreadStart
    {
        private long timeOut = 0L;
        private TimerEventListener listener = null;
        private Thread thread = null;
        public const int TIMER_NOT_STARTED = 0;
        public const int TIMER_STARTED = 1;
        public const int TIMER_TIMEDOUT = 2;
        public const int TIMER_INTERRUPTED = 3;
        public const int TIMER_COMPLETED = 4;
        private int currentStatus = 0;
        public Timer(long timeOut, TimerEventListener listener)
        {   
            if (timeOut < 1L)
            {   
                throw new System.ArgumentException("Time-Out value cannot be < 1");
            }  
            if (listener == null)
            {
                throw new System.ArgumentException("Listener cannot be null");
            } 
            this.timeOut = (timeOut * 1000L);
            this.listener = listener;
        }
        public virtual void startTimer()
        {  
            this.thread = new Thread(this);
            this.currentStatus = 1;
            this.thread.Daemon = true;
            this.thread.Start();
        }
        public virtual int Status
        {
            get
            {
                return this.currentStatus;
            }
            set
            {
                this.currentStatus = value;
            }
        }
        public virtual void run()
        {
            try
            {
                Thread.Sleep(this.timeOut);
                if (this.currentStatus == 1)
                {
                    this.currentStatus = 2;
                    this.listener.timerTimedOut();
                }
            }
            catch (ThreadInterruptedException iexp)
            {
                this.currentStatus = 3;
                this.listener.timerInterrupted(iexp);
            }
        }
    }
}