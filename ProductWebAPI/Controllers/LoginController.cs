using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Migrations;
using ProductWebAPI.Model;
using System;
using System.Security.Cryptography.Xml;

namespace ProductWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly Entityclass _context;

        public LoginController(Entityclass context)
        {
            _context = context;   
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
            var result = _context.login
                .FromSqlRaw("EXEC VerifyLogin @Username={0}, @Password={1}", dto.Username, dto.Password)
                .AsEnumerable()
                .FirstOrDefault();

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { result});
        }

    }
}
