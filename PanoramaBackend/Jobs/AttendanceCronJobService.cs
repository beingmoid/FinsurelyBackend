using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PanoramaBackend.Api.Jobs
{
    public class AttendanceCronJobService : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;

        private string Schedule => "* * */18 * * *"; //Runs every 4 hours
        //private string Schedule => "* */2 * * * *"; //Runs every 2 minute
        public AttendanceCronJobService()
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Attendance Cron Job!");
            await Task.Delay(60000, stoppingToken); //1 Minute delay
            do
                {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                   await Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }
        private async Task Process()
        {
            using var serviceScope = ServiceActivator.GetScope();
            var attendance = serviceScope.ServiceProvider.GetRequiredService<IAttendanceService>();
            await attendance.ProcessAbsentees();
        }
    }
   
}
