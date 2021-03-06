﻿using System;
using NUnit.Framework;
using SolarApp.Model;
using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using Newtonsoft.Json;
using SolarApp.DataProcessor.Unit.Tests.Properties;
using Rhino.Mocks;
using SolarApp.Persistence;
using System.Linq;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor.Unit.Tests
{
	[TestFixture]
	public class LocalFileProcessorTest
	{

		[Test]
		public void Given_SomeLocalFiles_When_Process_Then_AllFilesAreProcessed()
		{
			// Arrange
			var configuration = MockRepository.GenerateMock<IConfiguration>();
			var solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
			var fileSystem = MockRepository.GenerateMock<IFileSystem>();
			var logger = MockRepository.GenerateMock<ILogger>();
			string[] filesToProcess = { "D.log", "E.log" };
			string pollFilePath = "C:/folder";
			DataPoint dataPoint = new DataPoint();
			configuration.Expect(i => i.NewFilePollPath).Return(pollFilePath);
			fileSystem.Expect(f => f.Directory_Exists(Arg<string>.Is.Anything)).Return(true);
            fileSystem.Expect(f => f.Directory_GetFiles(pollFilePath, "Log*.log")).Return(filesToProcess);
            fileSystem.Expect(f => f.Directory_GetFiles(pollFilePath, "*.json")).Return(new string[0]);
			fileSystem.Expect(f => f.File_Exists(Arg<string>.Is.Anything)).Return(false);
			foreach(var fileToProcess in filesToProcess){
				fileSystem.Expect(f => f.File_ReadAllText(Arg<string>.Is.Equal(fileToProcess))).Return(JsonConvert.SerializeObject(dataPoint));
				fileSystem.Expect(f => f.GetFileNameFromFullPath(Arg<string>.Is.Equal(fileToProcess))).Return("A.log");
				fileSystem.Expect(f => f.File_Move(Arg<string>.Is.Equal(fileToProcess), Arg<string>.Is.Anything));				
			}
			solarAppContext.Expect(c => c.InsertDataPoint(Arg<DataPoint>.Is.Anything)).Repeat.Times(filesToProcess.Length);

			// Act
			var ftpFileProcessor = new LocalFileProcessor(configuration, fileSystem, solarAppContext, logger);
			var results = ftpFileProcessor.Process();

			// Assert
			Assert.AreEqual(filesToProcess.Length, results.Count);
			configuration.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();
			fileSystem.VerifyAllExpectations();
			logger.VerifyAllExpectations();
				
			}

	}
}
