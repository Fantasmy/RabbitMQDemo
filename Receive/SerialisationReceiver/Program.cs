using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialisationReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonService commonService = new CommonService();
            IConnection connection = commonService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();
            ReceiveSerialisationMessages(model);
        }
        private static void ReceiveSerialisationMessages(IModel model)
        {
            model.BasicQos(0, 1, false);  // para temas de configuración y envío. La configuración más básica.
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(CommonService.SerialisationQueueName, false, consumer);  // si cambias el false a true, el receptor estarái escuchando todo el rato
            while (true)  // que se mantenga encendido mientras vayamos enviando
            {
                BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;  //llama al Dequeue
                String jsonified = Encoding.UTF8.GetString(deliveryArguments.Body);
                Student student = JsonConvert.DeserializeObject<Student>(jsonified);  // deserializa el alumno
                Console.WriteLine("Pure json: {0}", jsonified);
                Console.WriteLine("Student info- Name: {0} , Surname: {1} , Dni: {2}", student.Name, student.Surname, student.Dni);
                model.BasicAck(deliveryArguments.DeliveryTag, false);
            }
        }
    }
}
