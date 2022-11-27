using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using Project_API.Repositories;

namespace API_Tests.Repository_tests
{
    public class DishRepositoryTests
    {

        DishRepository _dishRepository;
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

            _dishRepository = new DishRepository(_context);
        }

        [Test, Order(1)]
        public void GetAll_ReturnSearchResult()
        {
            var result = _dishRepository.GetAll();
            Assert.That(result, Is.Not.Null);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(2)]
        public async Task GetDishById_WhenIdExist_ReturnCorrectObject()
        { 
            var testDish = new Dish()
            {
                DishId = 1,
                DishName = "Dish nr 1",
                DishDescription = "Dish description 1",
                RestaurantId = 1,
                Price = 4.99M,
                Require18 = true
            };
            int dishTestId = 1;
            var result = await _dishRepository.GetDishById(dishTestId);

            Assert.That(result.DishName, Is.EqualTo(testDish.DishName));
            Assert.That(result.DishId, Is.EqualTo(testDish.DishId));
            Assert.That(result.DishId, Is.EqualTo(testDish.DishId));
            Assert.That(result.DishDescription, Is.EqualTo(testDish.DishDescription));
            Assert.That(result.RestaurantId, Is.EqualTo(testDish.RestaurantId));
            Assert.That(result.Price, Is.EqualTo(testDish.Price));
            Assert.That(result.Require18, Is.EqualTo(testDish.Require18));
        }

        [Test, Order(3)]
        public async Task GetDishById_WhenIdDoesNotExist_ReturnNull()
        {
            var dishTestId = 999;
            var result = await _dishRepository.GetDishById(dishTestId);
            Assert.That(result, Is.Null);
        }
        [Test, Order(4)]
        public async Task GetDishesByName_WhenNameExist_ReturnListOfObjects()
        {
            var dishName = "Dish nr 1";
            var result = await _dishRepository.GetDishesByName(dishName);
            Assert.That(result, Is.Not.Null);
        }
        [Test, Order(5)]
        public async Task GetDishesByName_WhenNameDoesNotExist_ReturnNull()
        {
            var dishName = "Sample dish name";
            var result = await _dishRepository.GetDishesByName(dishName);
            Assert.That(result, Is.Null);
        }
        [Test, Order(6)]
        public void AddNewDish_WithException_Test()
        {
            var testDish = new Dish()
            {
                DishName = "Dish nr 4",
                DishDescription = "Dish description 4",
                RestaurantId = 971,
                Price = 4.99M,
                Require18 = true
            };

            Assert.That(() => _dishRepository.AddNewDish(testDish), Throws.Exception);
        }
        [Test, Order(7)]
        public void AddNewDish_WithoutException_Test()
        {
            var testDish = new Dish()
            {
                DishName = "Dish nr 4",
                DishDescription = "Dish description 4",
                RestaurantId = 1,
                Price = 4.99M,
                Require18 = true
            };

            var result = _dishRepository.AddNewDish(testDish);
            Assert.DoesNotThrowAsync(() => result);
        }


        [Test, Order(8)]
        public void DeleteDishById_WithException_Test()
        {
            var dishId = 961;
            Assert.That(() => _dishRepository.DeleteDishById(dishId), Throws.Exception.With.Message.EqualTo($"The dish with ID: \"{dishId}\" does not exist!"));
        }

        [Test, Order(9)]
        public void DeleteDishById_WithoutException_Test()
        {
            var id = 1;
            var result = _dishRepository.DeleteDishById(id);
            Assert.DoesNotThrowAsync(() => result);
        }
        [Test, Order(10)]
        public void UpdateDishById_WhenDishIdDoesNotExist_ThrowCorrectException()
        {
            int id = 92;
            var testDish = new Dish()
            {
                DishName = "Dish nr 10",
                DishDescription = "Dish description 10",
                RestaurantId = 2,
                Price = 4.99M,
                Require18 = true
            };
            Assert.That(() => _dishRepository.UpdateDishById(testDish, id), Throws.Exception.With.Message.EqualTo($"The dish with ID: \"{id}\" does not exist!"));
        }
        [Test, Order(11)]
        public void UpdateDishById_WhenRestaurantIdDoesNotExist_ThrowCorrectException()
        {
            int id = 2;
            var testDish = new Dish()
            {
                DishName = "Dish nr 10",
                DishDescription = "Dish description 10",
                RestaurantId = 92,
                Price = 4.99M,
                Require18 = true
            };
            Assert.That(() => _dishRepository.UpdateDishById(testDish, id), Throws.Exception.With.Message.EqualTo($"The given restaurant ID: \"{testDish.RestaurantId}\" does not exist!"));
        }
        [Test, Order(12)]
        public void UpdateDishById_WithoutException_Test()
        {
            int id = 2;
            var testDish = new Dish()
            {
                DishName = "Dish nr 10",
                DishDescription = "Dish description 10",
                RestaurantId = 2,
                Price = 4.99M,
                Require18 = true
            };
            var result = _dishRepository.UpdateDishById(testDish, id);
            Assert.DoesNotThrowAsync(() => result);
        }



        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

      

    }
}