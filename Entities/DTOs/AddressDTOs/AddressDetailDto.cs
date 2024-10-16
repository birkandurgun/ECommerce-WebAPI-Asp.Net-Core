using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.AddressDTOs
{
    public class AddressDetailDto
    {
        public int Id { get; set; }
        public string AddressLine { get; set; } 
        public string City { get; set; }
        public string State { get; set; } 
        public string Country { get; set; }
    }
}
