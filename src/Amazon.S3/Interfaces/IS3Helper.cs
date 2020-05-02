using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.S3.Interfaces
{
    /// <summary>
    /// Amazon S3 Helper Interface
    /// </summary>
    public interface IS3Helper
    {
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="keyName">Name of the key.</param>
        void UploadFile(string filePath, string keyName);

        /// <summary>
        /// Puts the object.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="bodyContent">Content of the body.</param>
        Task<int> PutObject(string keyName, string bodyContent);

        /// <summary>
        /// Retrieves the filestream from S3 and moves file to a specified path
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="dumpPath">The dump path.</param>
        /// <returns></returns>
        Stream RetrieveFile(string keyName, string dumpPath);
    }
}
