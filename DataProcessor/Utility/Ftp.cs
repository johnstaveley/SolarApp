using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SolarApp.DataProcessor.Utility.Interfaces;
using SolarApp.Utility.Interfaces;

namespace SolarApp.DataProcessor.Utility
{
	public class Ftp : SolarApp.DataProcessor.Utility.IFtp
    {

        private string _rootUrl { get; set; }
		private string _fileUrl { get; set; }
		private string _username { get; set; }
		private string _password { get; set; }
		private readonly IFileSystem _fileSystem;
		private readonly ILogger _logger;

        public Ftp(IConfiguration configuration, IFileSystem fileSystem, ILogger logger)
        {

			_rootUrl = configuration.FtpDestinationUrl;
			_password = configuration.FtpPassword;
			_username = configuration.FtpUsername;
            _fileSystem = fileSystem;
			_logger = logger;
        }

        private FtpWebRequest InitialiseConnection(string remoteFileName = null)
        {
			_fileUrl = null;
            if (!string.IsNullOrEmpty(_rootUrl)) {
                Uri baseUri = new Uri(_rootUrl);
                Uri modifiedUri = new Uri(baseUri, remoteFileName);
                _fileUrl = modifiedUri.AbsoluteUri.ToString();
			}
			var connectionUrl = _fileUrl ?? _rootUrl;
			_logger.DebugFormat("Making connection to {0}", connectionUrl);
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(connectionUrl);
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
					_logger.DebugFormat("Directory List Complete, status {0}", response.StatusDescription);
					return reader.ReadToEnd();
                }
            }
        }

        public void Download(string fileToDownload, string localStoragePath) {

            FtpWebRequest request = InitialiseConnection(fileToDownload);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
			_logger.DebugFormat("About to download {0}", fileToDownload);
			var response = GetResponse(request);
            if (!_fileSystem.Directory_Exists(localStoragePath)) { _fileSystem.CreateDirectory(localStoragePath); }
            var localFilePath = Path.Combine(localStoragePath, fileToDownload);
            File.WriteAllText(localFilePath, response);
			_logger.DebugFormat("Downloaded file to {0}", localFilePath);
		}

        /// <summary>
        /// Get a list of all the files in the directory
        /// </summary>
        /// <returns></returns>
        public string[] GetDirectoryListing()
        {

            FtpWebRequest request = InitialiseConnection();
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = GetResponse(request);
            return response.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(s => s != String.Empty && s.Contains(".")).ToArray();
        }

        public void Delete(string fileToDelete)
        {
            FtpWebRequest request = InitialiseConnection(fileToDelete);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            var response = GetResponse(request);
        }

        public void Upload(string fileName, string contents)
        {
            FtpWebRequest request = InitialiseConnection(fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            byte[] fileContents = Encoding.UTF8.GetBytes(contents);
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();
            var response = GetResponse(request);

        }

    }
}
