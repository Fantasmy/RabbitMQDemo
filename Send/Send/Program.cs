using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Send
{
    class Program  //tarbajamos con el puerto por defecto, por eso no hace falta poner el puerto de la cola
    {
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" }; // conexion a la cola, donde le paso el host name
            using (var connection = factory.CreateConnection()) // para la conexion
                using (var channel = connection.CreateModel()) // para el canal
                {
                    channel.QueueDeclare(queue: "hello",  // aqui declaramos la cola, al declarar la cola se crea en el RabbitMq
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = "Hello World!";        //hacemos string paraa enviar mensaje
                    var body = Encoding.UTF8.GetBytes(message); // conseguimos bytes, pasamos mensaje a bytes

                    channel.BasicPublish(exchange: "",  // publicamos mensaje
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
    }
}