using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Products")]
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumberName { get; set; }

        [StringLength(100)]
        public string? ShownName { get; set; }

        [StringLength(100)]
        public string? ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public TypesOfProduct TypeOfProduct { get; set; }

        public void SetDbName()
        {
            switch (TypeOfProduct)
            {
                case TypesOfProduct.Crumble:
                    ShownName = "Кр-" + Convert.ToString(NumberName);
                    break;
                case TypesOfProduct.Marble:
                    ShownName = "Км-" + Convert.ToString(NumberName);
                    break;
            }
        }
    }

    /// <summary>
    ///  Enum для работы с типом камня продукат
    /// </summary>
    public enum TypesOfProduct
    {
        /// <value> Крошка </value> 
        Crumble,
        /// <value> Камень </value>
        Marble,
    }
}
