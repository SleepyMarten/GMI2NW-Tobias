using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tobias.General.Util
{
    public static class FileUtil
    {
        /// <summary>
        /// Copies all files in <paramref name="originalFolderPath"/> matching <paramref name="filePattern"/> to <paramref name="destinationFolderPath"/>.
        /// </summary>
        /// <param name="originalFolderPath"></param>
        /// <param name="destinationFolderPath"></param>
        /// <param name="filePattern"></param>
        /// <returns>Number of files actually copied.</returns>
        public static int CopyFiles(string originalFolderPath, string destinationFolderPath, string filePattern)
        {
            if (!Directory.Exists(originalFolderPath))
            {
                throw new ArgumentException("Origin path does not exist. Cannot copy.", nameof(originalFolderPath));
            }

            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            var fileNamesInOriginalFolder = Directory.EnumerateFiles(originalFolderPath, filePattern).ToList();
            fileNamesInOriginalFolder.Sort();
            foreach (var fileNameWithPath in fileNamesInOriginalFolder)
            {
                var fileNameWithoutPath = Path.GetFileName(fileNameWithPath);
                var newFileName = Path.Combine(destinationFolderPath, fileNameWithoutPath);
                File.Copy(fileNameWithPath, newFileName, true);
            }
            return fileNamesInOriginalFolder.Count;
        }

        /// <summary>
        /// Deletes all files in folder <paramref name="folderPath"/> matching <paramref name="filePattern"/>.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="filePattern"></param>
        /// <returns>Number of files actually deleted.</returns>
        public static int DeleteFiles(string folderPath, string filePattern)
        {
            var fileNamesWithPath = Directory.EnumerateFiles(folderPath, filePattern).ToList();
            foreach (var fileNameWithPath in fileNamesWithPath)
            {
                File.Delete(fileNameWithPath);
            }
            return fileNamesWithPath.Count;
        }
    }


}
