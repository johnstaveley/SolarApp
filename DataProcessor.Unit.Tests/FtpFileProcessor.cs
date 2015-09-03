using System;
using NUnit.Framework;
using SolarApp.Model;
using SolarApp.DataProcessor;
using SolarApp.DataProcessor.Utility;
using SolarApp.DataProcessor.Utility.Interfaces;
using Newtonsoft.Json;
using SolarApp.DataProcessor.Unit.Tests.Properties;
using Rhino.Mocks;
using SolarApp.Persistence;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor.Unit.Tests
{
	[TestFixture]
	public class FtpFileProcessorTest
	{

		[Test]
		public void Given_SomeFilesToDownloadAndTwoFilesAlreadyExist_When_ProcessRemoteFiles_Then_OneFileIsDownload()
		{
			// Arrange
			var configuration = MockRepository.GenerateMock<IConfiguration>();
			var solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
			var fileSystem = MockRepository.GenerateMock<IFileSystem>();
			var ftp = MockRepository.GenerateMock<IFtp>();
			var logger = MockRepository.GenerateMock<ILogger>();
			string[] filesToDownload = { "A", "B", "C" };
			ftp.Expect(i => i.GetDirectoryListing()).Return(filesToDownload);
			string pollFilePath = "C:/folder";
			configuration.Expect(i => i.NewFilePollPath).Return(pollFilePath);
            solarAppContext.Expect(s => s.FindDataPointById("A")).Return(new DataPoint());
            solarAppContext.Expect(s => s.FindDataPointById("B")).Return(null);
            solarAppContext.Expect(s => s.FindFailedDataById("B")).Return(new FailedData());
            solarAppContext.Expect(s => s.FindDataPointById("C")).Return(null);
            solarAppContext.Expect(s => s.FindFailedDataById("C")).Return(null); 
            configuration.DeleteFileAfterDownload = false;

			// Act
			var ftpFileProcessor = new FtpFileProcessor(configuration, solarAppContext, fileSystem, ftp, logger);
			ftpFileProcessor.Process();

			// Assert
			configuration.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();
			fileSystem.VerifyAllExpectations();

			ftp.AssertWasCalled(i => i.Download(Arg<string>.Is.Equal("C"), Arg<string>.Is.Equal(pollFilePath)));
            ftp.AssertWasNotCalled(f => f.Delete(Arg<string>.Is.Anything));
			ftp.VerifyAllExpectations();
			logger.VerifyAllExpectations();

		}
	}
}
