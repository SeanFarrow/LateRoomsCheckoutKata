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
                int unitPrice = 50;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentNullException>().And.Message.Should().Be("A null sku has been passed in. The sku cannot be null.\r\nParameter name: sku");
            }

            [Test]
            public void ShouldThrowAnArgumentExceptionWithAnAppropriateMessageWhenTheSKUPassedInIsAnEmtpyString()
            {
                string sku = String.Empty;
                int unitPrice = 50;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentException>().And.Message.Should().Be("An empty sku has been passed in. The sku cannot be an empty string.\r\nParameter name: sku");
            }
            
            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWithAnAppropriateMessageWhenTheUnitPricePassedInIs0()
            {
                string sku = "a";
                int unitPrice = 0;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentOutOfRangeException>().And.Message.Should().Be("0 has been passed in as the unit price.The unit price must be 1 or greater.\r\nParameter name: unitPrice");
            }

            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWithAnAppropriateMessageWhenTheUnitPricePassedInIsNegative()
            {
                string sku = "a";
                int unitPrice = -200;
                Action act = () => new Product(sku, unitPrice);
                act.ShouldThrow<ArgumentOutOfRangeException>().And.Message.Should().Be("A negative number has been passed in as the unit price.The unit price must be possitive.\r\nParameter name: unitPrice");
            }

            [Test]
            public void ShouldAssignTheSkuAndUnitPricePropertiesWhenThePassedInSkuAndUnitPriceAreBothValid()
            {
                string sku = "a";
                int unitPrice = 50;
                var expectedProduct = new Product(sku, unitPrice);
                var newProduct = new Product(sku, unitPrice);
                //Here we use the ShouldBeEquivalentTo method to obviate the need for us to implement the IEquatable interface or override Equals/GetHashcode from the Object base class.
                newProduct.ShouldBeEquivalentTo(expectedProduct);
            }
        }
    }
}