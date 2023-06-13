using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopKnitting.Models.Auxiliary;
using ShopKnitting.Models.DataModel;

namespace ShopKnitting.Models
{
    [Table("Products")]
    public class ProductModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        
        [Display(Name = "Марка")]
        public BrandModel Brand { get; set; }
        public int BrandId { get; set; }

        [Display(Name = "Модель")]
        public string Model { get; set; }

        [Display(Name = "Год выпуска")]
        public short Year { get; set; }

        [Display(Name = "Тип кузова")]
        public CarBodyEnum CarBody { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public double Cost { get; set; }

        [Display(Name = "Количесво в салоне")]
        public int CountInStack { get; set; }

        [Display(Name = "Фото автомобиля")]
        public ImageModel Images { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
