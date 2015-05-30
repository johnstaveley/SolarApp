﻿using System;
using TechTalk.SpecFlow;
using System.Configuration;
using NUnit.Framework;
using System.Collections;
using DataProcessor.Utility;
using System.IO;
using System.Linq;

namespace DataProcessor.Integration.Tests
{
    [Binding]
    public class DownloadingDataFromTheDropPointSteps
    {

        private string _ftpPassword { get; set; }
        private string _ftpUsername { get; set; }
        private string _ftpUrl { get; set; }
        private Ftp _ftp { get; set; }

        [Given(@"I have the credentials of the ftp site")]
        public void GivenIHaveTheCredentialsOfTheFtpSite()
        {
            var privateSettings = (IDictionary) ConfigurationManager.GetSection("privateSettings");
            _ftpUsername = (string) privateSettings["FtpUsername"];
            _ftpPassword = (string) privateSettings["FtpPassword"];
            _ftpUrl = (string) privateSettings["FtpUrl"];
        }
        
        [When(@"I access the site")]
        public void WhenIAccessTheSite()
        {
            if (string.IsNullOrEmpty(_ftpUsername)) { Assert.Inconclusive("ftp username not set"); }
            if (string.IsNullOrEmpty(_ftpPassword)) { Assert.Inconclusive("ftp password not set"); }
            if (string.IsNullOrEmpty(_ftpUrl)) { Assert.Inconclusive("ftp url not set"); }
            _ftp = new Ftp(_ftpUrl, _ftpUsername, _ftpPassword);
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
            Uri baseUri = new Uri(_ftpUrl);
            Uri uriWithSubDirectory = new Uri(baseUri, ftpSubDirectory + "/");
            _ftpUrl = uriWithSubDirectory.AbsoluteUri.ToString();
        }

        [Given(@"the local temp directory '(.*)' is empty")]
        public void GivenTheLocalTempDirectoryIsEmpty(string localTempDirectory)
        {
            var localStoragePath = GetLocalStoragePath(localTempDirectory);
            if (Directory.Exists(localStoragePath)) { Directory.Delete(localStoragePath, true); }
            Directory.CreateDirectory(localStoragePath);
            ScenarioContext.Current.Add("LocalStoragePath", localStoragePath);
        }

        [Given(@"there is a file '(.*)' waiting in the '(.*)' subdirectory")]
        public void GivenThereIsAFileWaitingInTheSubdirectory(string fileToDownload, string ftpSubDirectory)
        {
            // TODO: Assumed the file is present
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
            Assert.IsTrue(File.ReadAllText(filePath).Contains("TOTAL_ENERGY"), "File is not correct and does not contain the right text");
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