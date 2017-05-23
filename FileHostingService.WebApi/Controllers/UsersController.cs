using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using FileHostingService.DataAccess;
using FileHostingService.DataAccess.SQL;
using FileHostingService.Model;

namespace FileHostingService.WebApi.Controllers
{
    public class UsersController: ApiController
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;

        public UsersController()
        {
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
        }

        [HttpPost]
        public User CreateUser([FromBody]User user)
        {
            return _usersRepository.Add(user);
        }
    }
}