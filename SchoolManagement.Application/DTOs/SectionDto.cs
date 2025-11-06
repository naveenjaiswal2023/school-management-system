using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ClassId { get; set; }
        public int Capacity { get; set; }
    }
}
