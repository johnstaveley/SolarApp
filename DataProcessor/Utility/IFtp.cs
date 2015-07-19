using System;

namespace SolarApp.DataProcessor.Utility
{
	public interface IFtp
	{
		void Delete(string fileToDelete);
		void Download(string fileToDownload, string localStoragePath);
		string[] GetDirectoryListing();
		void Upload(string fileName, string contents);
	}
}
