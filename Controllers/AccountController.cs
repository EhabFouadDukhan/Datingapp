using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context,ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
           if(await UserExists(registerDto.Username)) return BadRequest("User is taken");
            
            var user = _mapper.Map<AppUser>(registerDto);

           using var hmac = new HMACSHA512();
           
            user.UserName =registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
  
           _context.Users.Add(user);
           await _context.SaveChangesAsync();

           return new UserDto
           {
              Username = user.UserName,
              Token = _tokenService.CreateToken(user),
              KnownAs = user.KnownAs,
              Gender = user.Gender
           };

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

            var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName ==loginDto.Username);
            if(user == null) return Unauthorized("invalid Username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<ComputeHash.Length;i++){
                if(ComputeHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
            }
            return new UserDto
           {
              Username = user.UserName,
              Token = _tokenService.CreateToken(user),
              PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
              KnownAs = user.KnownAs,
              Gender = user.Gender
           };
        }
        //        [HttpGet("Login2")]
        // public async Task<ActionResult<UserDto>> Login2(){

        //     var user = await _context.Users
        //     .Include(p => p.Photos)
        //     .SingleOrDefaultAsync(x => x.UserName =="ehab");
        //     if(user == null) return Unauthorized("invalid Username");
        //     using var hmac = new HMACSHA512(user.PasswordSalt);
        //     var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Ehabehab"));
        //     for(int i=0;i<ComputeHash.Length;i++){
        //         if(ComputeHash[i] != user.PasswordHash[i])
        //         return Unauthorized("Invalid Password");
        //     }
        //     return new UserDto
        //    {
        //       Username = user.UserName,
        //       Token = _tokenService.CreateToken(user),
        //       PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        //       KnownAs = user.KnownAs,
        //       Gender = user.Gender
        //    };
        // } 

       private async Task<bool> UserExists(string username){
          return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());  
        }

    }
}