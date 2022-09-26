using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using APIIII.Data;
using APIIII.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIIII.Controllers
{
    [ApiController]
    [Route("apiiii/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers(){
           return _context.Users.ToList();
        
        }

        [HttpGet]
        public ActionResult<AppUser> GetUser(){
            return _context.Users.Find();
        
        }
    }
}