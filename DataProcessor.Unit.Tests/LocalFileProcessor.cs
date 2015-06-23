using System;
using NUnit.Framework;
using Model;
using DataProcessor.Utility;
using DataProcessor.Utility.Interfaces;
using Newtonsoft.Json;
using SolarApp.DataProcessor.Unit.Tests.Properties;
using Rhino.Mocks;
using Persistence;
using System.Linq;

namespace DataProcessor.Unit.Tests
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
			string[] filesToProcess = { "D.log", "E.log" };
			string pollFilePath = "C:/folder";
			configuration.Expect(i => i.NewFilePollPath).Return(pollFilePath);
			fileSystem.Expect(f => f.Directory_Exists(Arg<string>.Is.Anything)).Return(true);
			fileSystem.Expect(f => f.Directory_GetFiles(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(filesToProcess);
			fileSystem.Expect(f => f.File_Exists(Arg<string>.Is.Anything)).Return(false);
			foreach(var fileToProcess in filesToProcess){
				fileSystem.Expect(f => f.File_ReadAllText(Arg<string>.Is.Equal(fileToProcess))).Return("{}");
				fileSystem.Expect(f => f.GetFileNameFromFullPath(Arg<string>.Is.Equal(fileToProcess))).Return("A.log");
				fileSystem.Expect(f => f.File_Move(Arg<string>.Is.Equal(fileToProcess), Arg<string>.Is.Anything));				
			}
			solarAppContext.Expect(c => c.InsertDataPoint(Arg<DataPoint>.Is.Anything)).Repeat.Times(filesToProcess.Length);

			// Act
			var ftpFileProcessor = new LocalFileProcessor(configuration, fileSystem, solarAppContext);
			var results = ftpFileProcessor.Process();

			// Assert
			Assert.AreEqual(filesToProcess.Length, results.Count);
			configuration.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();
			fileSystem.VerifyAllExpectations();		
				
			}

	}
}
