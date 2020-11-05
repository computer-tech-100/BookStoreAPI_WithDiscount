using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Core.Models.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; } //primary key

        [MinLength(5)]
        public string Title {get; set; }
        [Required] //Author's name is required
        [Column("Author")] //The property name (i.e NameOfAuthor) will be changed to Author in database 
        public string NameOfAuthor { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] //CategoryId is Foreign Key 

        public CategoryDTO Category { get; set; }

        public long ISBN { get; set; }

        public int Price { get; set; }

        [DataType(DataType.Date)]
        //We use DisplayFormat attribute for date format
        //The ApplyFormatInEditMode means the specified formatting should be provided while editing 
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]  
        public DateTime DateOfPublication { get; set; }

    } 
}