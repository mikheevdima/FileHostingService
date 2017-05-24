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
            _filesRepository = new FilesRepository(_usersRepository, ConnectionString);
            _commentsRepository = new CommentsRepository(ConnectionString, _usersRepository, _filesRepository);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldDeleteComment()
        {
            var comment = new Comment
            {
                FileId = TestFile,
                UserId = TestUser,
                Text = "Test comment",
                AddDate = DateTime.Now
            };
            var newComment = _commentsRepository.Add(comment);
            _commentsRepository.Delete(newComment.Id);

            var result = _commentsRepository.Get(newComment.Id);
        }

        [TestMethod]
        public void ShouldUpdateComment()
        {
            var comment = new Comment
            {
                FileId = TestFile,
                UserId = TestUser,
                Text = "Test comment",
                AddDate = DateTime.Now
            };
            var newComment = _commentsRepository.Add(comment);
            var newText = "New comment";

            _commentsRepository.Update(comment.Id, newText);
            var result = _commentsRepository.Get(comment.Id);

            Assert.IsTrue(newText.SequenceEqual(result.Text));
        }

        [TestMethod]
        public void ShouldGetUserAndFileComments()
        {
            var comments = new List<Comment>();
            var comment1 = new Comment
            {
                FileId = TestFile,
                UserId = TestUser,
                Text = "Test comment",
                AddDate = DateTime.Now
            };
            var comment2 = new Comment
            {
                FileId = TestFile,
                UserId = TestUser,
                Text = "Test comment2",
                AddDate = DateTime.Now
            };
            comments.Add(comment1);
            comments.Add(comment2);

            var newComment1 = _commentsRepository.Add(comments[0]);
            var newComment2 = _commentsRepository.Add(comments[1]);
            var resultbyUser = _commentsRepository.GetUserComments(TestUser.Id);
            var resultbyFile = _commentsRepository.GetFileComments(TestFile.Id);

            foreach (var res in resultbyUser)
            {
                var i = comments.FindIndex(f => f.Id == res.Id);
                Assert.AreEqual(comments[i].FileId.Id, res.FileId.Id);
                Assert.AreEqual(comments[i].UserId.Id, res.UserId.Id);
                Assert.AreEqual(comments[i].Text, res.Text);
                Assert.AreEqual(comments[i].AddDate.ToString(), res.AddDate.ToString());
            }

            foreach (var res in resultbyFile)
            {
                var i = comments.FindIndex(f => f.Id == res.Id);
                Assert.AreEqual(comments[i].FileId.Id, res.FileId.Id);
                Assert.AreEqual(comments[i].UserId.Id, res.UserId.Id);
                Assert.AreEqual(comments[i].Text, res.Text);
                Assert.AreEqual(comments[i].AddDate.ToString(), res.AddDate.ToString());
            }
        }
    }
}
