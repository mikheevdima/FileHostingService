using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHostingService.Model
{
    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User UserId { get; set; }
        public DateTime AddDate { get; set; }
    }
}
