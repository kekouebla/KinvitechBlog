using Kinvitech.Services.Constants;
using Kinvitech.Services.Helpers;
using Kinvitech.Services.Interfaces;
using Steeltoe.Common.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Kinvitech.Services
{
    /// <summary>
    /// Service for Purging and deleting of files 
    /// </summary>
    public class FileMonitorArchiveService : IMonitoringService
    {
        private readonly string _volumeServiceShare;
        private readonly string _volumeServiceUsername;
        private readonly string _volumeServicePassword;
        private readonly string _volumeServiceDomain;
        private readonly string _incomingFolder;
        private readonly string _processingFolder;
        private readonly string _errorFolder;
        private readonly string _archiveFolder;
        private readonly int _deleteFilesOlderThanDays;

        /// <summary>
        /// File Monitor Archive Service constructor
        /// </summary>
        public FileMonitorArchiveService()
        {
            _volumeServiceShare = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC); ;
            _volumeServiceUsername = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_USERNAME);
            _volumeServicePassword = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_PASSWORD);
            _volumeServiceDomain = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_DOMAIN);

            _incomingFolder = Environment.GetEnvironmentVariable(EnvVar.INCOMING_FOLDER);
            _processingFolder = Environment.GetEnvironmentVariable(EnvVar.PROCESSING_FOLDER);
            _errorFolder = Environment.GetEnvironmentVariable(EnvVar.ERROR_FOLDER);
            _archiveFolder = Environment.GetEnvironmentVariable(EnvVar.ARCHIVE_FOLDER);
            _deleteFilesOlderThanDays = EnvVar.DEFAULT_DELETE_FILES_AGE_DAYS;
        }

        /// <summary>
        /// Method for File Purging
        /// </summary>
        public void Monitor()
        {
            LoggerHelper.Debug("Delete Old Files Timer is firing");

            if (Environment.GetEnvironmentVariable(EnvVar.ASPNET_CORE_ENVRIONMENT) == "Development")
            {
                LoggerHelper.Info("Using local service UNC");

                // Attempt to create all directories needed
                UncFolderHelper.CreateUncFolders();

                try
                {
                    DeleteFileLoop();
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("FileManager_DeleteLoop", ex);
                }
            }
            else
            {
                try
                {
                    NetworkCredential credential = new NetworkCredential(_volumeServiceUsername, _volumeServicePassword, _volumeServiceDomain);
                    using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(_volumeServiceShare, credential))
                    {
                        try
                        {
                            DeleteFileLoop();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.GeneralExceptionLogFile("FileManager_DeleteLoop", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("FileManager_Delete_NetworkCredentials", ex);
                }
            }

            LoggerHelper.Debug("Delete Old Files Timer has finished");
        }

        private void DeleteFileLoop()
        {
            var purgeDate = DateTime.UtcNow.AddDays(-1 * _deleteFilesOlderThanDays);

            // remove old files in /Incoming
            foreach (var file in Directory.GetFiles(_volumeServiceShare + _incomingFolder)
                .Select(x => new FileInfo(x))
                .Where(x => x.LastAccessTimeUtc <= purgeDate && x.CreationTimeUtc <= purgeDate))
            {

                PurgeFile(file);

            }

            // remove old files in /Processing
            foreach (var file in Directory.GetFiles(_volumeServiceShare + _processingFolder)
                .Select(x => new FileInfo(x))
                .Where(x => x.LastAccessTimeUtc <= purgeDate && x.CreationTimeUtc <= purgeDate))
            {

                PurgeFile(file);

            }

            // remove old files in /Archive
            foreach (var file in Directory.GetFiles(_volumeServiceShare + _archiveFolder)
                .Select(x => new FileInfo(x))
                .Where(x => x.LastAccessTimeUtc <= purgeDate && x.CreationTimeUtc <= purgeDate))
            {
                PurgeFile(file);
            }
        }
        private void PurgeFile(FileInfo file)
        {
            LoggerHelper.Debug($"Deleting old file {file.FullName}");
            try
            {
                file.Delete();
            }
            catch
            {
                LoggerHelper.Error($"Could not delete old file {file.FullName}");
            }
        }
    }
}
