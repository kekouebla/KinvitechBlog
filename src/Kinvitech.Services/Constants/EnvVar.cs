using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Constants
{
    public class EnvVar
    {
        public static readonly string INCOMING_FOLDER_CHECK_SECONDS = "incomingFolderCheckSeconds";
        public static readonly string VOLUME_SERVICE_UNC = "VolumeServiceUNC";
        public static readonly string VOLUME_SERVICE_USERNAME = "VolumeServiceUserName";
        public static readonly string VOLUME_SERVICE_PASSWORD = "VolumeServiceUserPassword";
        public static readonly string VOLUME_SERVICE_DOMAIN = "VolumeServiceUserDomain";
        public static readonly string INCOMING_FOLDER = "IncomingFolder";
        public static readonly string PROCESSING_FOLDER = "ProcessingFolder";
        public static readonly string ARCHIVE_FOLDER = "ArchiveFolder";
        public static readonly string ERROR_FOLDER = "ErrorFolder";
        public static readonly string ASPNET_CORE_ENVRIONMENT = "ASPNet_Core_Environment";
        public static readonly string RESPONSE_FOLDER = "ResponseFolder";
        public static readonly int DEFAULT_DELETE_FILES_AGE_DAYS = 730;
        public static readonly string ARCHIVE_FOLDER_CHECK_HOURS = "archiveFolderCheckHours";
        public static readonly string SFG_HOST = "SfgHost";
        public static readonly string SFG_PORT = "SfgPort";
        public static readonly string SFG_USERNAME = "SfgUsername";
        public static readonly string SFG_PASSWORD = "SfgPassword";
        public static readonly string SFG_PATH = "SfgPath";
        public static readonly string SFG_DOWNLOAD_ON = "SfgDownloadOn";
        public static readonly string SFG_CHECK_IN_SECONDS = "SfgCheckInSeconds";
    }
}
