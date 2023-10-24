using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
    public class EventBusConfig
    {
        public int ConnectionRetryCount { get; set; } = 5; // bağlanırken 5 kere dene sonra hata fırlatacağız Re-Try mekanizması kurulacak bunun için. Network problemleri yaşandığı durumlarda can yakmaması için

        public string DefaultTopicName { get; set; } = "SellinguddyEventBus"; //Default Topic name- dışarıdan verilmemesi durumda sistem hata almaması için.

        public string EventBusConnectionString { get; set; } = String.Empty;
        public string SubscriberClientAppName { get; set; } = String.Empty;
        public string EventNamePrefix { get; set; } = String.Empty;

        public string EventNameSuffix { get; set; } = "IntegrationEvent";

        public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;
        public object Connection { get; set; }

        public bool DeleteEventPrefix => !String.IsNullOrEmpty(EventNamePrefix);
        public bool DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);

    }

    public enum EventBusType
    {
        RabbitMQ = 0,
        AzureServiceBus = 1

    }
}