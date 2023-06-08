using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace Shop
{
    public class OrderData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int amount { get; set; }
        public string name { get; set; }
        public Guid? timeoutId { get; set; }
    }

    public class OrderSaga : MassTransitStateMachine<OrderData>
    {
        public State Unconfirmed { get; private set; }
        public State ConfirmedByWarehouse { get; private set; }
        public State ConfirmedByClient { get; private set; }

        public Event<Order> OrderEvent { get; private set; }
        public Event<WarehouseConfirmationResponseAccept> WarehouseAcceptEvent { get; private set; }
        public Event<WarehouseConfirmationResponseReject> WarehouseRejectEvent { get; private set; }
        public Event<ClientConfirmationResponseAccept> ClientAcceptEvent { get; private set; }
        public Event<ClientConfirmationResponseReject> ClientRejectEvent { get; private set; }

        public Event<Timeout> TimeoutEvt { get; private set; }
        public Schedule<OrderData, Timeout> TO { get; private set; }

        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderEvent, x =>
                x.CorrelateBy(s => s.name, ctx => ctx.Message.name)
                .SelectId(context => Guid.NewGuid())
            );

            Schedule(() => TO,
                x => x.timeoutId,
                x => x.Delay = TimeSpan.FromSeconds(10));

            Initially(
                When(OrderEvent)
                    .Then(ctx => ctx.Saga.amount = ctx.Message.amount)
                    .Then(ctx => ctx.Saga.name = ctx.Message.name)
                    .Then(ctx => Console.WriteLine($"New order: amount={ctx.Data.amount} name={ctx.Data.name} id={ctx.Instance.CorrelationId}"))
                    .Respond(ctx => new ConfirmationRequest() { CorrelationId = ctx.Instance.CorrelationId, amount = ctx.Instance.amount, name = ctx.Instance.name })
                    .TransitionTo(Unconfirmed)
                    .Schedule(TO, ctx => new Timeout() { CorrelationId = ctx.Instance.CorrelationId })
            );

            During(Unconfirmed,
                When(WarehouseAcceptEvent)
                    .Then(ctx => Console.WriteLine("Confirmed by the warehouse. Waiting for the client."))
                    .TransitionTo(ConfirmedByWarehouse),
                When(ClientAcceptEvent)
                    .Then(ctx => Console.WriteLine("Confirmed by the client. Waiting for the warehouse."))
                    .TransitionTo(ConfirmedByClient),
                When(TimeoutEvt)
                    .Then(ctx => Console.WriteLine("Timeout: Order was not confirmed by the warehouse or client within 10 seconds."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "timeout", amount = ctx.Instance.amount })
                    .Finalize(),
                When(WarehouseRejectEvent)
                    .Then(ctx => Console.WriteLine("Rejected by the warehouse."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "rejected by the warehouse", amount = ctx.Instance.amount })
                    .Finalize(),
                When(ClientRejectEvent)
                    .Then(ctx => Console.WriteLine("Rejected by the client."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "rejected by the client", amount = ctx.Instance.amount })
                    .Finalize()
            );

            During(ConfirmedByWarehouse,
                When(ClientAcceptEvent)
                    .Then(ctx => Console.WriteLine("Confirmed by the client and the warehouse. Success."))
                    .Unschedule(TO)
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "success", amount = ctx.Instance.amount })
                    .Finalize(),
                When(TimeoutEvt)
                    .Then(ctx => Console.WriteLine("Timeout: Order was not confirmed by the warehouse or client within 10 seconds."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "timeout", amount = ctx.Instance.amount })
                    .Finalize(),
                When(WarehouseRejectEvent)
                    .Then(ctx => Console.WriteLine("Rejected by the warehouse."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "rejected by the warehouse", amount = ctx.Instance.amount })
                    .Finalize(),
                When(ClientRejectEvent)
                    .Then(ctx => Console.WriteLine("Rejected by the client."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "rejected by the client", amount = ctx.Instance.amount })
                    .Finalize()
            );

            During(ConfirmedByClient,
                When(WarehouseAcceptEvent)
                    .Then(ctx => Console.WriteLine("Confirmed by the client and the warehouse. Success."))
                    .Unschedule(TO)
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "success", amount = ctx.Instance.amount })
                    .Finalize(),
                When(TimeoutEvt)
                    .Then(ctx => Console.WriteLine("Timeout: Order was not confirmed by the warehouse or client within 10 seconds."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "timeout", amount = ctx.Instance.amount })
                    .Finalize(),
                When(WarehouseRejectEvent)
                    .Then(ctx => Console.WriteLine("Rejected by the warehouse."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "rejected by the warehouse", amount = ctx.Instance.amount })
                    .Finalize(),
                When(ClientRejectEvent)
                    .Then(ctx => Console.WriteLine("Rejected by the client."))
                    .Respond(ctx => new ShopResponse() { CorrelationId = ctx.Instance.CorrelationId, text = "rejected by the client", amount = ctx.Instance.amount })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var repo = new InMemorySagaRepository<OrderData>();
            var machine = new OrderSaga();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(new Uri("rabbitmq://localhost"),
                    h => { h.Username("guest"); h.Password("guest"); });
                sbc.ReceiveEndpoint("shopqueue", ep =>
                {
                    ep.StateMachineSaga(machine, repo);
                });
                sbc.UseInMemoryScheduler();
            });
            bus.Start();
            Console.WriteLine("Shop started");
            Console.ReadKey();
            bus.Stop();
        }
    }
}