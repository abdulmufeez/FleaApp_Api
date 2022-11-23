using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            DataContext context, IPhotoService photoService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _photoService = photoService;
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromForm]UserRegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is already taken");
            if (await UserEmailExists(registerDto.Email)) return BadRequest("Email is already taken");

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                Email = registerDto.Email
            };     

            if (registerDto.Photo is not null)
            {
                var photoResult = await _photoService.AddPhotoAsync(registerDto.Photo);

                if (photoResult.Error is not null) return BadRequest(photoResult.Error.Message);

                var photo = new Photo
                {
                    Url = photoResult.SecureUrl.AbsoluteUri,
                    PublicId = photoResult.PublicId,
                    IsMain = true
                };
                _context.Photos.Add(photo);
                if (await _context.SaveChangesAsync() > 0)
                {
                    user.ProfilePhoto = photo;
                    user.ProfilePhotoId = photo.Id;
                }
            }       

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var userRole = "ShopKeeper";
            if (registerDto.isUser)
                userRole = "User";

            var roleResult = await _userManager.AddToRoleAsync(user, userRole);

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);            

            return Ok(new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                PhotoUrl = user.ProfilePhoto is not null ? user.ProfilePhoto.Url : null,
                Token = await _tokenService.CreateTokenAsync(user)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto loginDto)
        {
            var user = await _userManager.Users.Include(user => user.ProfilePhoto)
                .SingleOrDefaultAsync(user => user.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid Username");

            if(user.isDisabled) return BadRequest("Your account has been disabled ;)");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid password");

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                PhotoUrl = user.ProfilePhoto is not null ? user.ProfilePhoto.Url : null,
                Token = await _tokenService.CreateTokenAsync(user)
            };

            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("update-photo")]
        public async Task<ActionResult<PhotoDto>> UpdatePhoto([FromForm] CreatePhotoDto photoDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == photoDto.Id);

            if (user.ProfilePhoto.PublicId != null)
            {
                var photoResult = await _photoService.DeletePhotoAsync(user.ProfilePhoto.PublicId);
                if (photoResult.Error != null) return BadRequest(photoResult.Error.Message);
            }

            _context.Photos.Remove(user.ProfilePhoto);
            
            if (await _context.SaveChangesAsync() > 0)
            {
                var updateResult = await _photoService.AddPhotoAsync(photoDto.Photo);

                if (updateResult is not null) return BadRequest(updateResult.Error.Message);

                var photo = new Photo
                {
                    Url = updateResult.SecureUrl.AbsoluteUri,
                    PublicId = updateResult.PublicId,
                    IsMain = true
                };

                user.ProfilePhoto = photo;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return Ok(new UserDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        PhotoUrl = user.ProfilePhoto.Url,
                        Token = null
                    });
            }

            return BadRequest("Problem Adding Photo");
        }

        [Authorize]
        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto(DeletePhotoDto photoDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == photoDto.Id);

            if (user.ProfilePhoto.PublicId != null)
            {
                var photoResult = await _photoService.DeletePhotoAsync(user.ProfilePhoto.PublicId);
                if (photoResult.Error != null) return BadRequest(photoResult.Error.Message);
            }
            user.ProfilePhoto = null;
            user.ProfilePhotoId = null;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok("Successfully deleted photo");

            return BadRequest("Error deleting the photo");
        }


        //checking if username is already exist
        private async Task<bool> UserExists(string username)
            => await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());

        private async Task<bool> UserEmailExists(string email)
            => await _userManager.Users.AnyAsync(user => user.Email == email);
    }
}