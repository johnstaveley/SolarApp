using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Persistence;
using SolarApp.Utility.Interfaces;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace SolarApp.DataProcessor
{
	public partial class SolarAppService : ServiceBase
    {
		private readonly AutoResetEvent _idle = new AutoResetEvent(true);
		private readonly ITimer _timer;
		private readonly IConfiguration _configuration;
		private readonly IFileSystem _fileSystem;
		private readonly IFtp _ftp;
		private readonly ISolarAppContext _context;
		private readonly IServices _services;
		private readonly ILogger _logger;

		public SolarAppService(IConfiguration configuration, IFileSystem fileSystem, IFtp ftp, ILogger logger, ISolarAppContext context, IServices services, ITimer timer)
        {
			_configuration = configuration;
			_context = context;
			_fileSystem = fileSystem;
			_ftp = ftp;
			_logger = logger;
            _services = services;
			_timer = timer;
			_timer.Tick += TimerTick;
			_timer.Enabled = false;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
			_logger.Debug("On Start");
            this.RequestAdditionalTime(30000);
			Init();
        }

		public void Init()
		{
			// TODO: Start timer that triggers events
			_logger.Debug("Init called");
			_timer.Interval = _configuration.PollIntervalSeconds * 1000;
			_timer.Start();
		}

        protected override void OnStop()
        {
			_logger.Debug("Stop called");
			_timer.Stop();
        }

		public void TimerTick(object state)
		{
			_idle.Reset();
			_timer.Enabled = false;

			try
			{
				_logger.Debug("Tick");
				if (_context.IsDatabasePresent)
				{
					if (!_context.IsDatabaseSeeded)
					{
						_context.SeedDatabase();
					}

					//var ftpFileProcessor = new FtpFileProcessor(_configuration, _context, _ftp, _logger);
					//ftpFileProcessor.Process();

					var localFileProcessor = new LocalFileProcessor(_configuration, _fileSystem, _context, _logger);
					localFileProcessor.Process();

					//var weatherProcessor = new WeatherProcessor(_configuration, _context, _logger, _services);
					//weatherProcessor.GetWeatherForecast();
					//weatherProcessor.GetWeatherObservation();

                    _context.UpdateLastRunDate();
				}
				else
				{
					_logger.Debug("TimerTick - Database not present");
				}

			}
			catch (Exception ex)
			{
				_logger.Error(string.Format("{0}-{1}-{2}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message));
				//var audit = new Model.Audit(System.Environment.UserName, ex.Message, string.Format("{0}-{1}", this.GetType().Name, MethodBase.GetCurrentMethod().Name), true);
			}

			_timer.Enabled = true;
			_idle.Set();
		}

    }
}
