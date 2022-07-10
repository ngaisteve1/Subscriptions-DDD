using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Subscriptions.Data;
using Subscriptions.Domain;
using Subscriptions.Domain.Services;

namespace Subscriptions.Commands
{
    public class SubscribeRequest : IRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
    }
    public class SubscribeRequestHandler : IRequestHandler<SubscribeRequest>
    {
        private readonly SubscriptionContext _subscriptionContext;
        private readonly ISubscriptionAmountCalculator _subscriptionAmountCalculator;

        public SubscribeRequestHandler(SubscriptionContext subscriptionContext, ISubscriptionAmountCalculator subscriptionAmountCalculator)
        {
            _subscriptionContext = subscriptionContext;
            _subscriptionAmountCalculator = subscriptionAmountCalculator;
        }
        public async Task<Unit> Handle(SubscribeRequest request, CancellationToken cancellationToken)
        {
            var customer = await _subscriptionContext.Customers.FindAsync(request.CustomerId);
            //var customer = new Customer(new Email("customer@example.org"), new CustomerName("Hossam", "Barakat"));
            var product = await _subscriptionContext.Products.FindAsync(request.ProductId);

            if (customer == null || product == null) return Unit.Value;

            customer.AddSubscription(product, _subscriptionAmountCalculator);

            var result = await _subscriptionContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}