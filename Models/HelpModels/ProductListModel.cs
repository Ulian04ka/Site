using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.HelpModels
{
    public class ProductListModel
    {
        public readonly ProductModel productModel = new ProductModel();
        public IEnumerable<ProductModel> productList { get; set; }
    }
}
