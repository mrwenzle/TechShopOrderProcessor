using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopOrderProcessor.Models;


// IShippingCalculator.cs
namespace TechShopOrderProcessor.Services
{
    public interface IShippingCalculator
    {
        decimal Calculate(string zipCode, double weight);
    }
}

// SimpleShippingCalculator.cs
namespace TechShopOrderProcessor.Services
{
    public class SimpleShippingCalculator : IShippingCalculator
    {
        public decimal Calculate(string zipCode, double weight)
        {
            // Implementação simplificada
            // Em um cenário real, você consultaria uma API de frete

            // Base de R$10,00 + R$5,00 por kg
            decimal baseCost = 10.0m;
            decimal costPerKg = 5.0m;

            // Simulando diferença por região baseada no primeiro dígito do CEP
            decimal regionMultiplier = 1.0m;
            if (!string.IsNullOrEmpty(zipCode) && zipCode.Length > 0)
            {
                int firstDigit = int.Parse(zipCode[0].ToString());
                regionMultiplier = 1.0m + (firstDigit * 0.1m); // Regiões com CEP iniciando com dígitos maiores pagam mais
            }

            return baseCost + (costPerKg * (decimal)weight) * regionMultiplier;
        }
    }
}

// OrderProcessor.cs
namespace TechShopOrderProcessor.Services
{
    public class OrderProcessor
    {
        private readonly IShippingCalculator _shippingCalculator;

        public OrderProcessor(IShippingCalculator shippingCalculator)
        {
            _shippingCalculator = shippingCalculator;
        }

        public bool ProcessOrder(Order order, List<Product> inventory)
        {
            // Validar endereço do cliente
            if (!order.Customer.IsValidAddress())
            {
                Console.WriteLine($"Pedido {order.Id}: Endereço inválido");
                order.Status = "Rejected - Invalid Address";
                return false;
            }

            // Verificar disponibilidade em estoque
            foreach (var item in order.Items)
            {
                var product = inventory.FirstOrDefault(p => p.Id == item.Product.Id);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    Console.WriteLine($"Pedido {order.Id}: Produto {item.Product.Name} sem estoque suficiente");
                    order.Status = "Rejected - Insufficient Stock";
                    return false;
                }
            }

            // Calcular desconto
            order.CalculateDiscount();

            // Calcular frete
            order.CalculateShipping(_shippingCalculator);

            // Atualizar estoque
            foreach (var item in order.Items)
            {
                var product = inventory.FirstOrDefault(p => p.Id == item.Product.Id);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                }
            }

            order.Status = "Processed";
            Console.WriteLine($"Pedido {order.Id} processado com sucesso. Total: R${order.Total}");
            return true;
        }
    }
}