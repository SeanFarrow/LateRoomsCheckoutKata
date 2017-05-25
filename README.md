# LateRoomsCheckoutKata by SEan Farrow

This repository contains my solution to the [LateRooms Checkout Kata](https://github.com/LateRoomsGroup/interview-katas/blob/master/checkout.md) 

There is one public interface to the checkout, which is the same as defined in the readme of the repository referenced above.

The discounts have been taken directly from the table in that repository.

To calculate discounts I have created an IProductDiscountRule interface, stored in an IProductDiscountRepository, enabling the retrieval of rules from some form of data store. In this case all discount rules are mocked, but I could imagine a percentage discount rule being implemented in a real-world scenario.

# building the code.

Once the code has been cloned, building the code is simple, just run build.cmd. This download [Cake](http://www.cakebuild.net/), which in turn installs all the required NuGet packages, builds the code and runs the tests.
