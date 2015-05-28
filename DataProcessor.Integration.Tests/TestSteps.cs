using System;
using TechTalk.SpecFlow;
using System.Configuration;
using NUnit.Framework;
using System.Collections;
using DataProcessor.Utility;

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
            var privateSettings = (IDictionary) ConfigurationSettings.GetConfig("privateSettings");
            _ftpUsername = (string) privateSettings["FtpUsername"];
            _ftpPassword = (string) privateSettings["FtpPassword"];
            _ftpUrl = (string) privateSettings["FtpUrl"];
            if (string.IsNullOrEmpty(_ftpUsername)) { Assert.Inconclusive("ftp username not set"); }
            if (string.IsNullOrEmpty(_ftpPassword)) { Assert.Inconclusive("ftp password not set"); }
            if (string.IsNullOrEmpty(_ftpUrl)) { Assert.Inconclusive("ftp url not set"); }
        }
        
        [Given(@"there are files waiting there")]
        public void GivenThereAreFilesWaitingThere()
        {
            
        }
        
        [When(@"I access the site")]
        public void WhenIAccessTheSite()
        {
           _ftp  = new Ftp(_ftpUrl, _ftpUsername, _ftpPassword);
        }
        
        [Then(@"I download the files to a local directory")]
        public void ThenIDownloadTheFilesToALocalDirectory()
        {
            var logFiles = _ftp.GetDirectoryListing();
            Assert.AreNotEqual(0, logFiles.Length);
            Assert.IsTrue(logFiles[0].Contains(string.Format("Log{0}", DateTime.Now.Year)));
            Assert.IsTrue(logFiles[0].Contains(".log"));
        }
        
        [Then(@"Remove the files from the ftp site")]
        public void ThenRemoveTheFilesFromTheFtpSite()
        {
        }
    }
}
