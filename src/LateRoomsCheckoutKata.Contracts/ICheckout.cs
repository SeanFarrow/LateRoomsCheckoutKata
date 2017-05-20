namespace LateRoomsCheckoutKata.Contracts
{
    /// <summary>
    /// The interface for a supermarket checkout.
    /// </summary>
    public interface ICheckout
    {
        /// <summary>
        /// Scan an item and add it to the list of products we are purchasing.
        /// </summary>
        /// <param name="item">The SKU of the item we want to purchase.</param>
        void Scan(string item);
        
        /// <summary>
        /// Retrieve the current running total.
        /// </summary>
        /// <returns>The checkout's currently calculated total.</returns>
        int GetTotalPrice();
    }
}