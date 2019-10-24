# BlazorFile2Azure

### Blazor WebAssembly sample to upload a file to Azure Blob Storage via an API

This application uses [Steve Sanderson's](https://twitter.com/stevensanderson) [BlazorInputFile](https://github.com/SteveSandersonMS/BlazorInputFile) to upload a file and save it to blob storage

#### BlazorFile2Azure.Client

The Blazor WASM application that utilises the BlazorInputFile, checking that a file has been added and then calls the server-side API

#### BlazorFile2Azure.Server

The API controller that hosts the Blazor application and server-side API

### Instructions

Add your blob storage account details within the [appSettings.json](./BlazorFile2Azure.Server/appSettings.json)

| Key | Description |
| ----- | ----- |
| blobStorageAccountKey | Blob storage account key secret |
| blobStorageAccountName | The name of your storage account |
| blobStorageContainer | The container name within your storage account |

### Known issue

* Clearing value of input element: https://github.com/SteveSandersonMS/BlazorInputFile/issues/2
