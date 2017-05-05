using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.Model;

namespace FileHostingService.DataAccess
{
    public interface IUsersRepository
    {
        User Add(User user);
        void Delete(Guid id);
        User Get(Guid id);
    }
}
