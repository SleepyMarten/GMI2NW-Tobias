using System.IO;
using System.Runtime.CompilerServices;

namespace Tobias.TestUtil.IO
{
    public static class FileSystemUtil
    {
        /// <summary>
        /// Generates a folder name 'CallerClassName/CallerMethodName' based on the caller's class and method names. 
        /// Then creates the folder, if not existing, and returns the 
        /// full path of the folder.
        /// </summary>
        /// <param name="deleteBeforeCreate">Deletes the folder first, if it exists.</param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        public static string CreateTestFolder(bool deleteBeforeCreate = true, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            var callerFileName = Path.GetFileNameWithoutExtension(callerFilePath);
            var folderPath = Path.GetFullPath(Path.Combine(callerFileName, callerMemberName));

            if (deleteBeforeCreate && Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
    }
}
