using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SolarApp.Persistence;
using Newtonsoft.Json;
using SolarApp.Model;
using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor
{
	public class LocalFileProcessor
	{

		private readonly IConfiguration _configuration;
		private readonly IFileSystem _fileSystem;
		private readonly ISolarAppContext _context;
		private readonly ILogger _logger;

		public LocalFileProcessor(IConfiguration configuration, IFileSystem fileSystem, ISolarAppContext context, ILogger logger)
		{
			_configuration = configuration;
			_context = context;
			_fileSystem = fileSystem;
			_logger = logger;
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
                try
                {
                    string archiveFileName = Path.Combine(archivePath, fileName);
                    if (_fileSystem.File_Exists(archiveFileName))
                    {
                        _fileSystem.File_Delete(archiveFileName);
                    }
                    _fileSystem.File_Move(fileToProcess, archiveFileName);
                    DataPoint dataPoint = JsonConvert.DeserializeObject<DataPoint>(fileText);
                    dataPoint.Id = fileName;
                    dataPointIds.Add(dataPoint.Id);
                    if (dataPoint.Head.Status.Code == 0 && dataPoint.Head.Status.Reason == "" && dataPoint.Head.Status.UserMessage == "" &&
                        dataPoint.Head.RequestArguments.Query == "Inverter" && dataPoint.Head.RequestArguments.Scope == "System")
                    {
                        _context.InsertDataPoint(dataPoint);
                    }
                    else
                    {
                        _context.InsertFailedData(new FailedData() { Id = fileName, Data = fileText });
                    }
                }
                catch (Exception exception)
                {
					_logger.ErrorFormat("Error processing message {0}", exception.Message);
                    _context.InsertFailedData(new FailedData() { Id = fileName, Data = fileText });
                    dataPointIds.Add(fileName);
                }
			}
			return dataPointIds;

		}

	}
}
