using System;
using TechTalk.SpecFlow;
using System.Configuration;
using NUnit.Framework;
using System.Collections;

namespace DataProcessor.Integration.Tests
{
    [Binding]
    public class DownloadingDataFromTheDropPointSteps
    {

        private string _ftpPassword { get; set; }
        private string _ftpUsername { get; set; }
        private string _ftpUrl { get; set; }

        [Given(@"I have the credentials of the ftp site")]
        public void GivenIHaveTheCredentialsOfTheFtpSite()
        {
            var privateSettings = (IDictionary) ConfigurationSettings.GetConfig("privateSettings");
            _ftpUsername = (string) privateSettings["FtpUsername"];
            _ftpPassword = (string) privateSettings["FtpUsername"];
            _ftpUrl = (string) privateSettings["FtpUrl"];
            if (string.IsNullOrEmpty(_ftpUsername)) { Assert.Inconclusive("ftp username not set"); }
            if (string.IsNullOrEmpty(_ftpPassword)) { Assert.Inconclusive("ftp password not set"); }
            if (string.IsNullOrEmpty(_ftpUrl)) { Assert.Inconclusive("ftp url not set"); }
        }
        
        [Given(@"there are files waiting there")]
        public void GivenThereAreFilesWaitingThere()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I access the site")]
        public void WhenIAccessTheSite()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I download the files to a local directory")]
        public void ThenIDownloadTheFilesToALocalDirectory()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"Remove the files from the ftp site")]
        public void ThenRemoveTheFilesFromTheFtpSite()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
