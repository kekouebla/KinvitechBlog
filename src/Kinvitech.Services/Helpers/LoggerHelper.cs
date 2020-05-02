using Kinvitech.Services.Constants;
using Kinvitech.Services.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Kinvitech.Services.Helpers
{
    public static class LoggerHelper
    {
        private static Serilog.Core.Logger log;
        public static string ErrorFolder { get; set; }

        static LoggerHelper()
        {
            log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            ErrorFolder = string.Concat(Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC), Environment.GetEnvironmentVariable(EnvVar.ERROR_FOLDER));
        }

        /// <summary>
        /// Used to log info for a system 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Info(string messageTemplate)
        {
            log.Information(messageTemplate);
        }

        /// <summary>
        /// Used to log info for a system with object. 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Info(string messageTemplate, params object[] propertyValues)
        {
            log.Information(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Used to log debug information for a system
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Debug(string messageTemplate)
        {
            log.Debug(messageTemplate);
        }

        /// <summary>
        /// Used to log debug information for a system with object. 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Debug(string messageTemplate, params object[] propertyValues)
        {
            log.Debug(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Used to log warning information for a system
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Warn(string messageTemplate)
        {
            log.Warning(messageTemplate);
        }

        /// <summary>
        /// Used to log warning information for a system with object
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Warn(string messageTemplate, params object[] propertyValues)
        {
            log.Warning(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Used to log error information with a system
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Error(string messageTemplate)
        {
            log.Error(messageTemplate);
        }


        /// <summary>
        /// Used to log error information with a system with object
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Error(string messageTemplate, params object[] propertyValues)
        {
            log.Error(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Used to log error information with a system with exception
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="ex"></param>
        public static void Error(string messageTemplate, Exception ex)
        {
            log.Error(messageTemplate, ex);
        }

        /// <summary>
        /// Used to log error information with a system with exception, and object. 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        public static void Error(string messageTemplate, Exception ex, params object[] propertyValues)
        {
            log.Error(messageTemplate, ex, propertyValues);
        }

        /// <summary>
        /// Generates an Exception file for any exception related to moving or processing files in Bill Discovery
        /// Also writes information to Splunk, including the files Error Metadata
        /// </summary>
        /// <param name="filePath">The path to the File with Metadata to log</param>
        /// <param name="errorText">Additional information about where the exception occured. (i.e. Error during application start)</param>
        public static void LogFile(string filePath, string errorText)
        {
            LoggerHelper.LogFile(filePath, errorText, null);
        }

        /// <summary>
        /// Generates an  Exception file for any exception related to moving or processing files in Bill Discovery
        /// Also writes information to Splunk, including the files Error Metadata
        /// </summary>
        /// <param name="exception">The Exception to log</param>
        /// <param name="filePath">The path to the File with Metadata to log</param>
        /// <param name="errorText">Additional information about where the exception occured. (i.e. Error during application start)</param>
        public static void LogFile(string filePath, string errorText, Exception exception)
        {
            LoggerHelper.LogFile(filePath, errorText, exception, true);
        }
        /// <summary>
        /// Generates an  Exception file for any exception related to moving or processing files in Bill Discovery
        /// May or may not write information to Splunk, including the files Error Metadata. 
        /// Mainly this method should be used for testing
        /// </summary>
        /// <param name="exception">The Exception to log</param>
        /// <param name="filePath">The path to the File with Metadata to log</param>
        /// <param name="errorText">Additional information about where the exception occured. (i.e. Error during application start)</param>
        /// <param name="writeToSplunk">Disable or enable writing the error text to splunk, defaults to true</param>
        public static void LogFile(string filePath, string errorText, Exception exception, bool writeToSplunk)
        {
            FileErrorMetadata fileErrorMetadata;

            var fileName = Path.GetFileName(filePath);

            fileErrorMetadata = GetFileErrorMetadata(filePath, exception);

            if (writeToSplunk)
            {
                LoggerHelper.Error(errorText, fileErrorMetadata);
            }

            string errorFileName = string.Format(LoggerConstants.ERROR_FILE_NAME_FORMAT, fileName, GetCurrentTimestamp(), LoggerConstants.ERROR_FILE_TYPE);
            var errorFolderPath = string.Concat(Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC), Environment.GetEnvironmentVariable(EnvVar.ERROR_FOLDER));
            var errorFilePath = string.Concat(errorFolderPath, LoggerConstants.DOUBLE_BACKSLASH, errorFileName);

            if (!File.Exists(errorFilePath))
            {
                using (StreamWriter sw = File.CreateText(errorFilePath))
                {
                    AddHeaderSectionToErrorFile(sw, errorText);
                    AddFileInformationtoErrorFile(sw, fileErrorMetadata);
                    if (fileErrorMetadata == null)
                    {
                        AddExceptionSectionToErrorFile(sw, exception);
                    }
                }
            }
        }

        /// <summary>
        /// Generates a generic error file for any exception
        /// Also writes information to Splunk
        /// </summary>
        /// <param name="exception">The Exception to log</param>
        /// <param name="errorText">Additional information about where the exception occured. (i.e. Error during application start)</param>
        public static void GeneralExceptionLogFile(string errorText, Exception exception)
        {
            GeneralExceptionLogFile(errorText, exception, true);
        }

        /// <summary>
        /// Generates a generic error file for any exception
        /// May or may not write information to Splunk.
        /// Mainly this methood should be used for testing
        /// </summary>
        /// <param name="exception">The Exception to log</param>
        /// <param name="errorText">Additional information about where the exception occured. (i.e. Error during application start)</param>
        /// <param name="writeToSplunk">Disable or enable writing the error text to splunk, defaults to true</param>
        public static void GeneralExceptionLogFile(string errorText, Exception exception, bool writeToSplunk)
        {
            if (writeToSplunk)
            {
                LoggerHelper.Error(errorText, exception);
            }

            string errorFileName = string.Format(LoggerConstants.GENERAL_FILE_NAME_FORMAT, GetCurrentTimestamp(), LoggerConstants.ERROR_FILE_TYPE);
            var errorFolderPath = string.Concat(Environment.GetEnvironmentVariable(EnvVar.VOLUME_SERVICE_UNC), Environment.GetEnvironmentVariable(EnvVar.ERROR_FOLDER));
            var errorFilePath = string.Concat(errorFolderPath, LoggerConstants.DOUBLE_BACKSLASH, errorFileName);

            if (!File.Exists(errorFilePath))
            {
                using (StreamWriter sw = File.CreateText(errorFilePath))
                {
                    AddHeaderSectionToErrorFile(sw, errorText);
                    AddExceptionSectionToErrorFile(sw, exception);
                }
            }
        }

        /// <summary>
        /// Used to get APM file metadata
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileErrorMetadata GetFileErrorMetadata(string filePath)
        {
            return LoggerHelper.GetFileErrorMetadata(filePath, null);
        }

        /// <summary>
        /// Used to get APM file metadata, with exception information
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static FileErrorMetadata GetFileErrorMetadata(string filePath, Exception exception)
        {
            var fileName = Path.GetFileName(filePath);

            string billerID = string.Empty;
            try
            {
                billerID = XDocument.Load(filePath).Root.Descendants()
                    .FirstOrDefault(x => x.Name.LocalName == LoggerConstants.ACCOUNT)
                    .Elements().FirstOrDefault(x => x.Name.LocalName == LoggerConstants.BLR_ID).Value;

            }
            catch (Exception)
            {
                return new FileErrorMetadata
                {
                    FileName = fileName,
                    BillerID = LoggerConstants.CAN_NOT_BE_DETERMINED,
                    Error = exception
                };
            }
            return new FileErrorMetadata
            {
                FileName = fileName,
                BillerID = billerID,
                Error = exception
            };

        }

        private static int GetCurrentTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private static void AddHeaderSectionToErrorFile(StreamWriter writer, string errorText)
        {
            writer.WriteLine(string.Format(LoggerConstants.DATE_FORMAT, DateTime.Now.ToString()));
            writer.WriteLine(string.Concat(string.Format(LoggerConstants.ERROR_DESCRIPTION_FORMAT, errorText), Environment.NewLine));
        }

        private static void AddExceptionSectionToErrorFile(StreamWriter writer, Exception ex)
        {

            writer.WriteLine(string.Format(LoggerConstants.HRESULT_FORMAT, ex.HResult.ToString(LoggerConstants.X)));
            writer.WriteLine(string.Format(LoggerConstants.MESSAGE_FORMAT, ex.Message));
            writer.WriteLine(string.Format(LoggerConstants.INNER_EXCEPTION_FORMAT, ex.InnerException == null ? LoggerConstants.NULL : ex.InnerException.Message));
            writer.WriteLine(string.Format(LoggerConstants.STACKTRACE_FORMAT, Environment.NewLine, ex.StackTrace));

        }

        private static void AddFileInformationtoErrorFile(StreamWriter writer, FileErrorMetadata fileErrorMetadata = null)
        {
            if (fileErrorMetadata != null)
            {
                writer.WriteLine(string.Format(LoggerConstants.FILE_NAME_FORMAT, fileErrorMetadata.FileName));
                writer.WriteLine(string.Format(LoggerConstants.BLR_ID_FORMAT, fileErrorMetadata.BillerID));
                if (fileErrorMetadata.Error != null)
                {
                    writer.WriteLine(LoggerConstants.ERROR_FORMAT);
                    AddExceptionSectionToErrorFile(writer, fileErrorMetadata.Error);
                }
            }
        }
    }
}
