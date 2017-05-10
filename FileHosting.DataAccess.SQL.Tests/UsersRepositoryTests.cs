using System;
using FileHostingService.DataAccess;
using FileHostingService.DataAccess.SQL;
using FileHostingService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileHosting.DataAccess.SQL.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private const string ConnectionString = "Data Source=LENOVO-PC; Initial Catalog=FileSharingDB; Integrated Security=True; Pooling=False";
        private readonly IUsersRepository _usersRepository;

        public UsersRepositoryTests()
        {
            _usersRepository = new UsersRepository(ConnectionString);
        }

        private User User { get; set; }

        [TestInitialize]
        public void Init()
        {
            User = new User
            {
                Name = "name",
                Surname = "surname",
                Email = "test@gmail.com"
            };
        }

        [TestMethod]
        public void ShouldCreateAndGetUser()
        {
            //arrange
            
            //act
            var newUser = _usersRepository.Add(User);
            var result = _usersRepository.Get(newUser.Id);
            //asserts
            Assert.AreEqual(User.Name,result.Name);
            Assert.AreEqual(User.Surname, result.Surname);
            Assert.AreEqual(User.Email, result.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldDeleteUser()
        {
            //arrange
           
            //act
            var newUser = _usersRepository.Add(User);
            _usersRepository.Delete(newUser.Id);
            var result = _usersRepository.Get(newUser.Id);
            //asserts
        }
    }
}
