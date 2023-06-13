using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.DataModel
{
    [Table("BasketProductLinks")]
    public class BasketProductLinkModel
    {
        public ProductModel Product { get; set; }
        public int ProductId { get; set; }
        public BasketModel Basket { get; set; }
        public int BasketId { get; set; }
        public int CountCopies { get; set; } = 1;

        public BasketProductLinkModel() { }

        public BasketProductLinkModel(
            ProductModel productModel,
            int countCopies
        )
        {
            this.Product = productModel;
            this.ProductId = productModel.Id;
            this.CountCopies = countCopies;
        }
    }
}
