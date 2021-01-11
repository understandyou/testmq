using System;
using System.Text;
using RabbitMQ.Client;

namespace send
{
    class Program
    {
        /// <summary>
        /// send
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            //factory.UserName = "guest";
            //factory.Password = "guest";
            for (int i = 0; i < 1000; i++)
            {
                using (var connection = factory.CreateConnection())
                {
                    using var model = connection.CreateModel();
                    //声明一个名为：kibaQueue队列
                    //设置 autoDeleted=true 的队列，当没有消费者之后，队列会自动被删除
                    QueueDeclareOk declareOk = model.QueueDeclare("kibaQueue", false, false, false, null);
                    //构建一个空的内容头
                    var basicProperties = model.CreateBasicProperties();
                    model.BasicPublish("", "kibaQueue",
                        basicProperties, Encoding.UTF8.GetBytes("这事消息"));
                    Console.WriteLine("ok");
                } 
            }

            Console.WriteLine("完毕");


        }
    }
}
