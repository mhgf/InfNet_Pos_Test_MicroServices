using Shared.Bus.Producer;
using Shared.Contracts.Product;

namespace Order.Core.Bus;

public interface ISenderProduct :ISenderMessage<ProductSoldContract>
{
    
}

public class SenderProduct(string host, string queueName) : SenderMessage<ProductSoldContract>(host, queueName), ISenderProduct
{
    
}