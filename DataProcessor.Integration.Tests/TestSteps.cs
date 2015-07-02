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

namespace SolarApp.DataProcessor.Integration.Tests
{
    [Binding]
    public class DownloadingDataFromTheDropPointSteps
    {

		private IConfiguration _configuration;
        private IFileSystem _fileSystem = new FileSystem();
        private Ftp _ftp { get; set; }

        [Given(@"I have the credentials of the ftp site")]
        public void GivenIHaveTheCredentialsOfTheFtpSite()
        {
			_configuration = new SolarApp.DataProcessor.Utility.Classes.Configuration();
			if (string.IsNullOrEmpty(_configuration.FtpUsername)) { Assert.Inconclusive("ftp username not set"); }
			if (string.IsNullOrEmpty(_configuration.FtpPassword)) { Assert.Inconclusive("ftp password not set"); }
			if (string.IsNullOrEmpty(_configuration.FtpDestinationUrl)) { Assert.Inconclusive("ftp url not set"); }
		}
        
        [When(@"I access the site")]
        public void WhenIAccessTheSite()
        {
			_ftp = new Ftp(_configuration, _fileSystem);
        }

        [When(@"I do a directory listing")]
        [Then(@"I do a directory listing")]
        public void ThenIDoADirectoryListing()
        {
            var logFiles = _ftp.GetDirectoryListing();
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
			Uri baseUri = new Uri(_configuration.FtpDestinationUrl);
            Uri uriWithSubDirectory = new Uri(baseUri, ftpSubDirectory + "/");
			_configuration.FtpDestinationUrl = uriWithSubDirectory.AbsoluteUri.ToString();
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
            _ftp.Upload(fileName, contents);
        }

        [Then(@"I download the file '(.*)' to a local directory")]
        public void ThenIDownloadTheFileToALocalDirectory(string fileToDownload)
        {
            var localStoragePath = ScenarioContext.Current.Get<string>("LocalStoragePath");
            _ftp.Download(fileToDownload, localStoragePath);
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
            _ftp.Delete(fileToDelete);
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
