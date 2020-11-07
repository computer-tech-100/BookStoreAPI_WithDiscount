using System.Collections.Generic;

namespace MyAPI.Core.Models.DbEntities
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public List<Favorites> AllBooksToBeBought { get; set; }

        public decimal TotalBill { get; set; }

        public decimal Discount { get; set; }//if total amount is above 100 dollors then we give 10% discount

    }
}