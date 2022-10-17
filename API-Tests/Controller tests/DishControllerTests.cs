﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Project_API.Controllers;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests.Controller_tests
{
    internal class DishControllerTests : FakeDatabase
    {
        //private readonly Mock<IDishRepository> _dishRepository;
        private DishController _dishController;
        private readonly Mock<IMapper> _mapper;

        DishRepository repo;
        public DishControllerTests()
        {
            // this._dishRepository = new Mock<IDishRepository>();
            this._mapper = new Mock<IMapper>();
            //this._dishController = new DishController(_dishRepository.Object, _mapper.Object, new NullLogger<DishController>());
            //_dbContext = new Mock<MyDbContext>(_context);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new MyDbContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
            repo = new DishRepository(_context);
            _dishController = new DishController(repo, _mapper.Object, new NullLogger<DishController>());
        }
        [Test, Order(1)]
        public void HTTPGET_GetAllDishes_ReturnOk_Test()
        {
            Task<IActionResult> actionResult = _dishController.GetAllDishes();
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test, Order(2)]
        public void HTTPGET_GetById_ReturnOk_WhenIdExist_Test()
        {
            int id = 3;
            Task<IActionResult> actionResult = _dishController.GetById(id);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());
            Assert.DoesNotThrowAsync(() => actionResult);
            //var dish = (actionResult.Result as OkObjectResult).Value as DishDTO;
            //Assert.That(dish.DishId, Is.EqualTo(id));
            
        }
        [Test, Order(3)]
        public void HTTPGET_GetById_ReturnNotFound_WhenIdDOesNotExist_Test()
        {
            int id = 999;
            Task<IActionResult> actionResult = _dishController.GetById(id);
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
        public void HTTPPUT_UpdateDish_WhenFieldsAreEmpty_ReturnBadRequest()
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
        //test when dish ID does not exist!
        [Test, Order(7)]
        public void HTTPPUT_UpdateDish_WhenIdIsWrong_ReturnNotFound()
        {
            var request = new DishRequestModel()
            {
                DishName = "new dish",
                DishDescription = "text",
                Price = 1,
                Require18 = false,
                RestaurantId = 1
            };
            int id = 99999;

            Task<IActionResult> result = _dishController.UpdateDish(request, id);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
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
           //Assert.DoesNotThrowAsync(() => result);
        }
















        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }


    }
}
