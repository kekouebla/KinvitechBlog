using Kinvitech.Services.Constants;
using Kinvitech.Services.Helpers;
using Kinvitech.Services.Interfaces;
using Kinvitech.Services.Validation;
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
    /// Service for monitoring of files in incoming folder
    /// </summary>
    public class FileMonitorCheckInboundFolderService : IMonitoringService
    {
        private readonly string _volumeServiceShare;
        private readonly string _volumeServiceUsername;
        private readonly string _volumeServicePassword;
        private readonly string _volumeServiceDomain;
        private readonly string _incomingFolder;
        private readonly string _processingFolder;
        private readonly string _errorFolder;
        private readonly string _archiveFolder;

        /// <summary>
        /// File monitor check inbound folder service constructor
        /// </summary>
        public FileMonitorCheckInboundFolderService()
        {

            _volumeServiceShare = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC);
            _volumeServiceUsername = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_USERNAME);
            _volumeServicePassword = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_PASSWORD);
            _volumeServiceDomain = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_DOMAIN);

            _incomingFolder = Environment.GetEnvironmentVariable(EnvVar.INCOMING_FOLDER);
            _processingFolder = Environment.GetEnvironmentVariable(EnvVar.PROCESSING_FOLDER);
            _errorFolder = Environment.GetEnvironmentVariable(EnvVar.ERROR_FOLDER);
            _archiveFolder = Environment.GetEnvironmentVariable(EnvVar.ARCHIVE_FOLDER);
        }

        /// <summary>
        /// Method for File monitoring
        /// </summary>
        public void Monitor()
        {
            LoggerHelper.Debug("Timer is firing");

            //Using X service UNC
            if (Environment.GetEnvironmentVariable(EnvVar.ASPNET_CORE_ENVRIONMENT) == "Development")
            {
                LoggerHelper.Info("Using local service UNC");
                // Attempt to create all directories needed
                UncFolderHelper.CreateUncFolders();

                try
                {
                    FileLoop();
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("FileManager_FileLoop", ex);
                }
            }
            else
            {
                LoggerHelper.Info("Using network service UNC");
                try
                {
                    NetworkCredential credential = new NetworkCredential(_volumeServiceUsername, _volumeServicePassword, _volumeServiceDomain);
                    using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(_volumeServiceShare, credential))
                    {
                        try
                        {
                            FileLoop();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.GeneralExceptionLogFile("FileManager_FileLoop", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("FileManager_FileLoop_NetworkCredentials", ex);
                }
            }

            LoggerHelper.Debug("Timer has finished");
        }

        private void FileLoop()
        {
            var files = Directory.GetFiles(_volumeServiceShare + _incomingFolder);
            if (!files.Any())
            {
                return;
            }

            foreach (var file in files)
            {
                LoggerHelper.Debug($"Found file {file}");
                //validate APM file
                bool isValid = ApmValidator.Validate(file);
                var fileInfo = new FileInfo(file);

                if (isValid)
                {
                    LoggerHelper.Debug($"Valid APM file: {file}");

                    try
                    {
                        // move valid file to Processing Folder
                        fileInfo.MoveTo(fileInfo.FullName.Replace(_incomingFolder, _processingFolder));
                        LoggerHelper.Debug($"Moved file {file} to processing folder");
                    }
                    catch (IOException ex)
                    {
                        if (ex.Message.Equals("The process cannot access the file because it is being used by another process."))
                        {
                            LoggerHelper.Debug($"The process cannot access the file {file} because it is being used by another process.");
                        }
                        else
                        {
                            LoggerHelper.LogFile(fileInfo.FullName, "FileManager_MoveFileToProcessingFolder", ex);
                        }
                    }
                }
                else
                {

                    LoggerHelper.LogFile(file, $"Invalid APM file: {file}");
                    try
                    {
                        // move invalid file to Error Folder
                        fileInfo.MoveTo(fileInfo.FullName.Replace(_incomingFolder, _errorFolder));
                    }
                    catch (IOException ex)
                    {
                        if (ex.Message.Equals("The process cannot access the file because it is being used by another process."))
                        {
                            LoggerHelper.Debug($"The process cannot access the file {file} because it is being used by another process.");
                        }
                        else
                        {
                            LoggerHelper.LogFile(fileInfo.FullName, "FileManager_MoveFileToErrorFolder", ex);
                        }
                    }
                }
            }
        }
    }
}
