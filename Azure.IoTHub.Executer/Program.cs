
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IoTHub.Executer
{
  public class Program
  {
    private const string DeviceConnectionString = "";
    private const string IoTHubConnectionString = "";

    static async Task Main(string[] args)
    {
      //await UpdateDesiredProperties("dps-device-112");

      //await ReportConnectivity();

      //await AddTagsAndQuery();

      //await SyncToIoTHubAsync();
    }

    private static async Task SyncToIoTHubAsync()
    {
      // connect to IoTHub
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

    public static async Task AddTagsAndQuery()
    {
      var registryManager = Microsoft.Azure.Devices.RegistryManager.CreateFromConnectionString(IoTHubConnectionString);

      var twin = await registryManager.GetTwinAsync("dps-device-112");
      var patch =
        @"{
            tags: {
                location: {
                    region: 'CN'
                }
            }
        }";
      await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag);
    }

    public static async Task ReportConnectivity()
    {
      try
      {
        Console.WriteLine("Sending connectivity data as reported property");
        var client = DeviceClient.CreateFromConnectionString(DeviceConnectionString,
             TransportType.Mqtt);

        var reportedProperties = new TwinCollection();
        var connectivity = new TwinCollection { ["check"] = "yes" };
        reportedProperties["renewkeys"] = connectivity;
        await client.UpdateReportedPropertiesAsync(reportedProperties);
      }
      catch (Exception ex)
      {
        Console.WriteLine();
        Console.WriteLine("Error in sample: {0}", ex.Message);
      }
    }

    public static async Task UpdateDesiredProperties(string deviceId)
    {
      var registryManager = Microsoft.Azure.Devices.RegistryManager.CreateFromConnectionString(IoTHubConnectionString);
      var twin = await registryManager.GetTwinAsync(deviceId).ConfigureAwait(false);

      var patch =
        @"{
                properties: {
                    desired: {
                      updater: '2.0.2'
                    }
                }
            }";

      await registryManager.UpdateTwinAsync(twin.DeviceId, patch, twin.ETag).ConfigureAwait(false);
    }
  }
}
