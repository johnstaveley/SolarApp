using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using DataProcessor.Utility;
using Persistence;
using DataProcessor.Utility.Interfaces;

namespace DataProcessor
{
    public partial class SolarAppService : ServiceBase
    {
		private ITimer _timer { get; set; }
		private IConfiguration _configuration { get; set; }
		private IFileSystem _fileSystem { get; set; }
		private IFtp _ftp { get; set; }
		private ISolarAppContext _context { get; set; }

		public SolarAppService(ITimer timer)
        {
			_timer = timer;
			_timer.Tick += TimerTick;
			_timer.Enabled = false;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
			Init();
        }

		public void Init()
		{
			// TODO: Start timer that triggers events
			_timer.Start();
		}

        protected override void OnStop()
        {
			_timer.Stop();
        }

		public void TimerTick(object state)
		{
			//_idle.Reset();
			_timer.Enabled = false;

			try
			{
				//var ftpFileProcessor = new FtpFileProcessor();
				//ftpFileProcessor.Process();

				//var localFileProcessor = new LocalFileProcessor();
				//localFileProcessor.Process();

			}
			catch (Exception ex)
			{
				//Logger.Error("Unhandled exception occurred.", ex);
			}

			_timer.Enabled = true;
//			_idle.Set();
		}
    }
}
