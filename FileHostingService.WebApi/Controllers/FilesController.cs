using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FileHostingService.DataAccess;
using FileHostingService.DataAccess.SQL;
using FileHostingService.Model;

namespace FileHostingService.WebApi.Controllers
{
    public class FilesController : ApiController
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;
        private readonly ICommentsRepository _commentsRepository;

        public FilesController()
        {
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
            _commentsRepository = new CommentsRepository(ConnectionString, _usersRepository, _filesRepository);
        }

        /// <summary> create file </summary>
        /// <param name="file"> file for creation </param>
        /// <returns> file </returns>
        [HttpPost]
        public File CreateFile(File file)
        {
            return _filesRepository.Add(file);
        }

        /// <summary> delete file </summary>
        /// <param name="id"> file id </param>
        /// <returns> </returns>
        [HttpDelete]
        [Route("api/files/{id}")]
        public void DeleteFile(Guid id)
        {
            _filesRepository.Delete(id);
        }

        /// <summary> get file info </summary>
        /// <param name="id"> file id </param>
        /// <returns> file </returns>
        [HttpGet]
        [Route("api/files/{id}")]
        public File GetFileInfo(Guid id)
        {
            return _filesRepository.GetInfo(id);
        }

        /// <summary> update file content </summary>
        /// <param name="id"> file id </param>
        /// <returns> </returns>
        [HttpPut]
        [Route("api/files/{id}/content")]
        public async Task UpdateFileContent(Guid id)
        {
            var bytes = await Request.Content.ReadAsByteArrayAsync();
            _filesRepository.UpdateContent(id, bytes);
        }

        /// <summary> get file content </summary>
        /// <param name="id"> file id </param>
        /// <returns> file content(binary) </returns>
        [HttpGet]
        [Route("api/files/{id}/content")]
        public byte[] GetFileContent(Guid id)
        {
            return _filesRepository.GetContent(id);
        }

        /// <summary> get file comments </summary>
        /// <param name="id"> file id </param>
        /// <returns> list of comments </returns>
        [HttpGet]
        [Route("api/files/{id}/comments")]
        public IEnumerable<Comment> GetFileComments(Guid id)
        {
            return _commentsRepository.GetFileComments(id);
        }

        /// <summary> give access to file </summary>
        /// <param name="fileid"> file id </param>
        /// /// <param name="userid"> user id </param>
        /// <returns> </returns>
        [HttpPost]
        [Route("api/files/{fileid}/shares/{userid}")]
        public void GiveAccessToFile(Guid fileid, Guid userid)
        {
            _filesRepository.GiveAccessToFile(userid, fileid);
        }

        /// <summary> delete access to file </summary>
        /// <param name="fileid"> file id </param>
        /// /// <param name="userid"> user id </param>
        /// <returns> </returns>
        [HttpDelete]
        [Route("api/files/{fileId}/shares/{userid}")]
        public void DeleteAccessToFile(Guid fileid, Guid userid)
        {
            _filesRepository.DeleteAccessToFile(userid, fileid);
        }
    }
}