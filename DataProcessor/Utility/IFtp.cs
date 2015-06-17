using System;
namespace DataProcessor.Utility
{
	public interface IFtp
	{
		void Delete(string fileToDelete);
		void Download(string fileToDownload, string localStoragePath);
		string[] GetDirectoryListing();
	}
}
