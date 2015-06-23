using System;
using NUnit.Framework;
using Model;
using DataProcessor.Utility;
using DataProcessor.Utility.Interfaces;
using Newtonsoft.Json;
using SolarApp.DataProcessor.Unit.Tests.Properties;
using Rhino.Mocks;
using Persistence;

namespace DataProcessor.Unit.Tests
{
	[TestFixture]
	public class FtpFileProcessorTest
	{

		[Test]
		public void Given_SomeFilesToDownload_When_ProcessRemoteFiles_Then_AllFilesAreDownload()
		{
			// Arrange
			var configuration = MockRepository.GenerateMock<IConfiguration>();
			var solarAppContext = MockRepository.GenerateMock<ISolarAppContext>();
			var fileSystem = MockRepository.GenerateMock<IFileSystem>();
			var ftp = MockRepository.GenerateMock<IFtp>();
			string[] filesToDownload = { "A", "B", "C" };
			ftp.Expect(i => i.GetDirectoryListing()).Return(filesToDownload);
			string pollFilePath = "C:/folder";
			configuration.Expect(i => i.NewFilePollPath).Return(pollFilePath);

			// Act
			var ftpFileProcessor = new FtpFileProcessor(configuration, solarAppContext, fileSystem, ftp);
			ftpFileProcessor.Process();

			// Assert
			configuration.VerifyAllExpectations();
			solarAppContext.VerifyAllExpectations();
			fileSystem.VerifyAllExpectations();

			foreach (var fileToDownload in filesToDownload)
			{
				ftp.AssertWasCalled(i => i.Download(Arg<string>.Is.Equal(fileToDownload), Arg<string>.Is.Equal(pollFilePath)));
			}
			ftp.VerifyAllExpectations();

		}
	}
}
