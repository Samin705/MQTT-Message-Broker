using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;

public class Publisher
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
        client.UseConnectedHandler(e =>
        {
            Console.WriteLine("Connected to the broker successfully");
        });

        client.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("Disconnected to the broker successfully");
        });

        await client.ConnectAsync(options);

        Console.WriteLine("Pres a key to publish the message");
        Console.ReadLine();

        await PublishMessageAsync(client);

      await client.DisconnectAsync();
    }

    private static async Task PublishMessageAsync(IMqttClient client)
    {
        string messagePayLoad = "Hello";
        var message = new MqttApplicationMessageBuilder()
                          .WithTopic("Samin")
                          .WithPayload(messagePayLoad)
                          .WithAtLeastOnceQoS()
                          .Build();

        if(client.IsConnected)
        {
           await client.PublishAsync(message);
        }
    }
}