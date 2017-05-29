using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using FileHostingService.DataAccess;
using FileHostingService.DataAccess.SQL;
using FileHostingService.Model;

namespace FileHostingService.WebApi.Controllers
{
    public class CommentsController: ApiController
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;
        private readonly ICommentsRepository _commentsRepository;

        public CommentsController()
        {
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
            _commentsRepository = new CommentsRepository(ConnectionString, _usersRepository, _filesRepository);
        }

        /// <summary> create comment </summary>
        /// <param name="comment"> comment for creation </param>
        /// <returns> comment </returns>
        [HttpPost]
        public Comment CreateComment([FromBody]Comment comment)
        {
            return _commentsRepository.Add(comment);
        }

        /// <summary> delete comment </summary>
        /// <param name="id"> comment id </param>
        /// <returns> </returns>
        [HttpDelete]
        [Route("api/comments/{id}")]
        public void DeleteComment(Guid id)
        {
            _commentsRepository.Delete(id);
        }

        /// <summary> get comment </summary>
        /// <param name="id"> comment id </param>
        /// <returns> comment </returns>
        [HttpGet]
        [Route("api/comments/{id}")]
        public Comment GetComment(Guid id)
        {
            return _commentsRepository.Get(id);
        }

        /// <summary> update comment </summary>
        /// <param name="id"> comment id </param>
        /// <returns> comment </returns>
        [HttpPut]
        [Route("api/comments/{id}")]
        public void UpdateComment(Guid id, [FromBody]string text)
        {
            _commentsRepository.Update(id, text);
        }
    }
}