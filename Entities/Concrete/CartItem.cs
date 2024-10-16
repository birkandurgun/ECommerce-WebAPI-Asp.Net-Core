using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CartItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } 
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } 
        public decimal UnitPrice { get; set; }
    }

}
