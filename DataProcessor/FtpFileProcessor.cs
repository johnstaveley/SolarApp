using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Persistence;
using Newtonsoft.Json;
using Model;
using DataProcessor.Utility;

namespace DataProcessor
{
	public class FtpFileProcessor
	{

		private IConfiguration _configuration { get; set; }
		private IFileSystem _fileSystem { get; set; }
		private IFtp _ftp { get; set; }
		private ISolarAppContext _context { get; set; }

		public FtpFileProcessor(IConfiguration configuration, ISolarAppContext context, IFileSystem fileSystem, IFtp ftp)
		{
			_configuration = configuration;
			_context = context;
			_fileSystem = fileSystem;
			_ftp = ftp;
		}

		/// <summary>
		/// Get list of remote files aand download them
		/// </summary>
		/// <returns></returns>
		public void Process()
		{

			var filesToDownload = _ftp.GetDirectoryListing();
			foreach (var fileToDownload in filesToDownload)
			{
				_ftp.Download(fileToDownload, _configuration.NewFilePollPath);
			}
		}

	}
}
