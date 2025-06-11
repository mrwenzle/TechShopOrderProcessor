using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopOrderProcessor.Models;


// ReportGenerator.cs
namespace TechShopOrderProcessor.Utils
{
    public class ReportGenerator
    {
        public static void GenerateOrdersCsv(List<Order> orders, string filePath)
        {
            StringBuilder csv = new StringBuilder();

            // Cabeçalho
            csv.AppendLine("ID,Data,Cliente,Email,CEP,Subtotal,Desconto,Frete,Total,Status");

            // Dados
            foreach (var order in orders)
            {
                csv.AppendLine($"{order.Id}," +
                               $"{order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")}," +
                               $"\"{order.Customer.Name}\"," +
                               $"{order.Customer.Email}," +
                               $"{order.Customer.ZipCode}," +
                               $"{order.Subtotal}," +
                               $"{order.DiscountAmount}," +
                               $"{order.ShippingCost}," +
                               $"{order.Total}," +
                               $"{order.Status}");
            }

            // Escrever arquivo
            File.WriteAllText(filePath, csv.ToString());
            Console.WriteLine($"Relatório gerado em: {filePath}");
        }
    }
}