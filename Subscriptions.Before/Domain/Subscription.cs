using System;
using Subscriptions.Before.SharedKernel;

namespace Subscriptions.Before.Domain
{
    public class Subscription : Entity
    {        
        public SubscriptionStatus Status { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public decimal Amount { get; set; }
        public DateTime CurrentPeriodEndDate { get; set; }

        public Subscription(Customer customer, Product product, decimal amount) : this()
        {
            Id = Guid.NewGuid();
            Status = SubscriptionStatus.Active;
            Customer = customer;
            Product = product;
            Amount = amount;
            CurrentPeriodEndDate = product.BillingPeriod.CalculateBillingPeriodEndDate();
        }

        private Subscription()
        {
        }
    }
}