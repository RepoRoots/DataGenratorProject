using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbGenratorWithBogus.DbModels
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
