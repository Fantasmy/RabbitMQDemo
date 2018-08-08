using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObjects
{
    public class CommonService
    {
        private string _hostName = "localhost";  // se podria poner en la pp.config
        private string _userName = "guest";
        private string _password = "guest";

        public static string SerialisationQueueName = "SerialisationDemoQueue"; // nombre de la cola

        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = _hostName;    //el host es la ip
            connectionFactory.UserName = _userName;
            connectionFactory.Password = _password;

            return connectionFactory.CreateConnection();
        }
    }
}
