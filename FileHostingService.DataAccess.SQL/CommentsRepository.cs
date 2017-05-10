using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.Model;

namespace FileHostingService.DataAccess.SQL
{
    public class CommentsRepository:ICommentsRepository
    {
        private readonly string _connectionString;
        private readonly IUsersRepository _usersRepository;
        private readonly IFilesRepository _filesRepository;

        public CommentsRepository(string connectionString, IUsersRepository usersRepository, IFilesRepository filesRepository)
        {
            _connectionString = connectionString;
            _usersRepository = usersRepository;
            _filesRepository = filesRepository;
        }

        public Comment Add(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Comment Update(Guid id)
        {
            throw new NotImplementedException();
        }

        public Comment Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetFileComments(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetUserComment(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
