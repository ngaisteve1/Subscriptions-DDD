using MediatR;
using Subscriptions.Data;
using Subscriptions.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriptions.Commands
{
    public class CustomerRequest : IRequest {
        public Email email { get; set; }
        public CustomerName customerName { get; set; }        
    }

    public class CustomerRequestHandler : IRequestHandler<CustomerRequest>
    {
        private readonly SubscriptionContext _subscriptionContext;

        public CustomerRequestHandler(SubscriptionContext subscriptionContext)
        {
            _subscriptionContext = subscriptionContext;
        }

        public async Task<Unit> Handle(CustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = new Customer(new Email(request.email.Value), new CustomerName(request.customerName.FirstName, request.customerName.LastName));
            await _subscriptionContext.AddAsync(customer);
            var result = await _subscriptionContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
