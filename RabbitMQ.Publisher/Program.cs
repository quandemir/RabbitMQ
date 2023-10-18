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

            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"Message {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"Mesajınız gönderilmiştir: {message}"); 
            });

            Console.ReadLine();
        }
    }
}