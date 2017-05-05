using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHostingService.Model
{
    public class Comment
    {
        public Guid Id { get; set; }
        public File FileId { get; set; }
        public User UserId { get; set; }
        public string Text { get; set; }
    }
}
