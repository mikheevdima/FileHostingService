using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHostingService.DataAccess;
using FileHostingService.DataAccess.SQL;
using FileHostingService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileHosting.DataAccess.SQL.Tests
{
    [TestClass]
    public class CommentsRepositoryTests
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;
        private readonly ICommentsRepository _commentsRepository;
        
        private User TestUser { get; set; }
        private File TestFile { get; set; }

        public CommentsRepositoryTests()
        {
            _commentsRepository = new CommentsRepository(ConnectionString, _usersRepository, _filesRepository);
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
        }

        [TestInitialize]
        public void Init()
        {
            var user = new User
            {
                Name = "name",
                Surname = "surname",
                Email = "test@gmail.com"
            };

            TestUser = _usersRepository.Add(user);

            var file = new File
            {
                Name = "testFile",
                UserId = TestUser,
                AddDate = DateTime.Now
            };

            TestFile = _filesRepository.Add(file);
        }

        [TestCleanup]
        public void Clean()
        {
            /*
            if (TestUser != null)
            {
                foreach (var file in _filesRepository.GetUserFiles(TestUser.Id))
                {
                    foreach (var comment in _commentsRepository.GetFileComments(file.Id))
                        _commentsRepository.Delete(comment.Id);

                    _filesRepository.Delete(file.Id);
                }
                _usersRepository.Delete(TestUser.Id);
            }
            */
        }

        [TestMethod]
        public void ShouldCreateAndGetComment()
        {
            var comment = new Comment
            {
                FileId = TestFile,
                UserId = TestUser,
                Text = "Test comment",
                AddDate = DateTime.Now
            };

            var newComment = _commentsRepository.Add(comment);
            var result = _commentsRepository.Get(newComment.Id);

            Assert.AreEqual(comment.FileId.Id, result.FileId.Id);
            Assert.AreEqual(comment.UserId.Id, result.UserId.Id);
            Assert.AreEqual(comment.Text, result.Text);
            Assert.AreEqual(comment.AddDate.ToString(), result.AddDate.ToString());
        }


    }
}
