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
        private BlobContainerClient _container;
        static bool containerCreated = false;

        public FileController(IConfiguration configuration)
        {
            string blobConnectionString = configuration["blobConnectionString"];
            string blobContainerName = configuration["blobStorageContainer"];

            if (!containerCreated)
            {
                _container = new BlobContainerClient(blobConnectionString, blobContainerName);
                _container.CreateIfNotExists();

                containerCreated = true;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            List<string> results = new List<string>();

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
    }
}