using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceHouse.Models
{
    public class Tags
    {
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }
    }
}
