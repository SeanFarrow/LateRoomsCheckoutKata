using FluentAssertions;
using NUnit.Framework;
using LateRoomsCheckoutKata .Checkout.Domain.Products;
using System;

namespace LateRoomsCheckoutKata.Checkout.Domain.Tests.Products
{
    public class ProductWithSKUNotFoundExceptionTests
    {
        /// <summary>
        /// This class provides all tests for the <see cref="ProductWithSKUNotFoundException.CheckSKUStringIsValid"/> method.
        /// </summary>
        [TestFixture]
        public class TheProductWithSKUNotFoundExceptionConstructor
        {
            [Test]
            public void ShouldThrowAnArgumentNullExceptionWithAnAppropriateMessageWhenTheMessageCannotBeAssignedBecauseTheSKUStringPassedInIsNull()
            {
                Action act = () => new ProductWithSKUNotFoundException(null);
                act.ShouldThrow<ArgumentNullException>().And.Message.Should().Be("A null sku has been passed in. The sku cannot be null.\r\nParameter name: sku");
            }
            
            [Test]
            public void ShouldThrowAnArgumentExceptionWithAnAppropriateMessageWhenTheMessageCannotBeAssignedBecauseTheSKUStringPassedInIsEmpty()
            {
                Action act = () => new ProductWithSKUNotFoundException(String.Empty);
                act.ShouldThrow<ArgumentException>().And.Message.Should().Be("An empty sku has been passed in. The sku cannot be an empty string.\r\nParameter name: sku");
            }
            
            [Test]
            public void ShouldAssignTheMessagePropertyWhenTheSKUPassedInIsNotNullOrEmpty()
            {
                var createdException =new ProductWithSKUNotFoundException("a");
                createdException.Message.Should()
                    .Be("The product with stock keeping unit 'a' could not be found in the product repository.");
            }
       }
    }
}
