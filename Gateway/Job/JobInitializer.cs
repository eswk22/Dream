using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Job
{
    public class JobInitializer : IJobInitializer
    {
        private readonly IJobFactory _jobFactory;

        public JobInitializer(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public void Start()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.JobFactory = _jobFactory;
            sched.Start();
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 15);

            // construct job info
            IJobDetail jobDetail = JobBuilder.Create<GatewayScheduler>()
                   .WithIdentity("job1", "group1")
                   .Build();

            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(300).WithRepeatCount(3))
                .Build();
            sched.ScheduleJob(jobDetail, trigger);
        }
    }
}
