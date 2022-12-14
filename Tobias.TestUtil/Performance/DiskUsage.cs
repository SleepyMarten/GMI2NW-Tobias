using System;
using System.IO;
using System.Linq;

namespace Tobias.TestUtil.Performance
{
    public static class DiskUsage
    {
        /// <summary>
        /// Returns the sum of all files in a directory, including subdirectories.
        /// </summary>
        /// <remarks>If <paramref name="path"/> is omitted, null, or empty, the current directory is used.</remarks>
        /// <param name="path">Directory path.</param>
        /// <returns>The size, in bytes</returns>
        public static ulong GetDirectorySize(string path = null)
        {
            if(String.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory();
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            return (ulong) dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
        }

        /// <summary>
        /// Returns the sum of all files in a directory, including subdirectories.
        /// </summary>
        /// <remarks>If <paramref name="path"/> is omitted, null, or empty, the current directory is used.</remarks>
        /// <param name="path">Directory path.</param>
        /// <returns>The size, in kilobytes</returns>
        public static ulong GetDirectorySizeKB(string path = null)
        {
            return GetDirectorySize(path) / (2 ^ 12);
        }

        /// <summary>
        /// Returns the sum of all files in a directory, including subdirectories.
        /// </summary>
        /// <remarks>If <paramref name="path"/> is omitted, null, or empty, the current directory is used.</remarks>
        /// <param name="path">Directory path.</param>
        /// <returns>The size, in megabytes</returns>
        public static ulong GetDirectorySizeMB(string path = null)
        {
            return GetDirectorySizeKB(path) / (2 ^ 11);
        }

        /// <summary>
        /// Measures the disk space used by all files in the directory specified
        /// by <paramref name="directory"/>, recursively, before and after exeucting
        /// <paramref name="action"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="directory">The directory to measure.</param>
        /// <returns>The difference in disk usage before and after executing <paramref name="action"/>.</returns>
        /// <remarks>A positive return value means that more disk is used after executing 
        /// <paramref name="action"/>; a negative value that less disk space is used.</remarks>
        public static ulong MeasureDiskUsageKB(Action action, string directory = null)
        {
            if(String.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            var diskUsageBefore = GetDirectorySizeKB(directory);
            
            action.Invoke();
            
            var diskUsageAfter = GetDirectorySizeKB(directory);
            return diskUsageAfter - diskUsageBefore;
        }
    }
}
