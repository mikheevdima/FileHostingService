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
    public class FilesController : ApiController
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;

        public FilesController()
        {
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
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
        /// <returns></returns>
        [HttpDelete]
        public void DeleteFile(Guid id)
        {
            _filesRepository.Delete(id);
        }

        /// <summary> get file info </summary>
        /// <param name="id"> file id </param>
        /// <returns> file </returns>
        [HttpGet]
        public File GetFileInfo(Guid id)
        {
            return _filesRepository.GetInfo(id);
        }


    }
}