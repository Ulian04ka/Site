using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.DataModel
{
    [Table("Baskets")]
    public class BasketModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public IdentityUser<Guid> User { get; set; }
        public Guid UserId { get; set; }
    }
}
