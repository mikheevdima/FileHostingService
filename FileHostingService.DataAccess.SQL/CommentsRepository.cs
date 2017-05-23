using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.Model;

namespace FileHostingService.DataAccess.SQL
{
    public class CommentsRepository: ICommentsRepository
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
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into comments (id, fileid, userid, text, adddate) values (@id, @fileid, @userid, @text, @adddate)";
                    var commentId = Guid.NewGuid();
                    var date = DateTime.Now;
                    command.Parameters.AddWithValue("@id", commentId);
                    command.Parameters.AddWithValue("@fileid", comment.FileId.Id);
                    command.Parameters.AddWithValue("@userid", comment.UserId.Id);
                    command.Parameters.AddWithValue("@text", comment.Text);
                    command.Parameters.AddWithValue("@adddate", date);
                    command.ExecuteNonQuery();
                    comment.Id = commentId;
                    comment.AddDate = date;
                    return comment;
                }
            }
        }

        public Comment Update(Guid id, string text)
        {
            throw new NotImplementedException();
        }

        public Comment Get(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, fileid, userid, text, adddate from comments where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new Comment
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                UserId = _usersRepository.Get(reader.GetGuid(reader.GetOrdinal("userid"))),
                                FileId = _filesRepository.GetInfo(reader.GetGuid(reader.GetOrdinal("fileid"))),
                                Text = reader.GetString(reader.GetOrdinal("text")),
                                AddDate = reader.GetDateTime(reader.GetOrdinal("adddate"))
                            };
                        }
                        throw new ArgumentException("Comment not found");
                    }
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from comments where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Comment> GetFileComments(Guid id)
        {
            var result = new List<Comment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id from comments where fileid = @fileid";
                    command.Parameters.AddWithValue("@fileid", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(Get(reader.GetGuid(reader.GetOrdinal("id"))));
                        }
                        return result;
                    }
                }
            }
        }

        public IEnumerable<Comment> GetUserComment(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
