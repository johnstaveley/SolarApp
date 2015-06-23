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
using DataProcessor.Utility.Interfaces;

namespace DataProcessor
{
	public class LocalFileProcessor
	{

		private IConfiguration _configuration { get; set; }
		private IFileSystem _fileSystem { get; set; }
		private ISolarAppContext _context { get; set; }

		public LocalFileProcessor(IConfiguration configuration, IFileSystem fileSystem, ISolarAppContext context)
		{
			_configuration = configuration;
			_context = context;
			_fileSystem = fileSystem;
		}

		public List<string> Process()
		{
			var dataPointIds = new List<string>();
			var pollPath  = _configuration.NewFilePollPath;
			var archivePath = Path.Combine(pollPath, "Archive");
			if (!_fileSystem.Directory_Exists(archivePath)) { 
				_fileSystem.CreateDirectory(archivePath);
			}
			var filesToProcess = _fileSystem.Directory_GetFiles(pollPath, "Log*.log");
			foreach (string fileToProcess in filesToProcess)
			{
				string fileText = _fileSystem.File_ReadAllText(fileToProcess);
				string fileName = _fileSystem.GetFileNameFromFullPath(fileToProcess);
				string archiveFileName = Path.Combine(archivePath, fileName);
				if (_fileSystem.File_Exists(archiveFileName))
				{
					_fileSystem.File_Delete(archiveFileName);
				}
				_fileSystem.File_Move(fileToProcess, archiveFileName);
				DataPoint dataPoint = JsonConvert.DeserializeObject<DataPoint>(fileText);
				dataPoint.Id = _fileSystem.GetFileNameFromFullPath(fileName);
				dataPointIds.Add(dataPoint.Id);
				_context.InsertDataPoint(dataPoint);
			}
			return dataPointIds;

		}

	}
}
