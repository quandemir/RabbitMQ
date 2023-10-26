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

            channel.ExchangeDeclare("log-topic",durable:true,type:ExchangeType.Direct);

            Random rnd = new Random();
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log1 = (LogNames)rnd.Next(1, 5);
                LogNames log2 = (LogNames)rnd.Next(1, 5);
                LogNames log3 = (LogNames)rnd.Next(1, 5);
                var routeKey = $"{log1}.{log2}.{log3}";
                string message = $"log-type: {log1}-{log2}-{log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);


                channel.BasicPublish("log-topic",routeKey, null, messageBody);

                Console.WriteLine($"Log gönderilmiştir: {message}"); 
            });

            Console.ReadLine();
        }
    }
}