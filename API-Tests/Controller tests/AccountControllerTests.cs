using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Project_API.Controllers;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Profiles;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Controller_tests
{
    public class AccountControllerTests
    {
        private AccountController _account;
        private IMapper mapper;
        AccountRepository repo;
        protected static DbContextOptions<MyDbContext> dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "API-Tests")
            .Options;

        protected MyDbContext _context;
        [OneTimeSetUp]
        public void Setup()
        {
            var database = new FakeDatabase();
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            database.SeedDatabase(_context);
            repo = new AccountRepository(_context);
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AccountProfile>();
            });
            mapper = new Mapper(config);
            _account = new AccountController(repo, mapper, new NullLogger<AccountController>());
        }
        [Test]
        public void HTTPGET_GetAll_WithOutException_ReturnAccounts()
        {
            var result = _account.GetAll();
            Assert.DoesNotThrowAsync(() => result);
            Assert.That(result.Result, Is.Not.Null);
        }
        [Test]
        public void HTTPGET_GetById_WhenIdDoesNotExist_ReturnNotFound()
        {
            int id = 999;
            var result = _account.GetById(id);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }
        [Test]
        public void HTTPGET_GetById_WhenIdExist_ReturnOk()
        {
            int id = 1;
            var result = _account.GetById(id);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void HTTPDELETE_DeleteAccount_WhenIdIsLessThanOne_ReturnBadRequest()
        {
            int id = -1;
            var result = _account.DeleteAccount(id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPDELETE_DeleteAccount_WhenIdIsEqualToOne_ReturnBadRequest()
        {
            int id = 1;
            var result = _account.DeleteAccount(id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPDELETE_DeleteAccount_WithOutException_ReturnOk()
        {
            int id = 3;
            var result = _account.DeleteAccount(id);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void HTTPPOST_AddAccount_WhenUsernameFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRequestModel()
            {
                UserName = "",
                UserPassword = "Password",
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.AddAccount(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_AddAccount_WhenPasswordFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRequestModel()
            {
                UserName = "sampleuser",
                UserPassword = "",
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.AddAccount(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_AddAccount_WhenEmailFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRequestModel()
            {
                UserName = "User",
                UserPassword = "Password",
                EmailAddress = "",
                TelNumber = null
            };
            var result = _account.AddAccount(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_AddAccount_WhenEmailFormatIsInvalid_ReturnBadRequest()
        {
            var request = new AccountRequestModel()
            {
                UserName = "username123113",
                UserPassword = "Password",
                EmailAddress = "sample",
                TelNumber = null
            };
            var result = _account.AddAccount(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_AddAccount_WithoutException_ReturnOk()
        {
            var request = new AccountRequestModel()
            {
                UserName = "username12111",
                UserPassword = "Password",
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.AddAccount(request);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void HTTPPUT_UpdateAccount_WhenUsernameFieldIsEmpty_ReturnBadRequest()
        {
            int id = 1;
            var request = new AccountAdminRequest()
            {
                UserName = "",
                UserPassword = "password1231",
                EmailAddress = "sample@gmail.com",
                Role = 1,
                IdUsers = null,
                RestaurantId = null,
                TelNumber = null
            };

            var result = _account.UpdateAccount(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPUT_UpdateAccount_WhenPasswordFieldIsEmpty_ReturnBadRequest()
        {
            int id = 1;
            var request = new AccountAdminRequest()
            {
                UserName = "account123131",
                UserPassword = "",
                EmailAddress = "sample@gmail.com",
                Role = 1,
                IdUsers = null,
                RestaurantId = null,
                TelNumber = null
            };

            var result = _account.UpdateAccount(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPUT_UpdateAccount_WhenEmailFieldIsEmpty_ReturnBadRequest()
        {
            int id = 1;
            var request = new AccountAdminRequest()
            {
                UserName = "account123131",
                UserPassword = "password1231",
                EmailAddress = "",
                Role = 1,
                IdUsers = null,
                RestaurantId = null,
                TelNumber = null
            };

            var result = _account.UpdateAccount(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPUT_UpdateAccount_WhenEmailFormatIsInvalid_ReturnBadRequest()
        {
            int id = 1;
            var request = new AccountAdminRequest()
            {
                UserName = "username1231311",
                UserPassword = "password1231",
                EmailAddress = "sample",
                Role = 1,
                IdUsers = null,
                RestaurantId = null,
                TelNumber = null
            };

            var result = _account.UpdateAccount(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPUT_UpdateAccount_WhenIdDoesNotExist_ReturnNotFound()
        {
            int id = 9999;
            var request = new AccountAdminRequest()
            {
                UserName = "username1",
                UserPassword = "password1231",
                EmailAddress = "sample@gmail.com",
                Role = 1,
                IdUsers = null,
                RestaurantId = null,
                TelNumber = null
            };

            var result = _account.UpdateAccount(request, id);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }
        [Test]
        public void HTTPPUT_UpdateAccount_WithoutException_ReturnOk()
        {
            int id = 1;
            var request = new AccountAdminRequest()
            {
                UserName = "username11",
                UserPassword = "password1231",
                EmailAddress = "sample@gmail.com",
                Role = 1,
                IdUsers = null,
                RestaurantId = null,
                TelNumber = null
            };

            var result = _account.UpdateAccount(request, id);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void HTTPGET_Login_WhenUsernameFieldIsEmpty_ReturnBadRequest()
        {
            string username = "";
            string password = "password";

            var result = _account.Login(username, password);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

        }
        [Test]
        public void HTTPGET_Login_WhenPasswordFieldIsEmpty_ReturnBadRequest()
        {
            string username = "username1231";
            string password = "";

            var result = _account.Login(username, password);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

        }
        [Test]
        public void HTTPGET_Login_WhenCredentialsAreInvalid_ReturnBadRequest()
        {
            string username = "username112";
            string password = "password";

            var result = _account.Login(username, password);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPGET_Login_WhenCredentialAreValid_ReturnOk()
        {
            string username = "account1";
            string password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918";

            var result = _account.Login(username, password);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void HTTPPOST_Register_WhenUsernameFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRegistration()
            {
                FullName = "Full name",
                UserName = "",
                UserPassword = "password",
                UserAddress = "address 1121",
                IsOver18 = false,
                Role = 1,
                RestaurantId = null,
                IdUsers = null,
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.Register(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_Register_WhenPasswordFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRegistration()
            {
                FullName = "Full name",
                UserName = "asdadadaad",
                UserPassword = "",
                UserAddress = "address 1121",
                IsOver18 = false,
                Role = 1,
                RestaurantId = null,
                IdUsers = null,
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.Register(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_Register_WhenEmailFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRegistration()
            {
                FullName = "Full name",
                UserName = "username1231",
                UserPassword = "password",
                UserAddress = "address 1121",
                IsOver18 = false,
                Role = 1,
                RestaurantId = null,
                IdUsers = null,
                EmailAddress = "",
                TelNumber = null
            };
            var result = _account.Register(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_Register_WhenFullnameFieldIsEmpty_ReturnBadRequest()
        {
            var request = new AccountRegistration()
            {
                FullName = "",
                UserName = "username1231",
                UserPassword = "password",
                UserAddress = "address 1121",
                IsOver18 = false,
                Role = 1,
                RestaurantId = null,
                IdUsers = null,
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.Register(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_Register_WhenEmailFormatIsInvalid_ReturnBadRequest()
        {
            var request = new AccountRegistration()
            {
                FullName = "Full name",
                UserName = "username 1",
                UserPassword = "password",
                UserAddress = "address 1121",
                IsOver18 = false,
                Role = 1,
                RestaurantId = null,
                IdUsers = null,
                EmailAddress = "sample",
                TelNumber = null
            };
            var result = _account.Register(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void HTTPPOST_Register_WithoutException_ReturnOk()
        {
            var request = new AccountRegistration()
            {
                FullName = "Full name",
                UserName = "username121343",
                UserPassword = "password",
                UserAddress = "address 1121",
                IsOver18 = false,
                Role = 1,
                RestaurantId = null,
                IdUsers = null,
                EmailAddress = "sample@gmail.com",
                TelNumber = null
            };
            var result = _account.Register(request);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }







        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
