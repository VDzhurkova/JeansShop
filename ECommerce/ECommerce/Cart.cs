using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace ECommerce.Models
{
    public class Cart
    {
        private List<CartLine> allLines = new List<CartLine>();

        public void AddLine(Product product, int quantity) {
           
            CartLine line = allLines.Where(p => p.Product.Id == product.Id).FirstOrDefault();
            if (line == null)
            {
                allLines.Add(new Models.CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product) {
            CartLine line = allLines.Where(p => p.Product.Id == product.Id).FirstOrDefault();
            if (line.Quantity > 1)
            {
                line.Quantity--;
            }
            else
            {
                allLines.RemoveAll(lines => lines.Product.Id == product.Id);
            }
        }

        public decimal totalValue() {
            return allLines.Sum(line => (decimal)line.Product.Price * line.Quantity);
        }

        public void Clear() {
            allLines.Clear();
        }
         public IEnumerable<CartLine> Lines {
            get { return allLines; }
        }
    }

    public class CartLine {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}