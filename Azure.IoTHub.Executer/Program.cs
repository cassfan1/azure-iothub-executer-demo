using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IoTHub.Executer
{
  class Program
  {
    private static readonly string DeviceConnectionString = "HostName=iot-hub-cass-test.azure-devices.net;DeviceId=dps-device-010;SharedAccessKey=ab0ff99f9d72da5e08e34968cf46ede30874981c9cddd9168e3809dbc7049cee";

    static async Task Main(string[] args)
    {
      DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
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
  }
}
