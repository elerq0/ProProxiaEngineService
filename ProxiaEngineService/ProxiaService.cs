using System;
using System.ServiceProcess;
using System.Timers;

namespace ProxiaEngineService
{
    public partial class ProxiaService : ServiceBase
    {
        private Timer t;
        public ProxiaService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (args.Length > 1)
                OneTimeUse(null, null, args);

            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 19, 0, 0, 0);
            if (nowTime > scheduledTime)
                scheduledTime = scheduledTime.AddDays(1);

            t = new Timer
            {
                Interval = (scheduledTime - DateTime.Now).TotalMilliseconds
            };

            t.Elapsed += (sender, e) => OnTimer(sender, e, args);
            t.Start();
        }

        protected void OnTimer(object sender, ElapsedEventArgs e, string[] args)
        {
            t.Enabled = false;
            t.Interval = 24 * 60 * 60 * 1000;
            t.Enabled = true;

            App.Run(args);
        }

        protected void OneTimeUse(object sender, ElapsedEventArgs e, string[] args)
        {
            string[] args2 = { args[1] };
            App.Run(args2);
        }

        protected override void OnStop()
        {
        }
    }
}
