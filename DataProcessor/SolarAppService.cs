using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SolarApp.DataProcessor.Utility;
using SolarApp.Persistence;
using SolarApp.DataProcessor.Utility.Interfaces;
using System.Threading;

namespace SolarApp.DataProcessor
{
    public partial class SolarAppService : ServiceBase
    {
		private readonly AutoResetEvent _idle = new AutoResetEvent(true);
		private ITimer _timer { get; set; }
		private IConfiguration _configuration { get; set; }
		private IFileSystem _fileSystem { get; set; }
		private IFtp _ftp { get; set; }
		private ISolarAppContext _context { get; set; }
		private IServices _services { get; set; }

		public SolarAppService(IConfiguration configuration, IFileSystem fileSystem, IFtp ftp, ISolarAppContext context, ITimer timer)
        {
			_configuration = configuration;
			_context = context;
			_fileSystem = fileSystem;
			_ftp = ftp;
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
			_timer.Interval = 600000;
			_timer.Start();
			_context.SeedDatabase();
		}

        protected override void OnStop()
        {
			_timer.Stop();
        }

		public void TimerTick(object state)
		{
			_idle.Reset();
			_timer.Enabled = false;

			try
			{
                var ftpFileProcessor = new FtpFileProcessor(_configuration, _context, _fileSystem, _ftp);
                ftpFileProcessor.Process();

				var localFileProcessor = new LocalFileProcessor(_configuration, _fileSystem, _context);
				localFileProcessor.Process();

				var weatherProcessor = new WeatherProcessor(_configuration, _context, _services);
				weatherProcessor.Process();

			}
			catch (Exception ex)
			{
				//Logger.Error("Unhandled exception occurred.", ex);
			}

			_timer.Enabled = true;
			_idle.Set();
		}
    }
}
