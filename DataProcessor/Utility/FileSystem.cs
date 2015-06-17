using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataProcessor.Utility
{
	public class FileSystem : DataProcessor.Utility.IFileSystem
	{

		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public bool Directory_Exists(string path)
		{
			return Directory.Exists(path);
		}

		public string[] Directory_GetFiles(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern);
		}

		public void File_Delete(string path)
		{
			File.Delete(path);
		}

		public bool File_Exists(string path)
		{
			return File.Exists(path);
		}

		public void File_Move(string sourceFileName, string destFileName)
		{
			File.Move(sourceFileName, destFileName);
		}

		public string File_ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}

		public string GetDirectoryName(string path)
		{
			return new FileInfo(path).DirectoryName;
		}

		public string GetFileNameFromFullPath(string path)
		{
			return new FileInfo(path).Name;
		}

	}
}
