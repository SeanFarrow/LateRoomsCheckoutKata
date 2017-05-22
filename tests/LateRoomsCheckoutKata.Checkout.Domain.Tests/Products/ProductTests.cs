using FluentAssertions;
using LateRoomsCheckoutKata.Checkout.Domain.Products;
using NUnit.Framework;
using System;

namespace LateRoomsCheckoutKata.Checkout.Domain.Tests.Products
{
    public class ProductTests
    {
        /// <summary>
        /// This class provides all tests for the <see cref="Product"/> constructor.
        /// </summary>
        [TestFixture]
        public class TheProductConstructorTests
        {
            [Test]
            public void ShouldThrowAnArgumentNullExceptionWithAnAppropriateMessageWhenTheSKUPassedInIsNull()
            {
                string sku = null;
                uint unitPrice = 50;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentNullException>().And.Message.Should().Be("A null sku has been passed in. The sku cannot be null.\r\nParameter name: sku");
            }

            [Test]
            public void ShouldThrowAnArgumentExceptionWithAnAppropriateMessageWhenTheSKUPassedInIsAnEmtpyString()
            {
                string sku = String.Empty;
                uint unitPrice = 50;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentException>().And.Message.Should().Be("An empty sku has been passed in. The sku cannot be an empty string.\r\nParameter name: sku");
            }
            
            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWithAnAppropriateMessageWhenTheUnitPricePassedInIs0()
            {
                string sku = "a";
                uint unitPrice = 0;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentOutOfRangeException>().And.Message.Should().Be("0 has been passed in as the unit price.The unit price must be 1 or greater.\r\nParameter name: unitPrice");
            }
        }
    }
}