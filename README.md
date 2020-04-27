# BlazorFile2Azure

### Blazor WebAssembly sample to upload a file to Azure Blob Storage via an API

This application uses [Steve Sanderson's](https://twitter.com/stevensanderson) [BlazorInputFile](https://github.com/SteveSandersonMS/BlazorInputFile) to upload files and save them to blob storage.

#### BlazorFile2Azure.Client

This Blazor WASM application that utilises the BlazorInputFile, checking that file(s) have been selected and then calls posts them to the server-side API.

#### BlazorFile2Azure.Server

The API controller that hosts the Blazor application and server-side API.

### Instructions

Add your blob storage account details within the appSettings.json file:

| Key | Description |
| ----- | ----- |
| blobConnectionString | Azure Blob storage Connection String |
| blobStorageContainer | The container name within your storage account |

### How it works

Blazor client simply iterates the selected files within FileListEntry array and posts each stream to the server side (API controller).  Which in turn uses the Azure Blob Storage SDK to upload the stream to a specified blob storage container.

Blazor components support [attribute splatting and arbitary parameters](https://docs.microsoft.com/en-us/aspnet/core/blazor/components?view=aspnetcore-3.0#attribute-splatting-and-arbitrary-parameters) - Steve's BlazorInputFile control uses this:

```
<input type="file" @ref="inputFileElement" @attributes="UnmatchedParameters" />

@code {
    [Parameter(CaptureUnmatchedValues = false)] public Dictionary<string, object> UnmatchedParameters { get; set; }
```

Meaning, that we can add the additional file input attributes such as capture mode and file type support :)
```
 <BlazorInputFile.InputFile id="inputControl" OnChange="HandleChangeSelected" capture="camera" accept="image/*" />
```


### Known issue

* Clearing value of input element: https://github.com/SteveSandersonMS/BlazorInputFile/issues/2
