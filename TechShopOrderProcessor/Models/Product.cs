using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopOrderProcessor.Services;

namespace TechShopOrderProcessor.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Weight { get; set; } //em kg
        public int StockQuantity { get; set; }

        public Product(int id, string name, decimal price, double weight, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Weight = weight;
            StockQuantity = stockQuantity;
        }
    }
}

// Customer.cs

namespace TechShopOrderProcessor.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }


        public Customer(int id, string name, string email, string address, string zipCode)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            ZipCode = zipCode;
        }

        public bool IsValidAddress()
        {
            // Implementação básica de validação
            return !string.IsNullOrWhiteSpace(Address) &&
                   !string.IsNullOrWhiteSpace(ZipCode) &&
                   ZipCode.Length == 8 &&
                   ZipCode.All(char.IsDigit);
        }
    }
}

// OrderItem.cs
namespace TechShopOrderProcessor.Models
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Product.Price * Quantity;

        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}

// Order.cs
namespace TechShopOrderProcessor.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Subtotal => Items.Sum(item => item.Subtotal);
        public decimal DiscountAmount { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal Total => Subtotal - DiscountAmount + ShippingCost;
        public string Status { get; set; }

        public Order(int id, Customer customer)
        {
            Id = id;
            OrderDate = DateTime.Now;
            Customer = customer;
            Items = new List<OrderItem>();
            Status = "Pending";
        }

        public void AddItem(Product product, int quantity)
        {
            Items.Add(new OrderItem(product, quantity));
        }

        public void CalculateDiscount()
        {
            // 10% de desconto para compras acima de R$300
            DiscountAmount = Subtotal >= 300 ? Subtotal * 0.1m : 0;
        }

        public void CalculateShipping(IShippingCalculator shippingCalculator)
        {
            double totalWeight = Items.Sum(item => item.Product.Weight * item.Quantity);
            ShippingCost = shippingCalculator.Calculate(Customer.ZipCode, totalWeight);
        }
    }
}