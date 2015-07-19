using System;
using TechTalk.SpecFlow;
using System.Configuration;
using NUnit.Framework;
using System.Collections;
using SolarApp.DataProcessor.Utility;
using System.IO;
using System.Linq;
using SolarApp.DataProcessor.Tests.Helper;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.DataProcessor.Utility.Classes;
using System.Collections.Generic;
using SolarApp.Persistence;

namespace SolarApp.DataProcessor.Integration.Tests
{
    [Binding]
    public class DownloadingDataFromTheDropPointSteps
    {

		[BeforeScenario]
		public void ScenarioSetup()
		{
			IConfiguration configuration = new SolarApp.DataProcessor.Utility.Classes.Configuration();
			configuration.MongoDatabaseName = "Test";
			if (!Directory.Exists(configuration.NewFilePollPath))
			{
				Directory.CreateDirectory(configuration.NewFilePollPath);
			}
			ScenarioContext.Current.Set<IServices>(new Services());
			ScenarioContext.Current.Set<IConfiguration>(configuration);
			ScenarioContext.Current.Set<List<DataItem>>(new List<DataItem>(), "DataItemsToTrack");
			var fileSystem = new FileSystem();
			ScenarioContext.Current.Set<IFileSystem>(fileSystem);
			ScenarioContext.Current.Set<IFtp>(new Ftp(configuration, fileSystem));
			var context = new SolarAppContext(configuration);
			ScenarioContext.Current.Set<ISolarAppContext>(context);
			var forecastRequestSetting = context.FindSettingById("RequestWeatherForecast");
			if (forecastRequestSetting == null)
			{
				forecastRequestSetting = new Model.Setting();
				forecastRequestSetting.Id = "RequestWeatherForecast";
				forecastRequestSetting.Value = "0";
				context.InsertSetting(forecastRequestSetting);
			}
			var observationRequestSetting = context.FindSettingById("RequestWeatherObservation");
			if (observationRequestSetting == null)
			{
				observationRequestSetting = new Model.Setting();
				observationRequestSetting.Id = "RequestWeatherObservation";
				observationRequestSetting.Value = "0";
				context.InsertSetting(observationRequestSetting);
			}
		}

		[AfterScenario]
		public void ScenarioCleanup()
		{
			// Remove tracked items from the database
			var dataItemsToTrack = ScenarioContext.Current.Get<List<DataItem>>("DataItemsToTrack");
			var context = ScenarioContext.Current.Get<ISolarAppContext>();
			foreach (var dataItem in dataItemsToTrack)
			{
				switch (dataItem.TableTypeKind)
				{
					case (TableTypeKind.DataPoint):
						context.DeleteDataPointById(dataItem.Id);
						context.DeleteFailedDataById(dataItem.Id);
						break;
					case (TableTypeKind.Setting):
						context.DeleteSettingById(dataItem.Id);
						break;
					case (TableTypeKind.WeatherForecast):
						context.DeleteWeatherForecastById(dataItem.Id);
						break;
					case (TableTypeKind.WeatherObservation):
						context.DeleteWeatherObservationById(dataItem.Id);
						break;
					default:
						throw new Exception(string.Format("Unable to determine type of artifact to delete for id ", dataItem.Id));
				}
			}
		}

        [Given(@"I have the credentials of the ftp site")]
        public void GivenIHaveTheCredentialsOfTheFtpSite()
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			if (string.IsNullOrEmpty(configuration.FtpUsername)) { Assert.Inconclusive("ftp username not set"); }
			if (string.IsNullOrEmpty(configuration.FtpPassword)) { Assert.Inconclusive("ftp password not set"); }
			if (string.IsNullOrEmpty(configuration.FtpDestinationUrl)) { Assert.Inconclusive("ftp url not set"); }
		}
        
