using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.Auxiliary
{
    public enum OrderStatus
    {
        [Display(Name = "Оформлен")]
        Created,
        [Display(Name = "Получен")]
        Received
    }
}
