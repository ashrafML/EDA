using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using RabbitMQService;
namespace E_Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly RabbitMQService.Events rbmq;

        public ValuesController()
        {
            rbmq = new RabbitMQService.Events();
        }
        public async Task<IActionResult> PlaceOrder(dynamic OrderDetails)
        {
            var orderkey = 10;//save order in db and generate it's key
            await Task.Delay(100);

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                orderkey = orderkey,
                userID = OrderDetails.userID,
                userName = OrderDetails.userName,
                UserEmail= OrderDetails.userEmail
            });
            rbmq.SendMsg("order", "order.placed", integrationEventData);
           
            return NoContent();
        }
    
    }
}
