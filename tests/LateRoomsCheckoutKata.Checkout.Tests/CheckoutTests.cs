using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LateRoomsCheckoutKata.Checkout.Tests
{
    public class CheckoutTests
    {
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
        }
    }
}
