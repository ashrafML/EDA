using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Payments
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();
           
            var eventBus = app.Services.GetRequiredService<RabbitMQService.Events>();
            eventBus.SubscribePayment("order.payments");
            app.Run();
        }
      
    }
}