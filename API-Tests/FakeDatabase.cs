using Microsoft.EntityFrameworkCore;
using Project_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Tests
{
    internal class FakeDatabase
    {
        public void SeedDatabase(MyDbContext _context)
        {
            var dishes = new List<Dish>
            {
                new Dish()
                {
                    DishId =1,
                    DishName = "Dish nr 1",
                    DishDescription = "Dish description 1",
                    RestaurantId = 1,
                    Price = 4.99M,
                    Require18 = true
                },
                new Dish()
                {
                    DishId =2,
                    DishName = "Dish nr 2",
                    DishDescription = "Dish description 2",
                    RestaurantId = 1,
                    Price = 5.99M,
                    Require18 = true
                },
                new Dish()
                {
                    DishId =3,
                    DishName = "Dish nr 3",
                    DishDescription = "Dish description 3",
                    RestaurantId = 2,
                    Price = 7.99M,
                    Require18 = false
                },
                new Dish()
                {
                    DishId =4,
                    DishName = "Dish nr 4",
                    DishDescription = "Update",
                    RestaurantId = 2,
                    Price = 7.99M,
                    Require18 = false
                }
            };
            _context.Dishes.AddRange(dishes);

            var restaurants = new List<Restaurant>
            {
                new Restaurant()
                {
                    RestaurantId = 1,
                    RestaurantName = "Restaurant name 1",
                    CategoryName = "category name 1",
                    RestaurantAddress = "Address 1",
                    ZoneId = 1
                },
                new Restaurant()
                {
                    RestaurantId = 2,
                    RestaurantName = "Restaurant name 2",
                    CategoryName = "category name 1",
                    RestaurantAddress = "Address 2",
                    ZoneId = 1
                },
                new Restaurant()
                {
                    RestaurantId = 3,
                    RestaurantName = "Restaurant name 3",
                    CategoryName = "category name 2",
                    RestaurantAddress = "Address 3",
                    ZoneId = 2
                },
                new Restaurant()
                {
                    RestaurantId = 4,
                    RestaurantName = "Restaurant name 4",
                    CategoryName = "category name 2",
                    RestaurantAddress = "Address 4",
                    ZoneId = 3
                },
                new Restaurant()
                {
                    RestaurantId = 5,
                    RestaurantName = "Restaurant name 5",
                    CategoryName = "category name 2",
                    RestaurantAddress = "Address 4",
                    ZoneId = 2
                },


            };
            _context.Restaurants.AddRange(restaurants);

            var foodCategories = new List<FoodCategory>
            {
                new FoodCategory()
                {
                    CategoryName = "category name 1",
                    CategoryDescription = "description 1"
                },
                new FoodCategory()
                {
                    CategoryName = "category name 2",
                    CategoryDescription = "description 2"
                },
                new FoodCategory()
                {
                    CategoryName = "category name 3",
                    CategoryDescription = "description 3"
                },
                 new FoodCategory()
                {
                    CategoryName = "to delete",
                    CategoryDescription = "description 4"
                },
                 new FoodCategory()
                {
                    CategoryName = "already exist",
                    CategoryDescription = "description 4"
                },
                 new FoodCategory()
                {
                    CategoryName = "update",
                    CategoryDescription = "description 4"
                },
                 new FoodCategory()
                {
                    CategoryName = "update me",
                    CategoryDescription = "description 4"
                },
            };
            _context.FoodCategories.AddRange(foodCategories);

            var zones = new List<Zone>
            {
                new Zone()
                {
                    ZoneId = 1,
                    ZoneName = "zone name 1"
                },
                new Zone()
                {
                    ZoneId = 2,
                    ZoneName = "zone name 2"
                },
                new Zone()
                {
                    ZoneId = 3,
                    ZoneName = "zone name 3"
                },
                new Zone()
                {
                    ZoneId = 4,
                    ZoneName = "Already exist"
                },
                new Zone()
                {
                    ZoneId = 5,
                    ZoneName = "Delete"
                },
                new Zone()
                {
                    ZoneId = 6,
                    ZoneName = "Update test"
                }
            };
            _context.Zones.AddRange(zones);

            var riders = new List<Rider>
            {
                new Rider()
                {
                    RiderId = 1,
                    RiderName = "rider name 1",
                    ZoneId = 1
                },
                new Rider()
                {
                    RiderId = 2,
                    RiderName = "rider name 2",
                    ZoneId = 2
                },
                new Rider()
                {
                    RiderId = 3,
                    RiderName = "rider name 3",
                    ZoneId = 2
                },
                new Rider()
                {
                    RiderId = 4,
                    RiderName = "rider name 4",
                    ZoneId = 3
                },
                new Rider()
                {
                    RiderId = 5,
                    RiderName = "delete me",
                    ZoneId = 3
                },
                new Rider()
                {
                    RiderId = 6,
                    RiderName = "update me!",
                    ZoneId = 2
                }
            };
            _context.Riders.AddRange(riders);

            var orders = new List<Order>
            {
                new Order()
                {
                    OrderId = 1,
                    OrderStatus = "status 1",
                    CreatedAt = DateTime.Now.AddDays(-15),
                    RiderId = 1,
                    IdUser = 1
                },
                new Order()
                {
                    OrderId = 2,
                    OrderStatus = "status 1",
                    CreatedAt = DateTime.Now.AddDays(-12),
                    RiderId = 1,
                    IdUser = 2
                },
                new Order()
                {
                    OrderId = 3,
                    OrderStatus = "status 2",
                    CreatedAt = DateTime.Now.AddDays(-11),
                    RiderId = 2,
                    IdUser = 2
                },
                new Order()
                {
                    OrderId = 4,
                    OrderStatus = "status 2",
                    CreatedAt = DateTime.Now.AddDays(-6),
                    RiderId = 3,
                    IdUser = 3
                },
                new Order()
                {
                    OrderId = 5,
                    OrderStatus = "status 3",
                    CreatedAt = DateTime.Now.AddDays(-5),
                    RiderId = 2,
                    IdUser = 1
                }
            };
            _context.Orders.AddRange(orders);

            var users = new List<User>
            {
                new User()
                {
                    IdUser = 1,
                    FullName = "User name 1",
                    CreatedAt = DateTime.Now.AddDays(-10),
                    LastUpdate = DateTime.Now.AddDays(-3),
                    UserAddress = "Address 123",
                    IsOver18 = false
                },
                new User()
                {
                    IdUser = 2,
                    FullName = "User name 2",
                    CreatedAt = DateTime.Now.AddDays(-9),
                    LastUpdate = DateTime.Now.AddDays(-4),
                    UserAddress = "Address 121",
                    IsOver18 = true
                },
                new User()
                {
                    IdUser = 3,
                    FullName = "User name 3",
                    CreatedAt = DateTime.Now.AddDays(-10),
                    LastUpdate = DateTime.Now.AddDays(-3),
                    UserAddress = "Address 150",
                    IsOver18 = true
                },
                new User()
                {
                    IdUser = 4,
                    FullName = "User name 4",
                    CreatedAt = DateTime.Now.AddDays(-6),
                    LastUpdate = DateTime.Now.AddDays(-2),
                    UserAddress = "Address 173",
                    IsOver18 = false
                },
                new User()
                {
                    IdUser = 5,
                    FullName = "UDelete me",
                    CreatedAt = DateTime.Now.AddDays(-6),
                    LastUpdate = DateTime.Now.AddDays(-2),
                    UserAddress = "Address 173",
                    IsOver18 = false
                }

            };
            _context.Users.AddRange(users);

            var orderDishes = new List<OrderDish>
            {
                new OrderDish()
                {
                    Id = 1,
                    OrderId = 1,
                    DishId = 1
                },
                new OrderDish()
                {
                    Id = 2,
                    OrderId = 1,
                    DishId = 3
                },
                new OrderDish()
                {
                    Id = 3,
                    OrderId = 2,
                    DishId = 3
                },
                new OrderDish()
                {
                    Id = 4,
                    OrderId = 3,
                    DishId = 1
                },
                new OrderDish()
                {
                    Id = 5,
                    OrderId = 3,
                    DishId = 3
                },
                new OrderDish()
                {
                    Id = 6,
                    OrderId = 2,
                    DishId = 2
                }
            };
            _context.OrderDishes.AddRange(orderDishes);

            var accounts = new List<Account>()
            {
                new Account()
                {
                    Id = 1,
                    UserName = "account1",
                    UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                    EmailAddress = "sample@gmail.com",
                    RestaurantId = null,
                    IdUsers = null,
                    Role = 3,
                    TelNumber = null,
                },
                new Account()
                {
                    Id = 2,
                    UserName = "account2",
                    UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                    EmailAddress = "sample1@gmail.com",
                    RestaurantId = 1,
                    IdUsers = null,
                    Role = 2,
                    TelNumber = null,
                },
                new Account()
                {
                    Id = 3,
                    UserName = "account3",
                    UserPassword = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                    EmailAddress = "sample2@gmail.com",
                    RestaurantId = null,
                    IdUsers = 1,
                    Role = 1,
                    TelNumber = null,
                }

            };
            _context.Accounts.AddRange(accounts);

            _context.SaveChanges();

        }
    }
}
