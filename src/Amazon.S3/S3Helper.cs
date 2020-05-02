using Amazon;
using Amazon.S3;
using Amazon.S3.Interfaces;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Amazon.S3
{
    /// <summary>
    /// Amazon S3 Helper Implementation
    /// </summary>
    public class S3Helper : IS3Helper
    {
        public RegionEndpoint RegionEndpoint { get { return _regionEndpoint; } }

        protected string _bucketName;
        protected IAmazonS3 _client;
        protected RegionEndpoint _regionEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="S3Helper"/> class.
        /// </summary>
        /// <param name="accessKey">The access key.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="bucketName">Name of the bucket.</param>
        public S3Helper(string accessKey,
            string secretKey,
            string regionName,
            string bucketName)
        {
            _bucketName = bucketName;
            _regionEndpoint = RegionEndpoint.GetBySystemName(regionName);
            _client = new AmazonS3Client(accessKey, secretKey, _regionEndpoint);
        }

        /// <summary>
        /// Retrieves the filestream from S3
        /// </summary>
        /// <param name="keyName">Name of the key or file.</param>
        /// <param name="dumpPath">The dump path.</param>
        /// <returns></returns>
        public virtual Stream RetrieveFile(string keyName, string dumpPath)
        {
            using (_client)
            {
                var request = new GetObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = keyName
                };

                // download the file and move to a specified dump folder
                GetObjectResponse response = _client.GetObjectAsync(request).Result;

                return response.ResponseStream;

            }
        }

        /// <summary>
        /// Uploads the file to S3 bucket
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="keyName">Name of the key or file.</param>
        public virtual void UploadFile(string filePath, string keyName)
        {
            var fileTransferUtil = new TransferUtility(_client);
            fileTransferUtil.Upload(filePath, _bucketName, keyName);
        }

        /// <summary>
        /// Puts the object.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="bodyContent">Content of the body.</param>
        public virtual async Task<int> PutObject(string keyName, string bodyContent)
        {
            using (_client)
            {
                var request = new PutObjectRequest()
                {
                    BucketName = _bucketName,
                    ContentBody = bodyContent,
                    Key = keyName
                };

                var response = await _client.PutObjectAsync(request);

                return (int)response.HttpStatusCode;
            }
        }
    }
}
