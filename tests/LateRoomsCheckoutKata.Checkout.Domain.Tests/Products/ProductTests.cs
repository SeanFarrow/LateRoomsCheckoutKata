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
        }
    }
}