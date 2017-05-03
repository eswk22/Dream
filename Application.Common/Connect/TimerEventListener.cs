using System.Threading;

namespace ExecutionEngine.Common.Connect
{
	public interface TimerEventListener
	{
	  void timerTimedOut();
	  void timerInterrupted(ThreadInterruptedException paramInterruptedException);
	}
}