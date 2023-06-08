using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Messages
{
    public class Order
    {
        public int amount { get; set; }
        public string name { get; set; }
    }

    public class ConfirmationRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public int amount { get; set; }
        public string name { get; set; }
    }

    public class ClientConfirmationResponseAccept : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class ClientConfirmationResponseReject : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class WarehouseConfirmationResponseAccept : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class WarehouseConfirmationResponseReject : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public class ShopResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public string text { get; set; }
        public int amount { get; set; }
    }

    public class Timeout : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

}
