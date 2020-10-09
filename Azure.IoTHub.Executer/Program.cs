using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client.Transport;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.IoTHub.Executer
{
  class Program
  {
    private static readonly string DeviceConnectionString = "";

    static async Task Main(string[] args)
    {
      await UploadFileAsync();

      //await SyncFileAsync();
    }

    private static async Task SyncFileAsync()
    {
      // connect to iothub
      var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

      while (true)
      {
        var msgModel = new
        {
          msg = "test connection"
        };
        var messageString = JsonConvert.SerializeObject(msgModel);
        var message = new Message(Encoding.ASCII.GetBytes(messageString));

        await deviceClient.SendEventAsync(message);
        Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

        await Task.Delay(1000);
      }
    }

    private static async Task UploadFileAsync()
    {
      // use blob sdk to upload with blob uri
      string fileName = "cytoflex123.txt";
      string filePath = "C://Cass//cyto_flex_log.txt";

      // connect to IoTHub
      var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

      // get linked blob sas uri
      var blob = await deviceClient.GetFileUploadSasUriAsync(new FileUploadSasUriRequest { BlobName = fileName });
      var blobUri = $"https://{blob.HostName}/{blob.ContainerName}/{blob.BlobName}{blob.SasToken}";

      // call blob sdk to upload files
      var destinationBlob = new CloudBlockBlob(new Uri(blobUri));
      await destinationBlob.UploadFromFileAsync(filePath);


      //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=casstestlake;AccountKey=BFhOE3ZqkeM55Cbpl2d9MsYQWM3jV4k0i8XoZPgL5M62APPUWy7s1IbGSV7WIgQqkjd4vj8YqJdtBbDMKZanbQ==;EndpointSuffix=core.windows.net");
      //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      //CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

      //if (await cloudBlobContainer.CreateIfNotExistsAsync())
      //{
      //  await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
      //}

      //CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
      //string blobUri = cloudBlockBlob.Uri.OriginalString;

      //var cbBlob = new CloudBlockBlob(cloudBlockBlob.Uri);
      //await cloudBlockBlob.UploadFromFileAsync(filePath);
    }
  }
}
