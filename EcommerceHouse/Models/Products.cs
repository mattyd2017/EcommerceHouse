using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceHouse.Models
{
    public class Products
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public string Image { get; set; }

        public string Type { get; set; }

        [Display(Name="Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }


        [Display(Name = "Tags Type")]
        public int TagsId { get; set; }

        [ForeignKey("TagsId")]
        public virtual Tags Tags { get; set; }
    }
}
