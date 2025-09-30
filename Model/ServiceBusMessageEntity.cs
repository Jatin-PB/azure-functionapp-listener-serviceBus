using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusListener2.Model
{
    public class ServiceBusMessageEntity
    {
        public int Id { get; set; }
        public string MessageBody { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
