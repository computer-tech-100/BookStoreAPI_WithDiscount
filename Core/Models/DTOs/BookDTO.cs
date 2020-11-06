using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyAPI.Core.Models.DbEntities;

namespace MyAPI.Core.Models.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; } //primary key

        [MinLength(5)]
        public string Title {get; set; }

        [Required] //Author's name is required
        public string Author { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] //CategoryId is Foreign Key 

        public Category Category { get; set; }

        public long ISBN { get; set; }

        public decimal Price { get; set; }
        
        public string DateOfPublication { get; set; }

    } 
}