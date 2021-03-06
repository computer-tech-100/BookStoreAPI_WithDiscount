using System.ComponentModel.DataAnnotations.Schema;//ForeignKey
using MyAPI.Core.Models.DbEntities;

namespace MyAPI.Core.Models.DbEntities
{
    public class BookAsCartItem
    {
        //this model represents one cart item  (i.e one book) inside the cart
        public int Id { get; set; } //Id is both primary key and foreign key

        [ForeignKey("Id")]

        public Book Book { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get { return Quantity * Price; } }

    }
}