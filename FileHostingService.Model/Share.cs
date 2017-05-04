using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHostingService.Model
{
    public class Share
    {
        public User UserId { get; set; }
        public File FileId { get; set; }
    }
}
