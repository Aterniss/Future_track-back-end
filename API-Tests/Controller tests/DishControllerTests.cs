using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Project_API.Controllers;
using Project_API.DTO;
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
    internal class DishControllerTests
    {
        private DishController _dishController;
        private IMapper _mapper;
        DishRepository repo;
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
            repo = new DishRepository(_context);
            var profile = new DishProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(configuration);
            _dishController = new DishController(repo, _mapper, new NullLogger<DishController>());
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllDishes_ReturnOk_Test()
        {
            Task<IActionResult> actionResult = _dishController.GetAllDishes();
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
           // Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_ReturnOk_WhenIdExist_Test()
        {
            int id = 3;
            Task<IActionResult> actionResult = _dishController.GetById(id);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => actionResult);
            var dish = (actionResult.Result as OkObjectResult).Value as DishDTO;
            Assert.That(dish.DishId, Is.EqualTo(id));
            Assert.That(dish.DishName, Is.EqualTo("Dish nr 3"));  
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_ReturnNotFound_WhenIdDOesNotExist_Test()
        {
            int id = 999;
            var actionResult = _dishController.GetById(id);
            Assert.That(actionResult.Result, Is.TypeOf<NotFoundObjectResult>());
            Assert.DoesNotThrowAsync(() => actionResult);
        }
        [Test, Order(4)]
        public void HTTPGET_GetByName_WhenNameExist_ReturnOK()
        {
            string name = "Dish nr 3";
            Task<IActionResult> actionResult = _dishController.GetByName(name);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => actionResult);
        }
        [Test, Order(5)]
        public void HTTPGET_GetByName_WhenNameDoesNotExist_ReturnNotFound()
        {
            string name = "Dish nr 999";
            Task<IActionResult> actionResult = _dishController.GetByName(name);
            Assert.That(actionResult.Result, Is.TypeOf<NotFoundObjectResult>());
            Assert.DoesNotThrowAsync(() => actionResult);
        }
        [Test, Order(6)]
        public void HTTPPUT_UpdateDish_WhenNameIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "",
                DishDescription = "text",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            int id = 2;

            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(14)]
        public void HTTPPUT_UpdateDish_WhenDescriptionIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "Dish Name",
                DishDescription = "",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            int id = 2;
            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(15)]
        public void HTTPPUT_UpdateDish_WhenPriceIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "Dish Name",
                DishDescription = "text",
                Price = 0,
                Require18 = false,
                RestaurantId = 1
            };
            int id = 2;
            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(16)]
        public void HTTPPUT_UpdateDish_WhenRestaurantIdIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "Dish Name",
                DishDescription = "text",
                Price = 1,
                Require18 = false,
                RestaurantId = 0
            };
            int id = 2;
            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(17)]
        public void HTTPPUT_UpdateDish_WhenAllFieldsAreEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "",
                DishDescription = "",
                Price = 0,
                Require18 = false,
                RestaurantId = 0
            };
            int id = 2;
            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(7)]
        public void HTTPPUT_UpdateDish_WhenIdIsWrong_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "new dish",
                DishDescription = "text",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            int id = 921;

            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(8)]
        public void HTTPPUT_UpdateDish_WhenEverythingIsCorrect_ReturnOk()
        {
            int id = 4;
            var request = new DishRequestModel()
            {
                DishName = "update",
                DishDescription = "sample text",
                Price = 1,
                Require18 = false,
                RestaurantId = 2
            };
            
            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(9)]
        public void HTTPDELETE_DeleteDish_WhenIdDoesNotExist_ReturnBadRequest()
        {
            int id = 999;
            Task<IActionResult> result = _dishController.DeleteDish(id);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(10)]
        public void HTTPDELETE_DeleteDish_WhenIdExist_ReturnOk()
        {
            int id = 3;
            Task<IActionResult> result = _dishController.DeleteDish(id);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(11)]
        public void HTTPPOST_AddNewDish_WhenFieldsAreEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "",
                DishDescription = "",
                Price = 0,
                Require18 = false,
                RestaurantId = 0
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(18)]
        public void HTTPPOST_AddNewDish_WhenNameIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "",
                DishDescription = "text",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(19)]
        public void HTTPPOST_AddNewDish_WhenDescriptionIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "Text",
                DishDescription = "",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(20)]
        public void HTTPPOST_AddNewDish_WhenPriceIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "text",
                DishDescription = "text",
                Price = 0,
                Require18 = false,
                RestaurantId = 1
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(21)]
        public void HTTPPOST_AddNewDish_WhenRestaurantIdIsEmpty_ReturnBadRequest()
        {
            var request = new DishRequestModel()
            {
                DishName = "text",
                DishDescription = "text",
                Price = 1,
                Require18 = false,
                RestaurantId = 0
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }
        [Test, Order(12)]
        public void HTTPPOST_AddNewDish_WhenIsException_ReturnNotFound()
        {
            var request = new DishRequestModel()
            {
                DishName = "ad",
                DishDescription = "asda",
                Price = 1,
                Require18 = false,
                RestaurantId = 99
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }
        [Test, Order(13)]
        public void HTTPPOST_AddNewDish_WithoutException_ReturnOk()
        {
            var request = new DishRequestModel()
            {
                DishName = "new dish name",
                DishDescription = "sample text",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            Task<IActionResult> result = _dishController.AddNewDish(request);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => result);
        }
        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }


    }
}
