using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://ddxgksni:M5FbF99hG84oMBfy7zE8FQsgfyt_ldXR@chimpanzee.rmq.cloudamqp.com/ddxgksni");

            using var connection=factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("log-fanout",durable:true,type:ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"log {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("log-fanout","", null, messageBody);

                Console.WriteLine($"Mesajınız gönderilmiştir: {message}"); 
            });

            Console.ReadLine();
        }
    }
}