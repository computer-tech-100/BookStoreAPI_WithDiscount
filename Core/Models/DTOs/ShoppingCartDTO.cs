using System.Collections.Generic;

namespace MyAPI.Core.Models.DTOs
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }

        public List<FavoritesDTO> AllBooksInsideCart{ get; set; }

        public decimal TotalBill { get; set; }

        public decimal Discount { get; set; }//if total amount is above 100 dollors then we give 10% discount

    }
}