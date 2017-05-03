using System;
namespace ExecutionEngine.Common.Connect
{
    using IntellidenException = com.intelliden.common.IntellidenException;
    using ApiSession = com.intelliden.icos.api.ApiSession;
    using WorkflowManager = com.intelliden.icos.api.workflow.WorkflowManager;
    using Work = com.intelliden.icos.idc.Work;
    using WorkState = com.intelliden.icos.idc.WorkState;
    using Application.Utility.Logging;
    public class ITNCMUoWPoller : System.Threading.ThreadStart
    {
        private ILogger _logger = new CrucialLogger();
        private string uowId;
        private ApiSession session;
        private CountDownLatch latch;
        public ITNCMUoWPoller(ApiSession session, string uowId, CountDownLatch latch)
        {
            this.uowId = uowId;
            this.session = session;
            this.latch = latch;
        }
        public virtual void run()
        {
            bool finished = false;
            WorkflowManager wfManager = this.session.workflowManager();
            while (!finished)
            {
                try
                {
                    Work work = wfManager.getWork(this.uowId);
                    WorkState state = work.State;
                    if ((state.Equals(WorkState.FINISHED)) || (state.Equals(WorkState.CANCELLED)) || (state.Equals(WorkState.EXPIRED)))
                    {
                        finished = true;
                        string execStatus = work.ExecutionStatus;
                        sbyte[] logBytes = wfManager.getWorkLog(this.uowId);
                        _logger.Trace("UOW " + this.uowId + " execution status: " + execStatus);
                        _logger.Trace("UOW " + this.uowId + " log: " + StringHelperClass.NewString(logBytes));
                    }
                }
                catch (IntellidenException ie)
                {
                    _logger.Debug("An error occurred while fetching or processing UOW " + this.uowId + ie.NestedExceptionStackTrace, ie);
                    finished = true;
                }
                catch (Exception t)
                {
                    _logger.Debug("An error occurred while fetching or processing UOW " + this.uowId, t);
                    finished = true;
                }
            }
            if (this.latch != null)
            {
                this.latch.countDown();
            }
        }
    }

}