using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFile2Azure.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            List<string> results = new List<string>();

            StorageCredentials storageCredentials = new StorageCredentials(_configuration["blobStorageAccountName"], _configuration["blobStorageAccountKey"]);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(_configuration["blobStorageContainer"]);

            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            do
            {
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.None, 999, continuationToken, null, null);
                foreach (CloudBlob blobItem in resultSegment.Results)
                {
                    results.Add(blobItem.StorageUri.PrimaryUri.ToString());
                }
                continuationToken = resultSegment.ContinuationToken;
            } while (continuationToken != null);

            return results;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            try
            {
                StorageCredentials storageCredentials = new StorageCredentials(_configuration["blobStorageAccountName"], _configuration["blobStorageAccountKey"]);
                CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(_configuration["blobStorageConntainer"]);

                string fileName = $"{Guid.NewGuid().ToString()}.jpg";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.UploadFromStreamAsync(Request.Body);
            }
            catch (Exception exp)
            {
                return new BadRequestObjectResult("Error saving file");
            }

            return new OkObjectResult("ok");
        }
    }
}