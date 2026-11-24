namespace RestaurantApi.Models;

public class Order
{
     public int Id  {get;set;}

     public DateTimeOffset CreatedAt {get; set;}
     public OrderStatus Status {get; set;}= OrderStatus.Pending;

     public List<OrderItem> Items {get; set;}= new List<OrderItem>();
     public decimal TotalAmount { get; set; }
}