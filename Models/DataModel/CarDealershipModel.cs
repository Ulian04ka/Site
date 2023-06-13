using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.DataModel
{
    [Table("CarDealerships")]
    public class CarDealershipModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Контакты")]
        public string Phone { get; set; }
    }
}
