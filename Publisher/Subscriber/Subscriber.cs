using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;

public class Subscriber
{
    static async Task Main(string[] args)
    {
        var mqttfactory = new MqttFactory();
        IMqttClient client = mqttfactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
                          .WithClientId(Guid.NewGuid().ToString())
                          .WithTcpServer("test.mosquitto.org", 1883)
                          .WithCleanSession()
                          .Build();

        client.UseConnectedHandler(async e =>
        {
            Console.WriteLine("Connected to the broker successfully");
            var topicBuilder = new TopicFilterBuilder()
                                   .WithTopic("Samin")
                                   .Build();

            await client.SubscribeAsync(topicBuilder);
        });

        client.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("Disconnected to the broker successfully");
        });

        client.UseApplicationMessageReceivedHandler(e =>
        {
            Console.WriteLine($"Received Message - {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        });

        await client.ConnectAsync(options);

       
        Console.ReadLine();

        await client.DisconnectAsync();

       
    }

}