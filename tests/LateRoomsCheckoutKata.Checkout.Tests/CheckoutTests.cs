using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            public void ShouldSelfAssignTheTillFieldToANewDictionaryWhenThePassedInTillIsNull()
            {
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                IDictionary<Product, uint> till = null;
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                //Here we use reflection to check a private field. This isn't strictly unit testing, but given we don't need access to the till from the outside, there is no property available.
                checkout.GetType().GetField("_till", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(checkout).Should().NotBeNull();
            }
            
            [Test]
            public void ShouldAssignThePassedInTillToTheTillFieldWhenTheTillIsNotNull()
            {
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var till = new Dictionary<Product, uint>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                //Here we use reflection to check a private field. This isn't strictly unit testing, but given we don't need access to the till from the outside, there is no property available.
                checkout.GetType()
                    .GetField("_till", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(checkout)
                    .Should()
                    .BeSameAs(till);
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
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository);
                Action act = () => checkout.Scan(null);
                act.ShouldThrow<ArgumentNullException>()
                    .And.Message.Should()
                    .Be("A null item has been passed in. The item cannot be null.\r\nParameter name: item");
            }
            
            [Test]
            public void ShouldThrowAnArgumentExceptionWithAnAppropriateMessageWhenTheItemBeingScannedIsAnEmptyString()
            {
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository);
                Action act = () => checkout.Scan(String.Empty);
                act.ShouldThrow<ArgumentException>()
                    .And.Message.Should()
                    .Be("An empty item has been passed in. The item cannot be an empty string.\r\nParameter name: item");
            }

            [Test]
            public void ShouldThrowAProductWithSKUNotFoundExceptionWithAnAppropriateMessageWhenTheItemPassedInIsNotASKUAvailableInTheProductRepository()
            {
                var sku = "a"; 
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(X => { throw new ProductWithSKUNotFoundException(sku); });                
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout =new Checkout(productRepository, productDiscountRuleRepository);
                Action act = () => checkout.Scan(sku);
                act.ShouldThrow<ProductWithSKUNotFoundException>().And.Message.Should().Be("The product with stock keeping unit 'a' could not be found in the product repository.");
            }

            [Test]
            public void ShouldAddTheProductToTheTillWithAQuantityOf1WhenTheProductIsAvailableInTheSupermarketAndHasNotBeenScannedPreviously()
            {
                var sku = "a";
                var unitPrice = 50;
                var expectedProduct = new Product(sku, unitPrice);
                var expectedProductAndQuantity = new KeyValuePair<Product, uint>(expectedProduct, 1);
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(expectedProduct);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var till = new Dictionary<Product, uint>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.Scan(sku);
                till.Should().Contain(expectedProductAndQuantity);
            }
            
            [Test]
            public void ShouldUpdateTheQuantityOfTheProductInTheTillWhenTheProductIsAvailableInTheSupermarketAndHasPreviouslyBeenScanned()
            {
                var sku = "a";
                var unitPrice = 50;
                var expectedProduct = new Product(sku, unitPrice);
                var expectedProductAndQuantity = new KeyValuePair<Product, uint>(expectedProduct, 2);
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(expectedProduct);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var till = new Dictionary<Product, uint> { { expectedProduct, 1}}; 
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.Scan(sku);
                till.Should().Contain(expectedProductAndQuantity);
            }
        }
        
        /// <summary>
        /// This class contains all tests for the GetTotalPrice method.
        /// </summary>
        [TestFixture]
        public class TheGetTotalPriceMethod
        {
            [Test] 
            public void ShouldReturn0WhenNothingHasBeenScanned()
            {
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository);
                checkout.GetTotalPrice().Should().Be(0);
            }

            [Test]
            [TestCase("a", 50)]
            [TestCase("b", 30)]
            [TestCase("c", 20)]
            [TestCase("d", 15)]
            public void ShouldReturnThePriceOfASingleItemWhenOnlyOneItemHasBeenScannedThroughTheTill(string sku, int unitPrice)
            {
                var product = new Product(sku, unitPrice);
                var till = new Dictionary<Product, uint> { { product, 1 } };
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(product);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(Convert.ToInt32(unitPrice));
            }
        }
    }
}