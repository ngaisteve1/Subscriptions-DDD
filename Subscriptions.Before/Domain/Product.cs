using Subscriptions.Before.SharedKernel;
using System;

namespace Subscriptions.Before.Domain
{
    public class Product: Entity
    {
        public string Name { get; set;}
        public decimal Amount { get; set; }
        public BillingPeriod BillingPeriod { get; set; }
    }

}