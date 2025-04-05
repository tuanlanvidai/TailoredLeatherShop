using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.ModelView
{
    // Models/ModelView/OrderView.cs
    public class OrderView
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
        public float QuotePrice { get; set; }
        public string AdminNote { get; set; }
        public float DepositAmount { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShippingAddress { get; set; }
        public string AdditionalRequest { get; set; }
        public List<OrderItemView> OrderItems { get; set; }
    }

    public class OrderItemView
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public string CustomNote { get; set; }
    }

    public class OrderCreateView
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string CustomNote { get; set; }
        public string ShippingAddress { get; set; }
        public string AdditionalRequest { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class AdminQuoteView
    {
        public int OrderId { get; set; }
        public float QuotePrice { get; set; } 
        public string AdminNote { get; set; } 
        public DateTime? EstimatedDeliveryDate { get; set; }
    }

}