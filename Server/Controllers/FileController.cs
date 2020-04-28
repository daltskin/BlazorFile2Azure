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
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _container;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            List<string> results = new List<string>();

            InitContainer();
            string prefix = _container.Uri.ToString();

            await foreach (var blob in _container.GetBlobsAsync())
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
                InitContainer();
                string fileName = $"{Guid.NewGuid().ToString()}.jpg";
                var blob = _container.GetBlobClient(fileName);
                await blob.UploadAsync(Request.Body);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Error saving file");
            }

            return new OkObjectResult("ok");
        }

        #region helper
        private void InitContainer()
        {
            string blobConnectionString = _configuration["blobConnectionString"];
            string blobContainerName = _configuration["blobStorageContainer"];

            var container = new BlobContainerClient(blobConnectionString, blobContainerName);
            container.CreateIfNotExists();
        }
        #endregion
    }
}