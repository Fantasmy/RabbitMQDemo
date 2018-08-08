using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Receive
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null); //declaramos la misma cola

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>   // (Para recibir))Received que estáa antes de += es el EVENTO. += le estás pasando el object sender y el event handler. El evento se produce cuando se recibe el mensaje. AL utilizar ea trabaja con sus propios eventargs.  model, ea-> en estos parámetros recibirá lo que el delegado ha marcado, un object en este caso, peor también el BasicDeliverArgs con el mensaje en el body.
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    };
                    channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer); // (Consumo, devuelve void) aqui se procesa el mensaje, se reciben. Se podría poner en otro método aparte. Después de consumir se ejecutará consumer.Receiver, porque es ASINCRONO. El CONSUME CREA UN SUSCRIPTOR. Aqui solo te suscribes, solo es configuración. Quiero escuchar lo que se envía a la cola hello.

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();  // leemos los datos
                }
        }
    }
}