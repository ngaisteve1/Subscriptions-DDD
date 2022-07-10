using MediatR;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Commands;
using System.Threading.Tasks;

namespace Subscriptions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(CustomerRequest request)
        {
            await _mediator.Send(request);

            return Ok();
        }
    }
}
