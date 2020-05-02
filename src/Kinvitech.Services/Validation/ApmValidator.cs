using Kinvitech.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Kinvitech.Services.Validation
{
    /// <summary>
    /// APM file Validator
    /// </summary>
    public static class ApmValidator
    {
        private static string TARGET_NAMESPACE = "http://www.kinvitech.com/BillHub";
        private static bool isValid = true;

        /// <summary>
        /// Method for validation of APM file
        /// </summary>
        /// <param name="apmFile">apm full path file including .apm file</param>
        /// <returns></returns>
        public static bool Validate(string apmFile)
        {
            if (!isValid)
            {
                isValid = true;
            }

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(TARGET_NAMESPACE, @"Schemas\BillHubTypes.xsd");
            schemas.Add(TARGET_NAMESPACE, @"Schemas\APM File Schema.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(schemas);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            LoggerHelper.Debug("Loading apm file...");

            try
            {

                using (XmlReader reader = XmlReader.Create(apmFile, settings))
                {
                    LoggerHelper.Debug("Validating...");
                    while (reader.Read())
                    {
                        // keep reading entire file
                    }
                }
            }
            catch
            {
                // this happens if we are unable to create the XmlReader
                // because the file hasn't finished writing yet
                // just ignore it and we'll try again later
                return false;
            }

            return isValid;
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            LoggerHelper.Debug(e.Message);
            isValid = false;
        }
    }
}
