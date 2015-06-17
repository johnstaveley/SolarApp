using System;
namespace DataProcessor.Utility
{
	public interface IFileSystem
	{
		void CreateDirectory(string path);
		bool Directory_Exists(string path);
		string[] Directory_GetFiles(string path, string searchPattern);
		void File_Delete(string path);
		bool File_Exists(string path);
		void File_Move(string sourceFileName, string destFileName);
		string File_ReadAllText(string path);
		string GetDirectoryName(string path);
		string GetFileNameFromFullPath(string path);
	}
}
