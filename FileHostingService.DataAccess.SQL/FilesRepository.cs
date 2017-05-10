using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.Model;

namespace FileHostingService.DataAccess.SQL
{
    public class FilesRepository: IFilesRepository
    {
        private readonly string _connectionString;
        private readonly IUsersRepository _usersRepository;

        public FilesRepository(IUsersRepository usersRepository, string connectionString)
        {
            _usersRepository = usersRepository;
            _connectionString = connectionString;
        }

        public File Add(File file)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into files (id, name, userid, adddate) values (@id, @name, @userid, @adddate)";
                    var fileId = Guid.NewGuid();
                    var date = DateTime.Now;
                    command.Parameters.AddWithValue("@id", fileId);
                    command.Parameters.AddWithValue("@name", file.Name);
                    command.Parameters.AddWithValue("@userid", file.UserId.Id);
                    command.Parameters.AddWithValue("@adddate", date);
                    command.ExecuteNonQuery();
                    file.Id = fileId;
                    return file;
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
                    command.CommandText = "delete from files where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateContent(Guid id, byte[] content)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update files set content = @content where id = @id";
                    command.Parameters.AddWithValue("@content", content);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public File GetInfo(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, name, userid, adddate from files where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new File
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                UserId = _usersRepository.Get(reader.GetGuid(reader.GetOrdinal("userid"))),
                                AddDate = reader.GetDateTime(reader.GetOrdinal("adddate"))
                            };
                        }
                        throw new ArgumentException("File not found");
                    }
                }
            }
        }

        public IEnumerable<File> GetUserFiles(Guid id)
        {
            var result = new List<File>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id from files where userid = @userid";
                    command.Parameters.AddWithValue("@userid", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(GetInfo(reader.GetGuid(reader.GetOrdinal("id"))));
                        }
                        return result;
                    }
                }
            }
        }

        public byte[] GetContent(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select content from files where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            return reader.GetSqlBinary(reader.GetOrdinal("content")).Value;
                        throw new ArgumentException($"File {id} not found");
                    }
                }
            }
        }
    }
}
