namespace Shared.Bus.Producer;

public interface ISenderMessage<in TContract>
{
    Task Send(TContract msg);
}