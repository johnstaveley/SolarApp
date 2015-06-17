using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor.Utility
{
    public class Ftp : DataProcessor.Utility.IFtp
    {

        private string _destinationUrl { get; set; }
		private string _username { get; set; }
		private string _password { get; set; }

        public Ftp(string destinationUrl, string username, string password)
        {
            _destinationUrl = destinationUrl;
            _password = password;
            _username = username;
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
            var localFilePath = Path.Combine(localStoragePath, fileToDownload);
            File.WriteAllText(localFilePath, response);
        }

        public string[] GetDirectoryListing()
        {

            FtpWebRequest request = InitialiseConnection();
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = GetResponse(request);
            return response.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }

        public void Delete(string fileToDelete)
        {
            FtpWebRequest request = InitialiseConnection(fileToDelete);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            var response = GetResponse(request);
        }
    }
}
