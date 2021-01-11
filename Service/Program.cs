using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Service
{
    class Program
    {//引用nuget   rabbitmq.client
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "guest";
            //factory.Password = "guest";
            int i = 0;
            //using (var connection = factory.CreateConnection())
            //{
            var connection = factory.CreateConnection();//这里demo没关闭
            var channel = connection.CreateModel();//这里demo没关闭
            var declare = channel.QueueDeclare("kibaQueue", false, false, false, null);
            var eventingBasicConsumer = new EventingBasicConsumer(channel);
            //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
            //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认 
            channel.BasicConsume(queue: "kibaQueue",
                autoAck: false,
                consumer: eventingBasicConsumer);//关闭自动回执

            eventingBasicConsumer.Received += (sender, ea) =>
            {
                Console.WriteLine(sender.GetType());
                Console.WriteLine(ea.Body);
                Console.WriteLine(Encoding.UTF8.GetString(ea.Body.ToArray()));
                Console.WriteLine("事件：" + Thread.CurrentThread.ManagedThreadId);
                    //手动发送回执
                    channel.BasicAck(ea.DeliveryTag, false);
            };
            //prefetchCount在服务端搜到n条消息时不在发送消息（此时服务端忙碌中）
            channel.BasicQos(0, 3, false);
            //}

            Console.WriteLine("主：" + Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
            Console.WriteLine($"i：{i}");
        }
    }
}
