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
                IDictionary<Product, int> till = null;
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                //Here we use reflection to check a private field. This isn't strictly unit testing, but given we don't need access to the till from the outside, there is no property available.
                checkout.GetType().GetField("_till", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(checkout).Should().NotBeNull();
            }
            
            [Test]
            public void ShouldAssignThePassedInTillToTheTillFieldWhenTheTillIsNotNull()
            {
                var productRepository = Substitute.For<IProductRepository>();
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var till = new Dictionary<Product, int>();
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
                var expectedProductAndQuantity = new KeyValuePair<Product, int>(expectedProduct, 1);
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(expectedProduct);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var till = new Dictionary<Product, int>();
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
                var expectedProductAndQuantity = new KeyValuePair<Product, int>(expectedProduct, 2);
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(expectedProduct);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var till = new Dictionary<Product, int> { { expectedProduct, 1}}; 
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
            public void ShouldReturnTheTotalPriceOfASingleItemWhenOnlyOneItemHasBeenScannedThroughTheTill(string sku, int unitPrice)
            {
                var product = new Product(sku, unitPrice);
                var till = new Dictionary<Product, int> { { product, 1 } };
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(product);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                IProductDiscountRule rule = null; //ensure we don't have a discount.
                productDiscountRuleRepository.GetDiscountRuleForSKU(sku).Returns(rule);
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(Convert.ToInt32(unitPrice));
            }
            
            [Test]
            [TestCase("c", 3, 20)]
            [TestCase("d", 5, 15)]
            public void ShouldReturnTheTotalPriceWhenASingleProductWithNoDiscountIsScannedMultipleTimes(string sku, int numberOfScannedProducts, int unitPrice)
            {
                var product = new Product(sku, unitPrice);
                var till = new Dictionary<Product, int> { { product, numberOfScannedProducts } };
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(product);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                IProductDiscountRule rule = null; //ensure we don't have a discount.
                productDiscountRuleRepository.GetDiscountRuleForSKU(sku).Returns(rule);
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(unitPrice*numberOfScannedProducts);
            }
            
            [Test]
            [TestCase("a", 3, 50, 40)]
            [TestCase("b", 2, 30, 50)]
            public void ShouldReturnTheTotalPriceWhenASingleProductIsScannedMultipleTimesAndTheNumberOfTimesTheProductWasScannedEquatesExactlyToTheNumberOfItemsRequiredForADiscount(string sku, int quantityForDiscount, int unitPrice, int percentageReduction)
            {
                var product = new Product(sku, unitPrice);
                var till = new Dictionary<Product, int> { { product, quantityForDiscount } };
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(product);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var productDiscountRule = Substitute.For<IProductDiscountRule>();
                productDiscountRule.QuantityToDiscount.Returns(quantityForDiscount);
                int expectedPrice = unitPrice * quantityForDiscount- (unitPrice / 100 * percentageReduction);
                productDiscountRule.CalculateDiscount(quantityForDiscount, unitPrice).Returns(expectedPrice);
                productDiscountRuleRepository.GetDiscountRuleForSKU(sku).Returns(productDiscountRule);
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(Convert.ToInt32(expectedPrice));
            }

            [Test]
            [TestCase("a", 8, 3, 50, 40)]
            [TestCase("b", 7, 2, 30, 50)]
            public void ShouldReturnTheTotalPriceWhenASingleProductIsScannedMultipleTimesAndTheQuantityOfTheScannedProductHasAtLeastSomePortionThatCanBeDiscounted(string sku, int numberOfProductsScanned, int quantityForDiscount, int unitPrice, int percentageReduction)
            {
                var product = new Product(sku, unitPrice);
                var till = new Dictionary<Product, int> { { product, numberOfProductsScanned } };
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(sku).Returns(product);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var productDiscountRule = Substitute.For<IProductDiscountRule>();
                productDiscountRuleRepository.GetDiscountRuleForSKU(sku).Returns(productDiscountRule);
                productDiscountRule.QuantityToDiscount.Returns(quantityForDiscount);
                int discountableItemsMultiplier = numberOfProductsScanned / quantityForDiscount;
                //The total price for a single discountable offer, i.e 3a for 130.
                int priceForDiscountableOffer = unitPrice * quantityForDiscount- (unitPrice * percentageReduction / 100);
                int numberOfUndiscountableItems = numberOfProductsScanned % quantityForDiscount;
                int totalPriceForDiscountableProducts = priceForDiscountableOffer * discountableItemsMultiplier;
                int expectedPrice = (numberOfUndiscountableItems * unitPrice) + totalPriceForDiscountableProducts;
                productDiscountRule.CalculateDiscount(numberOfProductsScanned, unitPrice).Returns(totalPriceForDiscountableProducts);
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(Convert.ToInt32(expectedPrice));
            }
            
            [Test]
            public void ShouldReturnTheTotalPriceWhenMultipleProductsAreScannedOnceEach()
            {
                //Construct a till with multiple products.
                var productA = new Product("A", 50);
                var productB = new Product("b", 30);
                var till = new Dictionary<Product, int>()
                               {
                                   { productA, 1},
                                   { productB, 1}
                               };

                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU("a").Returns(productA);
                productRepository.FindProductBySKU("b").Returns(productB);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                IProductDiscountRule rule = null;
                productDiscountRuleRepository.GetDiscountRuleForSKU(productA.SKU).Returns(rule);
                productDiscountRuleRepository.GetDiscountRuleForSKU(productB.SKU).Returns(rule);
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(80);
            }
            
            [Test]
            [TestCase("d", 5, 15)]
            public void ShouldReturnTheTotalPriceWhenMultipleProductsAreScannedMultipleTimesButNoProductsHaveADiscount(string sku, int numberOfProductsToScan, int unitPrice)
{
    //Construct a till with multiple products.
    var productC = new Product("c", 20);
    var productD = new Product("d", 15);
    var till = new Dictionary<Product, int>()
                   {
                       { productC, 3},
                       { productD, 8}
                   };

    var productRepository = Substitute.For<IProductRepository>();
    productRepository.FindProductBySKU("c").Returns(productC);
    productRepository.FindProductBySKU("d").Returns(productD);
    var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
    IProductDiscountRule rule = null;
    productDiscountRuleRepository.GetDiscountRuleForSKU(productC.SKU).Returns(rule);
    productDiscountRuleRepository.GetDiscountRuleForSKU(productD.SKU).Returns(rule);
    var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
    checkout.GetTotalPrice().Should().Be(180);
            }

            [Test]
            public void ShouldReturnTheTotalPriceWhenMultipleProductsAreScannedMultipleTimesAndTheNumberOfTimesTheProductsAreScannedEquatesExactlyToTheNumberOfItemsRequiredForADiscountForTheParticularProduct()
            {
                //Construct a till with multiple products.
                var productA = new Product("A", 50);
                var productB = new Product("b", 30);
                var till = new Dictionary<Product, int>()
                               {
                                   { new Product("A", 50), 3 },
                                   { new Product("b", 30), 2}
                               };
                
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU("a").Returns(productA);
                productRepository.FindProductBySKU("b").Returns(productB);
                var discountRuleProductA = Substitute.For<IProductDiscountRule>();
                discountRuleProductA.QuantityToDiscount.Returns(3);
                discountRuleProductA.CalculateDiscount(3, productA.UnitPrice).Returns(130);
                var discountRuleProductB = Substitute.For<IProductDiscountRule>();
                discountRuleProductB.QuantityToDiscount.Returns(2);
                discountRuleProductB.CalculateDiscount(2, productB.UnitPrice).Returns(45);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                productDiscountRuleRepository.GetDiscountRuleForSKU(productA.SKU).Returns(discountRuleProductA);
                productDiscountRuleRepository.GetDiscountRuleForSKU(productB.SKU).Returns(discountRuleProductB);
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(175);
            }
            
            [Test]
            public void ShouldReturnTheTotalPriceWhenMultipleProductsAreScannedMultipleTimesAndTheQuantityOfAllProductsHaveAPortionThatIsDiscountable()
            {
                //Construct a till with multiple products.
                var productA = new Product("A", 50);
                var productB = new Product("b", 30);
                const int scannedQuantity = 5;
                var till = new Dictionary<Product, int>() { { productA, scannedQuantity}, { productB, scannedQuantity} };
                var productADiscountPercentage = 40;
                var productBDiscountPercentage = 50;
                var productADiscountQuantity = 3;
                var productBDiscountQuantity = 2;
                var productRepository = Substitute.For<IProductRepository>();
                productRepository.FindProductBySKU(productA.SKU).Returns(productA);
                productRepository.FindProductBySKU(productB.SKU).Returns(productB);
                var productDiscountRuleRepository = Substitute.For<IProductDiscountRuleRepository>();
                var productADiscountRule = Substitute.For<IProductDiscountRule>();
                productADiscountRule.QuantityToDiscount.Returns(productADiscountQuantity);
                productADiscountRule.CalculateDiscount(scannedQuantity, productA.UnitPrice).Returns(130);
                productDiscountRuleRepository.GetDiscountRuleForSKU(productA.SKU).Returns(productADiscountRule);
                var productBDiscountRule = Substitute.For<IProductDiscountRule>();
                productBDiscountRule.QuantityToDiscount.Returns(productBDiscountQuantity);
                productBDiscountRule.CalculateDiscount(scannedQuantity, productB.UnitPrice).Returns(50);
                productDiscountRuleRepository.GetDiscountRuleForSKU(productB.SKU).Returns(productBDiscountRule);
                var expectedTotalPrice = 350;
                var checkout = new Checkout(productRepository, productDiscountRuleRepository, till);
                checkout.GetTotalPrice().Should().Be(expectedTotalPrice);
            }
        }
    }
}