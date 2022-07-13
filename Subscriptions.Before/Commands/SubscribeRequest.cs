using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Subscriptions.Before.Data;
using Subscriptions.Before.Domain;
using Subscriptions.Before.Domain.Services;
using Subscriptions.Before.Services;

namespace Subscriptions.Before.Commands
{
    public class SubscribeRequest : IRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
    }
    public class SubscribeRequestHandler : IRequestHandler<SubscribeRequest>
    {
        private readonly SubscriptionContext _subscriptionContext;
        private readonly IEmailSender _emailSender;
        private readonly ISubscriptionAmountCalculator _subscriptionAmountCalculator;

        public SubscribeRequestHandler(SubscriptionContext subscriptionContext,
            IEmailSender emailSender, ISubscriptionAmountCalculator subscriptionAmountCalculator)
        {
            _subscriptionContext = subscriptionContext;
            _emailSender = emailSender;
            _subscriptionAmountCalculator = subscriptionAmountCalculator;
        }
        public async Task<Unit> Handle(SubscribeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                
                var customer = await _subscriptionContext
                                .Customers
                                .Include(x => x.Subscriptions)
                                .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken: cancellationToken);
                // todo: validation - return nocontent if customer is null

                var product = await _subscriptionContext.Products.FindAsync(request.ProductId);
                // todo: validation - return nocontent if product is null

                // todo: validate - not to add the same subscription to the same customer.
                customer.AddSubscription(product, _subscriptionAmountCalculator);

                await _subscriptionContext.SaveChangesAsync(cancellationToken);

                // todo: validate - if SaveChangesAsync return is > 0, then only send email.
                await _emailSender.SendEmailAsync("Congratulations! You subscribed to a cool product");
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            



        }
    }
}