using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopKnitting.Models.Auxiliary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.DataModel
{
    [Table("Orders")]
    public class OrderModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; } 

        [Display(Name = "Общая стоимость")]
        public double TotalPrice { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime IssueDate { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Display(Name = "Автосалон")]
        public CarDealershipModel CarDealership { get; set; }

        public OrderStatus Status { get; set; }
        public IdentityUser<Guid> User { get; set; }
    }
}