using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class ShoppingCart
    {
        //public ShoppingCart()
        //{
        //    OrderHeader = new OrderHeader();
        //    List<ShoppingCart> ListCart = new List<ShoppingCart>();
        //}

        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        //public OrderHeader OrderHeader { get; set; }
    }
}
