using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSMultiDispatchTemp.Controllers
{
    [JobCallback(Name = "JobCallback25min")]
    public class JobCallback25min: JobDelegate
    {
        public async override Task<JobExecutionResult> ExecuteAsync(JobExecutionContext context, CancellationToken token)
        {
            Console.WriteLine("I am CallBack for 25 mins: " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("I am CallBack for 25 mins: " + DateTime.Now);
            string fullPath = String.Format("C:\\Users\\akritijain\\Desktop\\OutputSameStorage1\\25min.txt");
            using (StreamWriter writer = File.AppendText(fullPath))
            {
                writer.WriteLine("I am CallBack for 25 mins: " + DateTime.Now);
            }

            return await Task.FromResult(new JobExecutionResult { Status = JobExecutionStatus.Succeeded });
        }
    }
}
