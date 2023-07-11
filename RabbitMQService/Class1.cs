using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQService
{
    public class Events
    {
        public void SendMsg(string QueName, string integrationEvent, string eventData)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: QueName,
                                             routingKey: integrationEvent,
                                             basicProperties: null,
                                             body: body);
        }
        public void SubscribePayment(string ServName)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                //add code to send data to shipping service  here 
            };
            channel.BasicConsume(queue: ServName,
                                     autoAck: true,
                                     consumer: consumer);
        }

        public  void SubscribeNotifications(string ServName)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "order.placed")
                {
                    //send email here for placing order
                }
                else if (type == "order.shipped")
                {
                    //send email here for shipping order 
                }
            };
            channel.BasicConsume(queue: ServName,
                                     autoAck: true,
                                     consumer: consumer);
        }
    }
}