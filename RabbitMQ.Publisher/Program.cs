using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Publisher
{
    public enum LogNames
    {
        Criticial=1,
        Error=2,
        Warning=3,
        Info=4
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://ddxgksni:M5FbF99hG84oMBfy7zE8FQsgfyt_ldXR@chimpanzee.rmq.cloudamqp.com/ddxgksni");

            using var connection=factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("log-direct",durable:true,type:ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct",routeKey,null);
            });

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);
                string message = $"log-type: {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("log-direct",routeKey, null, messageBody);

                Console.WriteLine($"Log gönderilmiştir: {message}"); 
            });

            Console.ReadLine();
        }
    }
}