using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_API.DTO;
using Project_API.DTO.RequestModels;
using Project_API.Models;
using Project_API.Repositories;
using System.Net.Mail;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _account;
        private readonly IMapper mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository dish, IMapper mapper, ILogger<AccountController> logger)
        {
            this._account = dish;
            this.mapper = mapper;
            this._logger = logger;
            
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation(ReturnLogMessage("Account", "GetAll"));
            try
            {
                var result = await _account.GetAll();
                var resultDTO = mapper.Map<List<AccountDTO>>(result);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "GetById"));
            var result = await _account.GetById(id);
            if (result == null)
            {
                var msg = $"The account with Id: \"{id}\" was not found!";
                _logger.LogWarning(msg);
                return NotFound(msg);
            }
            else
            {
                var resultDTO = mapper.Map<AccountDTO>(result);
                return Ok(resultDTO);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "DeleteAccount"));
            if (id <= 0)
            {
                var msg = $"The ID can not be less or equal to 0!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            else if (id == 1)
            {
                var msg = $"You can not delete Admin account!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                await _account.Delete(id);
                return Ok("Deleted succesfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost()]
        public async Task<IActionResult> AddAccount([FromBody] AccountRequestModel request)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "AddAccount"));
            if (request.EmailAddress == "" || request.UserName == "" || request.UserPassword == "")
            {
                var msg = $"Fields: \"User name\", \"Password\" and \"e-mail\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            else if (IsValidEmail(request.EmailAddress) == false)
            {
                var msg = $"Email address format is invalid!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var newAccount = new Account()
                {
                    UserName = request.UserName,
                    UserPassword = request.UserPassword,
                    EmailAddress = request.EmailAddress,
                    TelNumber = request.TelNumber = null,
                    Role = 1, // basic user!
                    RestaurantId = null,
                    IdUsers = null
                };
                await _account.Add(newAccount);
                return Ok("Succesfully added!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountAdminRequest request, int id)
        {
            var trimmedEmail = request.EmailAddress.Trim();
            _logger.LogInformation(ReturnLogMessage("Account", "UpdateAccount"));
            if (request.EmailAddress == "" || request.UserName == "" || request.UserPassword == "")
            {
                var msg = $"Fields: \"User name\", \"Password\" and \"e-mail\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            else if (IsValidEmail(request.EmailAddress) == false)
            {
                var msg = $"Email address format is invalid!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var newAccount = new Account()
                {
                    UserName = request.UserName,
                    UserPassword = request.UserPassword,
                    EmailAddress = request.EmailAddress,
                    TelNumber = request.TelNumber,
                    Role = request.Role,
                    RestaurantId = request.RestaurantId,
                    IdUsers = request.IdUsers
                };
                await _account.Update(newAccount, id);
                return Ok("Succesfully updated!");

            }
            catch (BadHttpRequestException e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }
        [HttpGet("{username}, {password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if(username == null || password == null)
            {
                var msg = "Please enter username and password!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                var result = await _account.Login(username, password);
                _logger.LogInformation(ReturnLogMessage("Account", "Login"));
                if(result == null)
                {
                    var msg = "Username or password is incorrect!";
                    _logger.LogWarning(msg);
                    return BadRequest(msg);
                }
                return Ok(result);
            }
            catch (BadHttpRequestException e)
            {
                _logger.LogWarning(e.Message);
                return BadRequest(e.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("/accounts/register")]
        public async Task<IActionResult> Register([FromBody] AccountRegistration request)
        {
            _logger.LogInformation(ReturnLogMessage("Account", "Register"));
            if (request.EmailAddress == "" || request.UserName == "" || request.UserPassword == "" || request.FullName == "" || request.UserAddress == "")
            {
                var msg = $"Fields: \"User name\", \"Password\", \"Full name\", \"Your address\" and \"e-mail\" are required!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            else if (IsValidEmail(request.EmailAddress) == false)
            {
                var msg = $"Email address format is invalid!";
                _logger.LogWarning(msg);
                return BadRequest(msg);
            }
            try
            {
                await _account.Register(request);
                return Ok("Succesfully added!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }


        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string ReturnLogMessage(string controllerClassName, string nameMethod)
        {
            return $"Controller: {controllerClassName}Controller: Request: {nameMethod}()";
        }
    }
}
