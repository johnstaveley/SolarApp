using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SolarApp.DataProcessor.Utility.Interfaces;

namespace SolarApp.DataProcessor.Utility
{
	public class Ftp : SolarApp.DataProcessor.Utility.IFtp
    {

        private string _destinationUrl { get; set; }
		private string _username { get; set; }
		private string _password { get; set; }
        private IFileSystem _fileSystem { get; set; }

        public Ftp(IConfiguration configuration, IFileSystem fileSystem)
        {

			_destinationUrl = configuration.FtpDestinationUrl;
			_password = configuration.FtpPassword;
			_username = configuration.FtpUsername;
            _fileSystem = fileSystem;
        }

        private FtpWebRequest InitialiseConnection(string fileToDownload = null)
        {
            if (!string.IsNullOrEmpty(_destinationUrl)) {
                Uri baseUri = new Uri(_destinationUrl);
                Uri modifiedUri = new Uri(baseUri, fileToDownload);
                _destinationUrl = modifiedUri.AbsoluteUri.ToString();
            }
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_destinationUrl);
            request.Credentials = new NetworkCredential(_username, _password);
            return request;
        }

        private string GetResponse(FtpWebRequest request)
        {
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);
                    return reader.ReadToEnd();
                }
            }
        }

        public void Download(string fileToDownload, string localStoragePath) {

            FtpWebRequest request = InitialiseConnection(fileToDownload);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = GetResponse(request);
            if (!_fileSystem.Directory_Exists(localStoragePath)) { _fileSystem.CreateDirectory(localStoragePath); }
            var localFilePath = Path.Combine(localStoragePath, fileToDownload);
            File.WriteAllText(localFilePath, response);
        }

        public string[] GetDirectoryListing()
        {

            FtpWebRequest request = InitialiseConnection();
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = GetResponse(request);
            return response.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(s => s != String.Empty).ToArray();
        }

        public void Delete(string fileToDelete)
        {
            FtpWebRequest request = InitialiseConnection(fileToDelete);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            var response = GetResponse(request);
        }
    }
}
