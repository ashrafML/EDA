using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using RabbitMQService;
namespace Shipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly RabbitMQService.Events rbmq;
        public ValuesController() 
        { 
        rbmq= new RabbitMQService.Events();
        }
        public async Task<IActionResult> PlaceShippingOrder(dynamic OrderShipDetails)
        {
            var shippedid = 50;//save shipping in db and generate its orderkey
            await Task.Delay(100);

            var integrationEventData = JsonConvert.SerializeObject(new
            {
                orderkey = OrderShipDetails.orderkey,
                userID = OrderShipDetails.userID,
                userName = OrderShipDetails.userName,
                UserEmail = OrderShipDetails.userEmail,
                shippedId= shippedid,
            });
           rbmq.SendMsg("order", "order.shipped", integrationEventData);

            return NoContent();
        }
       
    }
}
