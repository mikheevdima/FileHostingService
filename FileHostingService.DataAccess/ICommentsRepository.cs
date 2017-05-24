using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.Model;

namespace FileHostingService.DataAccess
{
    public interface ICommentsRepository
    {
        Comment Add(Comment comment);
        void Update(Guid id, string text);
        Comment Get(Guid id);
        void Delete(Guid id);
        IEnumerable<Comment> GetFileComments(Guid id);
        IEnumerable<Comment> GetUserComments(Guid id);
    }
}
