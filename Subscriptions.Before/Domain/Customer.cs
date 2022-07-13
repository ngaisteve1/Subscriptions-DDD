using System.Collections.Generic;
using Subscriptions.Before.SharedKernel;

namespace Subscriptions.Before.Domain
{
    /// <summary>
    /// Find out from domain expert
    /// - what property is needed to make it Customer, then build the constructor from it.
    /// - what property can be changed once created.
    /// 
    /// Things to consider
    /// - make sure money spent cannot be less than zero
    /// - make sure Subscriptions cannot be cleared.
    /// </summary>
    public class Customer: Entity
    {
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public decimal MoneySpent { get; private set; }
        public List<Subscription> Subscriptions { get; private set; } = new();

        public void AddSubscription(Product product, Services.ISubscriptionAmountCalculator subscriptionAmountCalculator)
        {
            decimal subscriptionAmount = subscriptionAmountCalculator.Calculate(product,this);

            var subscription = new Subscription(this, product, subscriptionAmount);

            Subscriptions.Add(subscription);
            MoneySpent += subscription.Amount;
        }
    }
}