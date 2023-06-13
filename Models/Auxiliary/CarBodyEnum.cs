using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.Auxiliary
{
    public enum CarBodyEnum
    {
        [Display(Name = "- Не выбрано -")]
        defaultCarBody,
        [Display(Name = "Седан")]
        Sedan,
        [Display(Name = "Универсал")]
        StationWagon,
        [Display(Name = "Хэтчбэк")]
        Hatchback,
        [Display(Name = "Купэ")]
        Coupe,
        [Display(Name = "Лимузин")]
        Limousine,
        [Display(Name = "Внедорожник")]
        OffRoadVehicle,
        [Display(Name = "Пикап")]
        Pickup,
        [Display(Name = "Фургон")]
        Van,
        [Display(Name = "Минивен")]
        Minivan,
        [Display(Name = "Электромобиль")]
        ElectricCar,
        [Display(Name = "Гибридный автомобиль")]
        HybridVehicle
    }
}
