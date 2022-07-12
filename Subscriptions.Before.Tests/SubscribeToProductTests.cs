using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using Subscriptions.Before.Commands;
using Subscriptions.Before.Data;
using Subscriptions.Before.Domain;
using Subscriptions.Before.Domain.Services;
using Subscriptions.Before.Services;
using Xunit;

namespace Subscriptions.Before.Tests
{
    public class SubscribeToProductTests
    {
        [Fact]
        public async Task Should_Add_Subscription_To_Database()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<SubscriptionContext>()
                .UseSqlServer("Server=.;Database=Subscriptions;Trusted_Connection=True;")
                .Options;
            var context = new SubscriptionContext(options);
            await context.Database.EnsureCreatedAsync();

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "sample@example.org",
                FirstName = "Hossam",
                LastName = "Barakat"
            };
            await context.Customers.AddAsync(customer);
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Weekly Bunch",
                Amount = 10,
                BillingPeriod = BillingPeriod.Monthly
            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var sut = new SubscribeRequestHandler(context,
                Substitute.For<IEmailSender>(), new SubscriptionAmountCalculator());

            var subscribeRequest = new SubscribeRequest
            {
                CustomerId = customer.Id,
                ProductId = product.Id
            };

            // Act
            await sut.Handle(subscribeRequest, CancellationToken.None);

            // Assert
            var subscription = await context.Subscriptions
                .SingleOrDefaultAsync(x=>x.Customer.Id == customer.Id && x.Product.Id == product.Id);
            subscription.ShouldNotBeNull();
        }
    }
}