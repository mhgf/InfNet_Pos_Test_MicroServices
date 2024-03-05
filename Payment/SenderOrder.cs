using Shared.Bus.Producer;
using Shared.Contracts.Order;

namespace Payment.Web;

public interface ISenderOrder : ISenderMessage<PaidOrderContract>;
public class SenderOrder(string host, string queueName) : SenderMessage<PaidOrderContract>(host, queueName), ISenderOrder
{
    
}