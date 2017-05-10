using System;
using System.Linq;
using System.Text;
using FileHostingService.DataAccess;
using FileHostingService.DataAccess.SQL;
using FileHostingService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace FileHosting.DataAccess.SQL.Tests
{
    [TestClass]
    public class FilesRepositoryTests
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);
        private readonly IFilesRepository _filesRepository;

        private User TestUser { get; set; }

        public FilesRepositoryTests()
        {
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
        }

        [TestCleanup]
        public void Clean()
        {
            if (TestUser != null)
            {
                foreach (var file in _filesRepository.GetUserFiles(TestUser.Id))
                    _filesRepository.Delete(file.Id);
                _usersRepository.Delete(TestUser.Id);
            }
        }

        [TestMethod]
        public void ShouldCreateAndGetInfo()
        {
            //arrange
            var file = new File
            {
                Name = "testFile",
                UserId = TestUser,
                AddDate = DateTime.Now
            };
            //act
            var newFile = _filesRepository.Add(file);
            var result = _filesRepository.GetInfo(newFile.Id);
            //asserts
            Assert.AreEqual(file.UserId.Id, result.UserId.Id);
            Assert.AreEqual(file.Name, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldDeleteFile()
        {
            //arrange
            var file = new File
            {
                Name = "testFile",
                UserId = TestUser,
                AddDate = DateTime.Now
            };
            //act
            var newFile = _filesRepository.Add(file);
            _filesRepository.Delete(file.Id);
            var result = _filesRepository.GetInfo(file.Id);
        }

        [TestMethod]
        public void ShouldUpdateFileContent()
        {
            var file = new File
            {
                Name = "testFile",
                UserId = TestUser,
                AddDate = DateTime.Now
            };
            var content = Encoding.UTF8.GetBytes("Hello");
            var newFile = _filesRepository.Add(file);
            //act
            _filesRepository.UpdateContent(newFile.Id, content);
            var resultContent = _filesRepository.GetContent(newFile.Id);
            //asserts
            Assert.IsTrue(content.SequenceEqual(resultContent));
        }
    }
}
