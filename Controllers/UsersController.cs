using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    // [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;

        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService PhotoService)
        {
            _mapper = mapper;
            _photoService = PhotoService;
            _userRepository = userRepository;
        }
        // [Authorize (Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams){
            // var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var gender = await _userRepository.GetUserGender(User.GetUsername());
             userParams.CurrentUsername = User.GetUsername();
            if(string.IsNullOrEmpty(userParams.Gender))
                  userParams.Gender = gender == "male" ? "female" : "male";
                  
            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,
                           users.TotalCount,users.TotalPages);

           return Ok(users);
        
        }
        // [Authorize (Roles = "Member")]
        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){
            return await _userRepository.GetMemberAsync(username);
            
        
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
             var usre = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

             _mapper.Map(memberUpdateDto,usre);
             _userRepository.Update(usre);
             if(await _userRepository.SaveAllAsync())return NoContent();

             return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]

        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
           var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
           var result = await _photoService.AddPhotoAsync(file);
           if(result.Error != null) return BadRequest(result.Error.Message);
           var photo = new Photo
           {
              Url = result.SecureUrl.AbsoluteUri,
              PublicId = result.PublicId
           };
           if(user.Photos.Count == 0)
           {
            photo.IsMain = true;
           }
           user.Photos.Add(photo);

           if(await _userRepository.SaveAllAsync())
           {
             return CreatedAtRoute("GetUser",new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
           }
           return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var Photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(Photo.IsMain) return BadRequest("this is already your main photo");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
              Photo.IsMain = true;

              if(await _userRepository.SaveAllAsync()) return NoContent();

         return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("You cannot delete your main photo");
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletPhotoAsync(photo.PublicId);
                if(result.Error !=null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            
            if(await _userRepository.SaveAllAsync()) return Ok();
           return BadRequest("Failed to delete the photo");
        }


        // [HttpGet("Login2")]
        // public async Task<ActionResult<UserDto>> Login2()
        // {

        //     var user = await _context.Users
        //     .Include(p => p.Photos)
        //     .SingleOrDefaultAsync(x => x.UserName == "ehab");
        //     if (user == null) return Unauthorized("invalid Username");
        //     return new UserDto
        //     {
        //         Username = user.UserName,
        //         Token = await _tokenService.CreateToken(user),
        //         PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        //         KnownAs = user.KnownAs,
        //         Gender = user.Gender
        //     };
        // }
    }
}