using System;
using System.Collections.Generic;
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
        public void ShouldCreateAndGetFileInfo()
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
            Assert.AreEqual(file.AddDate.ToString(), result.AddDate.ToString());
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

        [TestMethod]
        public void ShouldGetUserFiles()
        {
            var files = new List<File>();
            var file1 = new File
            {
                Name = "testFile1",
                UserId = TestUser,
                AddDate = DateTime.Now
            };
            var file2 = new File
            {
                Name = "testFile2",
                UserId = TestUser,
                AddDate = DateTime.Now
            };
            files.Add(file1);
            files.Add(file2);

            var newFile1 = _filesRepository.Add(files[0]);
            var newFile2 = _filesRepository.Add(files[1]);
            var result = _filesRepository.GetUserFiles(TestUser.Id);

            foreach (var res in result)
            {
                var i = files.FindIndex(f => f.Id == res.Id);
                Assert.AreEqual(files[i].Name, res.Name);
                Assert.AreEqual(files[i].UserId.Id, res.UserId.Id);
            }
        }
    }
}
