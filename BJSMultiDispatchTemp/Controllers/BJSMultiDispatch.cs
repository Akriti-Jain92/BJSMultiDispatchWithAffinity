using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSMultiDispatchTemp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BJSMultiDispatch : ControllerBase
    {

        private readonly ILogger<BJSMultiDispatch> _logger;

        public BJSMultiDispatch(ILogger<BJSMultiDispatch> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<String> Get()
        {
            Console.WriteLine("I am for testing");
            System.Diagnostics.Debug.WriteLine("I am for testing");
            var jobBuilder1 = JobBuilder.Create(jobPartition: "5mins1", jobId: "id5mins1")
              .WithCallback(typeof(JobCallback5min))
              .WithExecutionAffinities("5mins1")
              .WithRepeatStrategy(3, TimeSpan.FromMinutes(5))
              .WithRetryStrategy(2, TimeSpan.FromMinutes(1));

            var managementClient1 = new JobManagementClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjstemp3;AccountKey=D17j5l15vII4xYvg0Q3eHnyx7E92ywx6uq25e2gjKUEGLO7mzS6aYdf/4yL+dResCXszZL8W0LsF+AStNSTMUw==;EndpointSuffix=core.windows.net"),
              executionAffinity: "5mins1",
              eventSource: new EventSourceTest(),
              queueNamePrefix: "queue5mins1",
              tableName: "table5mins1",
              encryptionUtility: null);

            await managementClient1
              .CreateOrUpdateJob(jobBuilder1)
              .ConfigureAwait(continueOnCapturedContext: false);

            var dispatcherClient1 = new JobDispatcherClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjstemp3;AccountKey=D17j5l15vII4xYvg0Q3eHnyx7E92ywx6uq25e2gjKUEGLO7mzS6aYdf/4yL+dResCXszZL8W0LsF+AStNSTMUw==;EndpointSuffix=core.windows.net"),
              executionAffinity: "5mins1",
              eventSource: new EventSourceTest(),
              tableName: "table5mins1",
              queueNamePrefix: "queue5mins1",
              encryptionUtility: null);

            dispatcherClient1.RegisterJobCallback(typeof(JobCallback5min));

            dispatcherClient1.ProvisionSystemConsistencyJob().Wait();

            dispatcherClient1.Start();



            var jobBuilder2 = JobBuilder.Create(jobPartition: "10mins1", jobId: "id10mins1")
              .WithCallback(typeof(JobCallback10min))
              .WithExecutionAffinities("10mins1")
              .WithRepeatStrategy(2, TimeSpan.FromMinutes(10))
              .WithRetryStrategy(2, TimeSpan.FromMinutes(1));

            var managementClient2 = new JobManagementClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjstemp3;AccountKey=D17j5l15vII4xYvg0Q3eHnyx7E92ywx6uq25e2gjKUEGLO7mzS6aYdf/4yL+dResCXszZL8W0LsF+AStNSTMUw==;EndpointSuffix=core.windows.net"),
              executionAffinity: "10mins1",
              eventSource: new EventSourceTest(),
              queueNamePrefix: "queue10mins1",
              tableName: "table10mins1",
              encryptionUtility: null);

            await managementClient2
              .CreateOrUpdateJob(jobBuilder2)
              .ConfigureAwait(continueOnCapturedContext: false);

            var dispatcherClient2 = new JobDispatcherClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjstemp3;AccountKey=D17j5l15vII4xYvg0Q3eHnyx7E92ywx6uq25e2gjKUEGLO7mzS6aYdf/4yL+dResCXszZL8W0LsF+AStNSTMUw==;EndpointSuffix=core.windows.net"),
              executionAffinity: "10mins1",
              tableName: "table10mins1",
              queueNamePrefix: "queue10mins1",
              eventSource: new EventSourceTest(),
              encryptionUtility: null);

            dispatcherClient2.RegisterJobCallback(typeof(JobCallback10min));

            dispatcherClient2.ProvisionSystemConsistencyJob().Wait();

            dispatcherClient2.Start();

            ScheduleJobClass scheduleJob = new ScheduleJobClass();
            scheduleJob.scheduleAJob(100, 7, typeof(JobCallback7min));
            scheduleJob.scheduleAJob(150, 12, typeof(JobCallback12min));
            scheduleJob.scheduleAJob(15, 15, typeof(JobCallback15min));
            scheduleJob.scheduleAJob(50, 18, typeof(JobCallback18min));
            scheduleJob.scheduleAJob(20, 20, typeof(JobCallback20min));
            scheduleJob.scheduleAJob(25, 25, typeof(JobCallback25min));

            return "BJS Execution: " + DateTime.Now;
        }
    }
}