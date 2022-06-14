using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSMultiDispatchTemp.Controllers
{
    public class ScheduleJobClass
    {
        public async void scheduleAJob(int count, int time, Type callback)
        {
            var affinity = time.ToString() + "mins1";
            var jobBuilder3 = JobBuilder.Create(jobPartition: time + "mins1", jobId: "id" + time + "mins1")
              .WithCallback(callback)
              .WithExecutionAffinities(affinity)
              .WithRepeatStrategy(count, TimeSpan.FromMinutes(time))
              .WithRetryStrategy(2, TimeSpan.FromMinutes(1));

            var managementClient3 = new JobManagementClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjstemp3;AccountKey=D17j5l15vII4xYvg0Q3eHnyx7E92ywx6uq25e2gjKUEGLO7mzS6aYdf/4yL+dResCXszZL8W0LsF+AStNSTMUw==;EndpointSuffix=core.windows.net"),
              executionAffinity: affinity,
              eventSource: new EventSourceTest(),
              queueNamePrefix: "queue" + time + "mins1",
              tableName: "table" + time + "mins1",
              encryptionUtility: null);

            await managementClient3
              .CreateOrUpdateJob(jobBuilder3)
              .ConfigureAwait(continueOnCapturedContext: false);

            var dispatcherClient3 = new JobDispatcherClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjstemp3;AccountKey=D17j5l15vII4xYvg0Q3eHnyx7E92ywx6uq25e2gjKUEGLO7mzS6aYdf/4yL+dResCXszZL8W0LsF+AStNSTMUw==;EndpointSuffix=core.windows.net"),
              executionAffinity: affinity,
              tableName: "table" + time + "mins1",
              queueNamePrefix: "queue" + time + "mins1",
              eventSource: new EventSourceTest(),
              encryptionUtility: null);

            dispatcherClient3.RegisterJobCallback(callback);

            dispatcherClient3.ProvisionSystemConsistencyJob().Wait();

            dispatcherClient3.Start();
        }
    }
}
