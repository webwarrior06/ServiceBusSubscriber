using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusSubscriber
{
    public class ServiceBusConfiguration
    {
        public string Endpoint { get; set; } =
            "Endpoint=sb://dev-bilagos-bus.servicebus.windows.net/;SharedAccessKeyName=SharedAccessKey;SharedAccessKey=xKti1AcGty+W9EGKseDjQAc5Sl5TUp2U3te2hTPEkk8=";
        public string TopicName { get; set; } = "tahalocal";
        public string RestApiSubscriptionName { get; set; } = "FirstSub"; //????
        public string InvoiceSubscriptionName { get; set; } = "FirstSub";
        public short LockDurationMinutes { get; set; } = 5;
        public double TimeoutDurationMinutes { get; set; } = 5;
        public bool ConsumeMessages { get; set; } = false;
    }
}

