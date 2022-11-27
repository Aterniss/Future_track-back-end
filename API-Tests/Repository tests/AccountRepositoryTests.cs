using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Repository_tests
{
    public class AccountRepositoryTests
    {
        AccountRepository _accountRepository;

        protected static DbContextOptions<MyDbContext> dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "API-Tests")
            .Options;

        protected MyDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            var db = new FakeDatabase();
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            db.SeedDatabase(_context);

            _accountRepository = new AccountRepository(_context);
        }

        [Test,Order(1)]
        public void Add_WithoutException_AddOrder()
        {
            var newAccount = new Account()
            {
                UserName = "account4",
                UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                EmailAddress = "sample@gmail.com",
                RestaurantId = null,
                IdUsers = null,
                Role = 1,
                TelNumber = null,
            };
            var result = _accountRepository.Add(newAccount);
            Assert.DoesNotThrowAsync(() => result);

        }
        [Test,Order(2)]
        public void Add_WithException_ThrowCorrectException()
        {
            var newAccount = new Account()
            {
                UserName = "account1",
                UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                EmailAddress = "sample@gmail.com",
                RestaurantId = null,
                IdUsers = null,
                Role = 1,
                TelNumber = null,
            };
            var result = _accountRepository.Add(newAccount);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo("Username already exists!"));
        }
        [Test ,Order(3)]
        public void Delete_WithoutException_DeleteAccount()
        {
            int accountId = 3;
            var result = _accountRepository.Delete(accountId);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(4)]
        public void Delete_WithException_ThrowException()
        {
            int accountId = 999;
            var result = _accountRepository.Delete(accountId);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"The account with ID: \"{accountId}\" does not exist!"));
        }
        [Test, Order(5)]
        public void GetAll_WithoutException_returnAllAccounts()
        {
            var result = _accountRepository.GetAll();
            Assert.That(result.Result, Is.Not.Null);
        }
        [Test, Order(6)]
        public void GetById_WhenIdDoesNotExist_ReturnNull()
        {
            int id = 99999;
            var result = _accountRepository.GetById(id);
            Assert.That(result.Result, Is.Null);
        }
        [Test, Order(7)]
        public void GetById_WhenIdExist_ReturnCorrectAccount()
        {
            int id = 1;
            var result = _accountRepository.GetById(id);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.Id, Is.EqualTo(id));
            Assert.That(result.Result.UserName, Is.EqualTo("account1"));
        }
        [Test, Order(8)]
        public void Login_WhenUserNameIsCorrectAndPasswordNot_ReturnNull()
        {
            string username = "account1";
            string password = "fakepassword";
            var result = _accountRepository.Login(username, password);
            Assert.That(result.Result, Is.Null);
        }
        [Test]
        public void Login_WhenUserNameIsNotCorrectButPasswordIs_ReturnNull()
        {
            string username = "fakeaccout1";
            string password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918";
            var result = _accountRepository.Login(username, password);
            Assert.That(result.Result, Is.Null);
        }
        [Test]
        public void Login_WhenUserNameAndPasswordIsCorrect_ReturnAccount()
        {
            string username = "account1";
            string password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918";
            var result = _accountRepository.Login(username, password);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.EmailAddress, Is.EqualTo("sample@gmail.com"));
        }
        [Test]
        public void Register_WhenUserNameExist_ThrowException()
        {
            var request = new AccountRegistration()
            {
                FullName = "Sample name",
                UserName = "account1",
                UserPassword = "password",
                UserAddress = "address 150",
                EmailAddress = "sample123@gmail.com",
                IsOver18 = false,
                Role = 1,
                TelNumber = null,
                IdUsers = null,
                RestaurantId = null
            };
            var result = _accountRepository.Register(request);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo("Username already exists!"));

        }
        [Test]
        public void Register_WhenUserNameDoesNotExist_CreateAccountAndUser()
        {
            var request = new AccountRegistration()
            {
                FullName = "Sample name",
                UserName = "accounttest2",
                UserPassword = "password",
                UserAddress = "address 150",
                EmailAddress = "sample123@gmail.com",
                IsOver18 = false,
                Role = 1,
                TelNumber = null,
                IdUsers = null,
                RestaurantId = null
            };
            var result = _accountRepository.Register(request);
            Assert.DoesNotThrowAsync(() => result);

        }
        [Test]
        public void Update_WhenAccountIdIsWrong_ThrowException()
        {
            int id = 999;
            var request = new Account()
            {
                UserName = "account8",
                UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                EmailAddress = "sample@gmail.com",
                RestaurantId = null,
                IdUsers = null,
                Role = 1,
                TelNumber = null,
            };
            var result = _accountRepository.Update(request, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo($"The account with ID: \"{id}\" does not exist!"));
         }
        [Test]
        public void Update_WhenUsernameAlreadyExist_ThrowException()
        {
            int id = 2;
            var request = new Account()
            {
                UserName = "account1",
                UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                EmailAddress = "sample@gmail.com",
                RestaurantId = null,
                IdUsers = null,
                Role = 1,
                TelNumber = null,
            };
            var result = _accountRepository.Update(request, id);
            Assert.That(() => result, Throws.Exception.With.Message.EqualTo("Username already exists!"));
        }
        [Test]
        public void Update_WhenEverythingIsOk_WithoutException()
        {
            int id = 2;
            var request = new Account()
            {
                UserName = "account987",
                UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                EmailAddress = "sample@gmail.com",
                RestaurantId = null,
                IdUsers = null,
                Role = 1,
                TelNumber = null,
            };
            var result = _accountRepository.Update(request, id);
            Assert.DoesNotThrowAsync(() => result);
            
        }





        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }




    }
}
