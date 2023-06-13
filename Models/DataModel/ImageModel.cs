using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.DataModel
{
    [Table("Images")]
    public class ImageModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Path { get; set; }
    }
}
