using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor.Utility
{
    public class Ftp
    {

        public string _destinationUrl { get; set; }
        public string _username { get; set; }
        public string _password { get; set; }

        public Ftp(string destinationUrl, string username, string password)
        {
            _destinationUrl = destinationUrl;
            _password = password;
            _username = username;
        }

        private FtpWebRequest InitialiseConnection()
        {
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

        public string Download() {

            FtpWebRequest request = InitialiseConnection();
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = GetResponse(request);
            return response;
        }

        public string[] GetDirectoryListing()
        {

            FtpWebRequest request = InitialiseConnection();
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = GetResponse(request);
            return response.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }


    }
}
