using Newtonsoft.Json;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Model;
using SolarApp.Persistence;
using SolarApp.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public List<string> GetFilesToProcess(string pollPath)
        {
            var filesToProcess = _fileSystem.Directory_GetFiles(pollPath, "Log*.log").ToList();
            filesToProcess.AddRange(_fileSystem.Directory_GetFiles(pollPath, "*.json").ToList());
            return filesToProcess;
        }

        public List<ProcessedFile> Process()
        {
            var processedFiles = new List<ProcessedFile>();
            var pollPath = _configuration.NewFilePollPath;
            var archivePath = Path.Combine(pollPath, "Archive");
            if (!_fileSystem.Directory_Exists(archivePath))
            {
                _fileSystem.CreateDirectory(archivePath);
            }
            var filesToProcess = GetFilesToProcess(pollPath);
            foreach (string fileToProcess in filesToProcess)
            {
                string fileText = _fileSystem.File_ReadAllText(fileToProcess);
                string fileName = _fileSystem.GetFileNameFromFullPath(fileToProcess);
                ProcessedFile processedFile = new ProcessedFile();
                try
                {
                    string archiveFileName = Path.Combine(archivePath, fileName);
                    if (_fileSystem.File_Exists(archiveFileName))
                    {
                        _fileSystem.File_Delete(archiveFileName);
                    }
                    _fileSystem.File_Move(fileToProcess, archiveFileName);
                    processedFile.FileKind = DetermineFileKind(fileText);
                    processedFile.Id = fileName;
                    switch (processedFile.FileKind)
                    {
                        case FileKind.DataPoint:
                            DataPoint dataPoint = JsonConvert.DeserializeObject<DataPoint>(fileText);
                            dataPoint.Id = fileName;
                            if (dataPoint.IsValid())
                            {
                                _context.InsertDataPoint(dataPoint);
                            }
                            else
                            {
                                processedFile.FileKind = FileKind.Unknown;
                            }
                            break;
                        case FileKind.WeatherForecast:
                            WeatherForecast weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(fileText);
                            weatherForecast.Id = fileName;
                            if (weatherForecast.IsValid())
                            {
                                _context.InsertWeatherForecast(weatherForecast);
                            }
                            else
                            {
                                 processedFile.FileKind = FileKind.Unknown;
                            }
                            break;
                        default:
                            // invalid
                            break;
                    }
                    if (processedFile.FileKind == FileKind.Unknown)
                    {
                        _context.InsertFailedData(new FailedData() { Id = fileName, Data = fileText });
                    }
                }
                catch (Exception exception)
                {
                    _logger.ErrorFormat("Error processing message {0}", exception.Message);
                    _context.InsertFailedData(new FailedData() { Id = fileName, Data = fileText });
                }
                processedFiles.Add(processedFile);
            }
            return processedFiles;

        }

        public FileKind DetermineFileKind(string fileText)
        {
            if (fileText.Contains("RequestArguments")) return FileKind.DataPoint;
            if (fileText.Contains("Wx") && fileText.Contains("DV")) return FileKind.WeatherForecast;
            return FileKind.Unknown;
        }

    }

    public enum FileKind
    {
        Unknown = 0,
        DataPoint = 1,
        WeatherForecast = 2
    }

    public class ProcessedFile
    {
        public FileKind FileKind { get; set; }

        public string Id { get; set; }

    }

}
