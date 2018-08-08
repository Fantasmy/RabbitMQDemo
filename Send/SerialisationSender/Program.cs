using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialisationSender
{
    class Program
    {
        static void Main(string[] args)
        {
            CommonService commonService = new CommonService();
            IConnection connection = commonService.GetRabbitMqConnection(); // se crea la conexión
            IModel model = connection.CreateModel(); // crea modelo
            SetupSerialisationMessageQueue(model);   
            RunSerialisationDemo(model);  //método, que llama el model
        }
        private static void SetupSerialisationMessageQueue(IModel model)
        {
            model.QueueDeclare(CommonService.SerialisationQueueName, true, false, false, null);
        }

        private static void RunSerialisationDemo(IModel model)
        {
            Console.WriteLine("Enter student name, surname, dni (press enter after each field). Quit with 'q'.");
            while (true)
            {
                //Refacturizar con lo de Dani

                string studentId = Console.ReadLine();
                string studentName = Console.ReadLine();
                string studentSurname = Console.ReadLine();
                string studentDni = Console.ReadLine();
                //if (studentName.ToLower() == "q") break;
                //Student student = new Student() { Name = studentName };
                if (studentName.ToLower() == "q") break;
                Student student = new Student() { Id = Convert.ToInt32(studentId), Name = studentName, Surname = studentSurname, Dni = studentDni };

                IBasicProperties basicProperties = model.CreateBasicProperties();  // se crean las properties de la cola
                basicProperties.SetPersistent(true);
                String jsonified = JsonConvert.SerializeObject(student);  // serializacion
                byte[] studentBuffer = Encoding.UTF8.GetBytes(jsonified); //devuelve array de bites
                model.BasicPublish("", CommonService.SerialisationQueueName, basicProperties, studentBuffer);
            }
        }
    }
}
