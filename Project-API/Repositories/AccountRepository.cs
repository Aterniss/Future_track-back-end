using Microsoft.EntityFrameworkCore;
using Project_API.DTO.RequestModels;
using Project_API.Models;

namespace Project_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyDbContext _context;

        public AccountRepository(MyDbContext context)
        {
            this._context = context;
        }
        public async Task Add(Account account)
        {
            var checkUsername = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == account.UserName);
            if(checkUsername == null)
            {
                await _context.AddAsync(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new BadHttpRequestException("Username already exists!");
            }
            
        }

        public async Task Delete(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if(account == null)
            {
                throw new Exception($"The account with ID: \"{id}\" does not exist!");
            }
            else
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            try
            {
                return await _context.Accounts
                .Include(x => x.Restaurant)
                .Include(x => x.IdUsersNavigation)
                .Include(x => x.RoleNavigation)
                .ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
        }

        public async Task<Account> GetById(int id)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<Account> Login(string username, string password)
        {
            var result = await _context.Accounts.Where(x => x.UserName == username && x.UserPassword == password).FirstOrDefaultAsync();
            if(result == null)
            {
                return null;
            }
            else
            {
                return result;
            }


        }

        public async Task Register(AccountRegistration request)
        {
            var checkUserName = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if(checkUserName == null)
            {
                var newUser = new User()
                {
                    FullName = request.FullName,
                    UserAddress = request.UserAddress,
                    IsOver18 = request.IsOver18,
                    CreatedAt = DateTime.Now,
                    LastUpdate = DateTime.Now,
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                var newAccount = new Account()
                {
                    UserName = request.UserName,
                    UserPassword = request.UserPassword,
                    EmailAddress = request.EmailAddress,
                    TelNumber = request.TelNumber = null,
                    Role = 1, // basic user!
                    RestaurantId = null,
                    IdUsers = newUser.IdUser
                };
                await _context.Accounts.AddAsync(newAccount);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new BadHttpRequestException("Username already exists!");
            }
        }

        public async Task Update(Account account, int id)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                throw new Exception($"The account with ID: \"{id}\" does not exist!");
            }
            else
            {
                var checkUsername = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName == account.UserName);
                if(checkUsername != null)
                {
                    throw new BadHttpRequestException("Username already exists!");
                }
                else
                {
                    
                    result.Role = account.Role;
                    result.TelNumber = account.TelNumber;
                   
                    result.RestaurantId = account.RestaurantId;
                    result.IdUsers = account.IdUsers;
                    result.EmailAddress = account.EmailAddress;

                    await _context.SaveChangesAsync();

                }

                
            }
        }


    }
}
