namespace APIs.Models
{
    public class OrderModels(
    );

    public record CreateOrderRequest(
    string CustomerEmail,
    string ItemName,
    int Quantity,
    decimal UnitPrice
);

    public record OrderResponseModels(
        Guid Id,
        string CustomerEmail,
        string ItemName,
        int Quantity,
        decimal TotalPrice,
        DateTime CreatedAt
    );
}
