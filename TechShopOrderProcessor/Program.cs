using TechShopOrderProcessor.Models;
using TechShopOrderProcessor.Services;
using TechShopOrderProcessor.Utils;

namespace TechShopOrderProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TechShop - Sistema de Processamento de Pedidos");
            Console.WriteLine("==============================================");

            // Criar alguns produtos para teste
            List<Product> inventory = new List<Product>
            {
                new Product(1, "Smartphone XYZ", 1299.99m, 0.3, 10),
                new Product(2, "Notebook ABC", 3499.99m, 2.5, 5),
                new Product(3, "Mouse Gamer", 99.90m, 0.1, 20),
                new Product(4, "Teclado Mecânico", 249.90m, 0.8, 15),
                new Product(5, "Monitor 24\"", 899.90m, 3.0, 8)
            };

            // Criar alguns clientes para teste
            List<Customer> customers = new List<Customer>
            {
                new Customer(1, "João Silva", "joao@email.com", "Rua A, 123", "12345678"),
                new Customer(2, "Maria Souza", "maria@email.com", "Av. B, 456", "87654321"),
                new Customer(3, "Pedro Santos", "pedro@email.com", "Praça C, 789", "23456789"),
                new Customer(4, "Ana Oliveira", "ana@email.com", "Rua D, 321", "98765432"),
                new Customer(5, "Carlos Pereira", "carlos@email.com", "", "12345") // Endereço inválido
            };

            // Criar alguns pedidos para teste
            List<Order> orders = new List<Order>();

            // Pedido 1 - Válido, com desconto
            var order1 = new Order(1, customers[0]);
            order1.AddItem(inventory[0], 1); // 1 Smartphone
            order1.AddItem(inventory[2], 2); // 2 Mouses
            orders.Add(order1);

            // Pedido 2 - Válido, sem desconto
            var order2 = new Order(2, customers[1]);
            order2.AddItem(inventory[2], 1); // 1 Mouse
            order2.AddItem(inventory[3], 1); // 1 Teclado
            orders.Add(order2);

            // Pedido 3 - Inválido, estoque insuficiente
            var order3 = new Order(3, customers[2]);
            order3.AddItem(inventory[1], 10); // 10 Notebooks (estoque insuficiente)
            orders.Add(order3);

            // Pedido 4 - Inválido, endereço inválido
            var order4 = new Order(4, customers[4]);
            order4.AddItem(inventory[4], 1); // 1 Monitor
            orders.Add(order4);

            // Processar pedidos
            var shippingCalculator = new SimpleShippingCalculator();
            var orderProcessor = new OrderProcessor(shippingCalculator);

            foreach (var order in orders)
            {
                Console.WriteLine($"\nProcessando pedido {order.Id}...");
                orderProcessor.ProcessOrder(order, inventory);
            }

            // Gerar relatório
            string reportPath = Path.Combine(Environment.CurrentDirectory, "orders_report.csv");
            ReportGenerator.GenerateOrdersCsv(orders, reportPath);

            // Mostrar estoque atualizado
            Console.WriteLine("\nEstoque atualizado:");
            foreach (var product in inventory)
            {
                Console.WriteLine($"{product.Name}: {product.StockQuantity} unidades");
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}