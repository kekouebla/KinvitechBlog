using Kinvitech.Services.Constants;
using Kinvitech.Services.Helpers;
using Kinvitech.Services.Interfaces;
using Renci.SshNet;
using Steeltoe.Common.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Kinvitech.Services
{
    /// <summary>
    /// Service that connect to SFG and download files
    /// </summary>
    public class FileMonitorSfgService : IMonitoringService
    {
        private readonly string _volumeServiceShare;
        private readonly string _volumeServiceUsername;
        private readonly string _volumeServicePassword;
        private readonly string _volumeServiceDomain;
        private readonly string _incomingFolder;
        private readonly string _sfgHost;
        private readonly string _sfgUsername;
        private readonly string _sfgPassword;
        private readonly string _sfgPath;
        private readonly int _sfgPort;
        private readonly bool _sfgDownloadOn;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileMonitorSfgService()
        {
            _volumeServiceShare = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC); ;
            _volumeServiceUsername = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_USERNAME);
            _volumeServicePassword = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_PASSWORD);
            _volumeServiceDomain = Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_DOMAIN);
            _incomingFolder = Environment.GetEnvironmentVariable(EnvVar.INCOMING_FOLDER);
            _sfgHost = Environment.GetEnvironmentVariable(EnvVar.SFG_HOST);

            int.TryParse(Environment.GetEnvironmentVariable(EnvVar.SFG_PORT), out _sfgPort);

            _sfgUsername = Environment.GetEnvironmentVariable(EnvVar.SFG_USERNAME);
            _sfgPassword = Environment.GetEnvironmentVariable(EnvVar.SFG_PASSWORD);
            _sfgPath = Environment.GetEnvironmentVariable(EnvVar.SFG_PATH);
            _sfgDownloadOn = Convert.ToBoolean(Environment.GetEnvironmentVariable(EnvVar.SFG_DOWNLOAD_ON));
        }

        /// <summary>
        /// Main method
        /// </summary>
        public void Monitor()
        {
            LoggerHelper.Debug("Download APM in SFG Timer is firing");

            if (Environment.GetEnvironmentVariable(EnvVar.ASPNET_CORE_ENVRIONMENT) == "Development")
            {
                LoggerHelper.Info("Using local service UNC");

                // Attempt to create all directories needed
                UncFolderHelper.CreateUncFolders();

                try
                {
                    DownloadApmFile();
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("FileManager_DownloadApmInSfgLoop", ex);
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
                            DownloadApmFile();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.GeneralExceptionLogFile("FileManager_DownloadApmInSfgLoop", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.GeneralExceptionLogFile("FileManager_FileManager_DownloadApmInSfg_NetworkCredentials", ex);
                }
            }

            LoggerHelper.Debug("FileManager_Download Apm In Sfg Timer has finished");
        }

        /// <summary>
        /// Download APM Files from SFG to Incoming Folder
        /// </summary>
        private void DownloadApmFile()
        {
            if (_sfgDownloadOn)
            {
                var connectionInfo = new ConnectionInfo(_sfgHost, _sfgPort,
                        _sfgUsername, new PasswordAuthenticationMethod(_sfgUsername, _sfgPassword));

                using (var client = new SftpClient(connectionInfo))
                {
                    client.Connect();
                    var files = client.ListDirectory(_sfgPath);

                    foreach (var file in files)
                    {
                        if (file.IsDirectory)
                        {
                            continue;
                        }

                        try
                        {
                            LoggerHelper.Debug($"Downloading {file.Name} from SFG.");
                            var newFilename = $"{Guid.NewGuid().ToString("N")}.downloading";
                            var fs = File.Create($"{_volumeServiceShare}{_incomingFolder}\\{newFilename}");
                            client.DownloadFile(file.FullName, fs);
                            fs.Close();

                            LoggerHelper.Debug($"Download complete for {file.Name} from SFG, changed the name to {newFilename}");

                            // Get list of downloaded files
                            // and rename them to .apm
                            RenameDownloadedFiles();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.GeneralExceptionLogFile("FileManager_DownloadApmInSfgLoop", ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Rename files from .downloading to .apm
        /// </summary>
        private void RenameDownloadedFiles()
        {
            // Get all .downloading files from incoming folder
            var files = Directory.GetFiles(_volumeServiceShare + _incomingFolder, "*.downloading");

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                LoggerHelper.Debug($"Changing file extension of {fileInfo.FullName} to .apm");

                fileInfo.MoveTo(fileInfo.FullName.Replace(".downloading", ".apm"));
            }
        }
    }
}
