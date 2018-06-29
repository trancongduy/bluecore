using System;
using System.IO;

namespace Framework.Common.Helpers
{
	public static class FileHelper
	{
		public static void CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		public static void DeleteFile(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

		public static void DeleteImage(string path, string imageFile)
		{
			var indexString = imageFile.LastIndexOf(".", StringComparison.Ordinal);
			var fileName = indexString > 0 ? imageFile.Substring(0, indexString) : imageFile;

			var smallPath = $"{path}{fileName}_small.jpg";
			var mediumPath = $"{path}{fileName}_medium.jpg";
			var largePath = $"{path}{fileName}_large.jpg";
			DeleteFile(smallPath);
			DeleteFile(mediumPath);
			DeleteFile(largePath);
		}

		public static void DeleteTempImage(string path, string imageFile)
		{
			var fileEntries = Directory.GetFiles(path);

			foreach (var file in fileEntries)
			{
				if (file.Contains(imageFile))
				{
					DeleteFile(file);
				}
			}
		}
	}
}