        [When(@"I access the site")]
        public void WhenIAccessTheSite()
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			var fileSystem = ScenarioContext.Current.Get<IFileSystem>();
			var ftp = ScenarioContext.Current.Get<IFtp>();
			ftp = new Ftp(configuration, fileSystem);
        }

        [When(@"I do a directory listing")]
        [Then(@"I do a directory listing")]
        public void ThenIDoADirectoryListing()
        {
			var ftp = ScenarioContext.Current.Get<IFtp>();
			var logFiles = ftp.GetDirectoryListing();
            ScenarioContext.Current.Add("LogFileNames", logFiles);
        }

        [Then(@"There are files of the right format")]
        public void ThenThereAreFilesOfTheRightFormat()
        {
            string[] logFiles = ScenarioContext.Current.Get<string[]>("LogFileNames");
            Assert.AreNotEqual(0, logFiles.Length);
            Assert.IsTrue(logFiles[0].Contains(string.Format("Log{0}", DateTime.Now.Year)));
            Assert.IsTrue(logFiles[0].Contains(".log"));
        }

        [Given(@"I want to navigate to a subdirectory of the ftp site '(.*)'")]
        public void GivenIWantToNavigateToASubdirectoryOfTheFtpSite(string ftpSubDirectory)
        {
			var configuration = ScenarioContext.Current.Get<IConfiguration>();
			Uri baseUri = new Uri(configuration.FtpDestinationUrl);
            Uri uriWithSubDirectory = new Uri(baseUri, ftpSubDirectory + "/");
			configuration.FtpDestinationUrl = uriWithSubDirectory.AbsoluteUri.ToString();
        }

        [Given(@"the local temp directory '(.*)' is empty")]
        public void GivenTheLocalTempDirectoryIsEmpty(string localTempDirectory)
        {
            var localStoragePath = GetLocalStoragePath(localTempDirectory);
            if (Directory.Exists(localStoragePath)) { Directory.Delete(localStoragePath, true); }
            Directory.CreateDirectory(localStoragePath);
            ScenarioContext.Current.Add("LocalStoragePath", localStoragePath);
        }

        [When(@"there is a file '(.*)' waiting with text '(.*)'")]
        public void GivenThereIsAFileWaiting(string fileName, string contents)
        {
			var ftp = ScenarioContext.Current.Get<IFtp>();
            ftp.Upload(fileName, contents);
        }

        [Then(@"I download the file '(.*)' to a local directory")]
        public void ThenIDownloadTheFileToALocalDirectory(string fileToDownload)
        {
            var localStoragePath = ScenarioContext.Current.Get<string>("LocalStoragePath");
			var ftp = ScenarioContext.Current.Get<IFtp>();
            ftp.Download(fileToDownload, localStoragePath);
        }

        [Then(@"The file '(.*)' is stored in the '(.*)' directory")]
        public void ThenTheFileIsStoredInTheDirectory(string fileToDownload, string localTempDirectory)
        {
            var localStoragePath = GetLocalStoragePath(localTempDirectory);
            var filePath = Path.Combine(localStoragePath, fileToDownload);
            Assert.IsTrue(File.Exists(filePath), string.Format("File {0} does not exist", filePath));
            Assert.IsTrue(File.ReadAllText(filePath).Contains("test download file"), "File is not correct and does not contain the right text");
        }

        [When(@"I delete the file '(.*)'")]
        public void WhenIDeleteTheFile(string fileToDelete)
        {
			var ftp = ScenarioContext.Current.Get<IFtp>();
            ftp.Delete(fileToDelete);
        }

        [Then(@"The file list does not contain the file '(.*)'")]
        public void ThenTheFileListDoesNotContainTheFile(string fileToCheckFor)
        {
            var logFiles = ScenarioContext.Current.Get<string[]>("LogFileNames");
            Assert.IsFalse(logFiles.ToList().Any(l => l.Contains(fileToCheckFor)), string.Format("File {0} has been found to exist", fileToCheckFor));
        }

        private string GetLocalStoragePath(string subDirectory)
        {
            var tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, subDirectory) + "/";
        }

    }
}
