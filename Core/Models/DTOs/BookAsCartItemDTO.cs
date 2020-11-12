using System.ComponentModel.DataAnnotations.Schema;//ForeignKey
using MyAPI.Core.Models.DbEntities;

namespace MyAPI.Core.Models.DTOs
{
    //this represents one book inside the cart
    public class BookAsCartItemDTO
    {
        public int Id { get; set; } //Id is both primary key and foreign key

        [ForeignKey("Id")]

        public Book Book { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get { return Quantity * Price; } }

    }
}