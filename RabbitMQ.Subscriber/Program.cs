using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://ddxgksni:M5FbF99hG84oMBfy7zE8FQsgfyt_ldXR@chimpanzee.rmq.cloudamqp.com/ddxgksni");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var randomQueueName = "log-database-save";//channel.QueueDeclare().QueueName;

            channel.QueueDeclare(randomQueueName, true, false, false);

            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);//her bir subscriber için 5 er mesaj gönderir(false)---true olursa 6 mesajı tek seferde 3e3 diye gönderir

            var consumer=new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName,false ,consumer);

            Console.WriteLine("Loglar dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message=Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen mesaj:" + message);
                channel.BasicAck(e.DeliveryTag, false);//mesajı siliyor yukarıda false yaptık
            };

            Console.ReadLine();
        }
    }
}