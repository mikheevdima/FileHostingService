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
        private readonly ICommentsRepository _commentsRepository;

        public UsersController()
        {
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
            _commentsRepository = new CommentsRepository(ConnectionString, _usersRepository, _filesRepository);
        }

        /// <summary> create user </summary>
        /// <param name="user"> user for creation </param>
        /// <returns> user </returns>
        [HttpPost]
        public User CreateUser([FromBody]User user)
        {
            return _usersRepository.Add(user);
        }

        /// <summary> delete user </summary>
        /// <param name="id"> user id </param>
        /// <returns>  </returns>
        [HttpDelete]
        public void DeleteUser(Guid id)
        {
            _usersRepository.Delete(id);
        }

        /// <summary> get user </summary>
        /// <param name="id"> user id </param>
        /// <returns> user </returns>
        [HttpGet]
        public User GetUser(Guid id)
        {
            return _usersRepository.Get(id);
        }

        /// <summary> get user files </summary>
        /// <param name="id"> user id </param>
        /// <returns> list of files </returns>
        [Route("api/users/{id}/files")]
        [HttpGet]
        public IEnumerable<File> GetUserFiles(Guid id)
        {
            return _filesRepository.GetUserFiles(id);
        }

        /// <summary> get user comments </summary>
        /// <param name="id"> user id </param>
        /// <returns> list of comments </returns>
        [Route("api/users/{id}/comments")]
        [HttpGet]
        public IEnumerable<Comment> GetUserComments(Guid id)
        {
            return _commentsRepository.GetUserComments(id);
        }
    }
}