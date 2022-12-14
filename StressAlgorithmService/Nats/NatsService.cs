using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NATS.Client;
using System;
using System.Text;
using StressAlgorithmService.Nats;

public class NatsService : INatsService
{
    private readonly IConnection? _connection;
    private IAsyncSubscription? _asyncSubscription;
    private TechnicalHealthManager technicalHealthManager;

    public NatsService()
    {
        _connection = Connect();
        technicalHealthManager = new TechnicalHealthManager(this);
    }

    public IConnection Connect()
    {
        ConnectionFactory cf = new ConnectionFactory();
        Options opts = ConnectionFactory.GetDefaultOptions();

        opts.Url = "nats://host.docker.internal:4222";
        Console.WriteLine("Trying to connect to the NATS Server");

        try
        {
            IConnection connection = cf.CreateConnection(opts);
            Console.WriteLine("Succesfully connected to the NATS server");
            return connection;
        }
        catch
        {
            Console.WriteLine("Failed to connect to the NATS server");
            return null;
        }
    }

    public void Publish<T>(string target, T data)
    {
        var message = new NatsMessage<T> { target = target, message = data };
        Console.WriteLine("Message sent: " + target);
        _connection?.Publish(target, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
    }

    public void Subscribe<T>(string target, Action<T> handler)
    {
        EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
        {
            // print the message
            string receivedMessage = Encoding.UTF8.GetString(args.Message.Data);
            Console.WriteLine("message received: " + target);

            var message = JsonConvert.DeserializeObject<T>(receivedMessage);

            handler(message);
        };

        _asyncSubscription = _connection?.SubscribeAsync(target);
        if (_asyncSubscription != null)
        {
            _asyncSubscription.MessageHandler += h;
            _asyncSubscription.Start();

            Console.WriteLine("Subscribed to: " + target);
        }
    }

    private void LogMessage(string message)
    {
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fffffff")} - {message}");
    }
}