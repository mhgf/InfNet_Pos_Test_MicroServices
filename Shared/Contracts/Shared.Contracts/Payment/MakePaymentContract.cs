namespace Shared.Contracts.Payment;

public class MakePaymentContract
{
    public string Card { get; set; } = string.Empty;
    public int Value { get; set; }
    public Guid OrderId { get; set; }
}