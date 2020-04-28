using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
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

            var container = GetContainer();
            string prefix = container.Uri.ToString();

            await foreach (var blob in container.GetBlobsAsync())
            {
                results.Add($"{prefix}/{blob.Name}");
            }

            return results;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            try
            {
                var container = GetContainer();
                string fileName = $"{Guid.NewGuid().ToString()}.jpg";
                var blob = container.GetBlobClient(fileName);
                await blob.UploadAsync(Request.Body);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Error saving file");
            }

            return new OkObjectResult("ok");
        }

        #region helper
        private BlobContainerClient GetContainer()
        {
            string blobConnectionString = _configuration["blobConnectionString"];
            string blobContainerName = _configuration["blobStorageContainer"];

            var container = new BlobContainerClient(blobConnectionString, blobContainerName);
            container.CreateIfNotExists();

            return container;
        }
        #endregion
    }
}