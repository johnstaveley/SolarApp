using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Persistence;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor
{
	public class FtpFileProcessor
	{

		private readonly IConfiguration _configuration;
		private readonly IFtp _ftp;
		private readonly ISolarAppContext _context;
		private readonly ILogger _logger;

		public FtpFileProcessor(IConfiguration configuration, ISolarAppContext context, IFtp ftp, ILogger logger)
		{
			_configuration = configuration;
			_context = context;
			_ftp = ftp;
			_logger = logger;
		}

		/// <summary>
		/// Get list of remote files aand downloads them if not already downloaded, optionally deletes them off the server
		/// </summary>
		/// <returns></returns>
		public void Process()
		{

			var filesToDownload = _ftp.GetDirectoryListing();
			_logger.DebugFormat("{0} files at remote site", filesToDownload.Length);
			foreach (var fileToDownload in filesToDownload)
			{
                if (_context.FindDataPointById(fileToDownload) == null && _context.FindFailedDataById(fileToDownload) == null)
                {
                    _ftp.Download(fileToDownload, _configuration.NewFilePollPath);
                    if (_configuration.DeleteFileAfterDownload)
                    {
						_logger.DebugFormat("Deleting file {0}", fileToDownload);
						_ftp.Delete(fileToDownload);
                    }
                }
			}
		}

	}
}
