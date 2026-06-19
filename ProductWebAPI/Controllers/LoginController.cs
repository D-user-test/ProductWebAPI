using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Migrations;
using ProductWebAPI.Model;
using ProductWebAPI.Services.LoginService;
using ProductWebAPI.Services.TokenService;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ProductWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly Entityclass _context;

        private readonly ITokenServices _token;
        private readonly ILoginServices _loginService;

        public LoginController(Entityclass context,ITokenServices token, ILoginServices loginService)
        {
            _context = context;   
            _token = token;
            _loginService = loginService;
        }
      

        [HttpPost("AddUser")]
        public IActionResult AddLogin(logindto logindata)
        {
            Login log = new Login();

            log.username = logindata.Username;
            log.password = logindata.Password;

            _context.login.Add(log);
            _context.SaveChanges();

            // Return response DTO
            var response = new Login
            {
                Id = log.Id,
                username = log.username,
                password = log.password
            };

            return Ok(response);
           
        }

        [HttpGet("GetAllLogins")]
        public IActionResult GetAllLogins()
        {
            var logins = _context.login.ToList();
            if (logins == null)
            {
                return NotFound("Data not found");
            }// fetch all rows
            return Ok(new { logins });
        }


        [HttpGet("loginuserdata")]      
        public IActionResult getuserdata(int id)
        {
            var login = _context.login.FirstOrDefault(l => l.Id == id);
            if (login == null)
            {
                return NotFound("Data not found");
            }
            return Ok(login);
          
        }


        [HttpPost("VerifyLogin")]
        public IActionResult VerifyLogin(logindto dto)
        {
            try { 
                var result = _loginService.VerifyLogin(dto.Username, dto.Password);
                if (result == null)
                    return Unauthorized("Invalid credentials");

                string role = result.username == "admin" ? "Admin" : "User";
                var token = _token.GenerateToken(result.username, role);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                // This matches what the test expects
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
