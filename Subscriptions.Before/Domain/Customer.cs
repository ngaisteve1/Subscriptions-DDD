using System.Collections.Generic;
using Subscriptions.Before.SharedKernel;

namespace Subscriptions.Before.Domain
{
    public class Customer: Entity
    {
        public string Email { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal MoneySpent { get; set; }
        public List<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

        public void AddSubscription(Product product, Services.ISubscriptionAmountCalculator subscriptionAmountCalculator)
        {
            decimal subscriptionAmount = subscriptionAmountCalculator.Calculate(product,this);

            var subscription = new Subscription(this, product, subscriptionAmount);

            Subscriptions.Add(subscription);
            MoneySpent += subscription.Amount;
        }
    }
}