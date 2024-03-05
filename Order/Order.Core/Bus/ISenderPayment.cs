using Shared.Bus.Producer;
using Shared.Contracts.Payment;

namespace Order.Core.Bus;

public interface ISenderPayment: ISenderMessage<MakePaymentContract>
{
}

public class SenderPayement(string host, string queueName) : SenderMessage<MakePaymentContract>(host, queueName), ISenderPayment
{
    
}