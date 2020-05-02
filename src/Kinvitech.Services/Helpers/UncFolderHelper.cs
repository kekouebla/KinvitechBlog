using Kinvitech.Services.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Helpers
{
    /// <summary>
    /// Helper for creating folder structure
    /// </summary>
    public static class UncFolderHelper
    {
        // Using static bool because we only need to try and create the folders once
        private static bool hasCreatedFolders;

        // Array of all the folder names inside the UNC
        private static Array FolderNames = new string[] {
                EnvVar.ARCHIVE_FOLDER,
                EnvVar.ERROR_FOLDER,
                EnvVar.PROCESSING_FOLDER,
                EnvVar.INCOMING_FOLDER,
                EnvVar.RESPONSE_FOLDER
            };

        public static void CreateUncFolders()
        {
            // If we've already created the folders, then no need to continue
            if (hasCreatedFolders)
            {
                return;
            }

            // Get the path to the UNC from the environment variables
            var volumeServiceShare = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC);

            // Loop through each folder
            foreach (string folderName in FolderNames)
            {
                try
                {
                    // Get the foldername from the envrionment variables. 
                    var folder = Environment.GetEnvironmentVariable(folderName);

                    if (string.IsNullOrEmpty(folder))
                    {
                        // If we aren't able to find the folder, lets throw a warn, but continue on.
                        LoggerHelper.Warn($"Unable to find environment variable for {folderName}");
                    }
                    else
                    {
                        // CreateDirectory will either create the directory, or do nothing if it already exists.
                        System.IO.Directory.CreateDirectory(volumeServiceShare + folder);
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("UncFolderHelper CreateFolders method", ex);
                    return;
                }
            }

            // Set the flag to true so we don't needlessly try to create the folders again.
            hasCreatedFolders = true;
        }
    }
}
