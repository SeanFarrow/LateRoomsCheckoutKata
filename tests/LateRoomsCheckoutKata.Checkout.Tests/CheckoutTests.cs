using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using LateRoomsCheckoutKata.Checkout.Domain.Products;

namespace LateRoomsCheckoutKata.Checkout.Tests
{
    public class CheckoutTests
    {
        /// <summary>
        /// The class provides all tests for the <see cref="Checkout"/> constructor.
        /// </summary>
        [TestFixture]
        public class TheCheckoutConstructor
        {
            [Test]
            public void ShouldThrowAnArgumentNullExceptionWithAnAppropriateMessageWhenTheProductRepositoryIsNull()
            {
                IProductRepository productRepository = null;
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                Action act = () => new Checkout(productRepository, productDiscountRuleRepository);
                act.ShouldThrow<ArgumentNullException>()
                    .And.Message.Should()
                    .Be("A null product repository has been passed in. The product repository cannot be null.\r\nParameter name: productRepository");
            }

            [Test]
            public void ShouldThrowAnArgumentNullExceptionWithAnAppropriateMessageWhenTheProductDiscountRuleRepositoryIsNull()
            {
                var productRepository = Substitute.For<IProductRepository>();
                IProductDiscountRuleRepository productDiscountRuleRepository = null;
                Action act = () => new Checkout(productRepository, productDiscountRuleRepository);
                act.ShouldThrow<ArgumentNullException>()
                    .And.Message.Should()
                    .Be("A null product discount rule repository has been passed in. The product discount rule repository cannot be null.\r\nParameter name: productDiscountRuleRepository");
            }
            
            [Test]
            public void ShouldConstructACheckoutWhenValidProductAndProductDiscountRuleRepositoriesArePassedIn()
            {
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository);
                checkout.Should().NotBeNull();
            }
        }
        
        /// <summary>
        /// This class contains all tests for the Scan method.
        /// </summary>
        [TestFixture]
        public class TheScanMethod
        {
            [Test]
            public void ShouldThrowAnArgumentNullExceptionWithAnAppropriateMessageWhenTheItemBeingScannedIsNull()
            {
                var checkout = new Checkout();
                Action act = () => checkout.Scan(null);
                act.ShouldThrow<ArgumentNullException>()
                    .And.Message.Should()
                    .Be("A null item has been passed in. The item cannot be null.\r\nParameter name: item");
            }
            
            [Test]
            public void ShouldThrowAnArgumentExceptionWithAnAppropriateMessageWhenTheItemIsAnEmptyString()
            {
                var checkout = new Checkout();
                Action act = () => checkout.Scan(String.Empty);
                act.ShouldThrow<ArgumentException>()
                    .And.Message.Should()
                    .Be("An empty item has been passed in. The item cannot be an empty string.\r\nParameter name: item");
            }

            [Test]
            public void ShouldThrowAProductWithSKUNotFoundExceptionWhenTheItemPassedInIsNotASKUAvailableInTheProductRepository()
            {
                var sku = "a"; 
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(X => { throw new ProductWithSKUNotFoundException(); });                
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout =new Checkout(productRepository, productDiscountRuleRepository);
                Action act = () => checkout.Scan(sku);
                act.ShouldThrow<ProductWithSKUNotFoundException>();
            }
        }
    }
}