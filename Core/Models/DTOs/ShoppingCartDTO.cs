using System.Collections.Generic;

namespace MyAPI.Core.Models.DTOs
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; } = 1;

        public List<BookAsCartItemDTO> AllBooksInsideCart { get; set; }

         public decimal Total { get; set; }

        public decimal Discount { get; set; }//if total amount is above 100 dollors then we give 10% discount

        public decimal GrandTotal { get; set; }//Grandtotal when total amount is above threshold

    }
}