using System;
using System.Collections.Generic;
using System.Text;

namespace Kinvitech.Services.Constants
{
    /// <summary>
    /// Constants for Logging
    /// </summary>
    public class LoggerConstants
    {
        /// <summary>
        /// Error message
        /// </summary>
        public const string CAN_NOT_BE_DETERMINED = "Can not be determined";
        public const string DOUBLE_BACKSLASH = "\\";

        /// <summary>
        /// File extension for error file
        /// </summary>
        public const string ERROR_FILE_TYPE = ".txt";

        #region Logging Constants
        public const string ACCOUNT = "Account";
        public const string BLR_ID = "BlrId";
        public const string BLR_ID_FORMAT = "BillerID: {0}";
        public const string DATE_FORMAT = "Date: {0}";
        public const string ERROR_DESCRIPTION_FORMAT = "ErrorDescription: {0}";
        public const string ERROR_FILE_NAME_FORMAT = "{0}_Error_{1}_{2}";
        public const string ERROR_FORMAT = "Error: ";
        public const string FILE_NAME_FORMAT = "FileName: {0}";
        public const string GENERAL_FILE_NAME_FORMAT = "General_Exception_{0}{1}";
        public const string HRESULT_FORMAT = "HResult: 0x{0}";
        public const string INNER_EXCEPTION_FORMAT = "InnerException: {0}";
        public const string MESSAGE_FORMAT = "Message: {0}";
        public const string NULL = "Null";
        public const string STACKTRACE_FORMAT = "StackTrace:{0}{1}";
        public const string X = "x";
        #endregion

        #region Logging Test Constants
        public const string APM_FILE_TYPE = ".apm";
        public const string BAD_FILE_NAME = "BadAPMFile.apm";
        public const string BAD_PATH = "TestingFiles//BadAPMFile.apm";
        public const string ERROR_TEST_FOLDER = "ErrorTestFolder";
        public const string FOURTY_TWO = "42";
        public const string GOOD_FILE_NAME = "GoodAPMFile.apm";
        public const string GOOD_PATH = "TestingFiles//GoodAPMFile.apm";
        public const string GUID = "GUID: ";
        public const string LABEL_BILLER_ID = "BillerID:";
        public const string LABEL_DATE = "Date:";
        public const string LABEL_ERROR = "Error:";
        public const string LABEL_ERROR_DESCRIPTION = "ErrorDescription:";
        public const string LABEL_FILE_NAME = "FileName:";
        public const string LABEL_HRESULT = "HResult:";
        public const string LABEL_INNER_EXCEPTION = "InnerException:";
        public const string LABEL_MESSAGE = "Message:";
        public const string LABEL_NAME_BILLER_ID = "Biller ID";
        public const string LABEL_NAME_DATE = "Date";
        public const string LABEL_NAME_ERROR_DESCRIPTION = "Error Description";
        public const string LABEL_NAME_FILE_ERROR = "File Error";
        public const string LABEL_NAME_FILE_NAME = "File Name";
        public const string LABEL_NAME_HRESULT = "HResult";
        public const string LABEL_NAME_INNER_EXCEPTION = "Inner Exception";
        public const string LABEL_NAME_MESSAGE = "Message";
        public const string LABEL_NAME_STACK_TRACE = "Stack Trace";
        public const string LABEL_NOT_CREATED_FORMAT = "The label {0} was not generated inside the log file.";
        public const string LABEL_STACK_TRACE = "StackTrace:";
        public const string LOG_FILE_NOT_GENERATED = "The log file was not generated";
        public const string TEST = "test";
        public const string TEST_BILLER_ID = "TestBillerID";
        #endregion
    }
}
