using FleaApp_Api.Entities;
using FleaApp_Api.Interfaces;
using FleaApp_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FleaApp_Api.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager,
            IUnitOfWork uow,
            IPhotoService photoService)
        {
            _photoService = photoService;
            _uow = uow;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{userId}")]
        public async Task<ActionResult> EditRoles(int userId, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToList();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null) return BadRequest("No such user found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("disable-account/{userId}")]
        public async Task<ActionResult> DisableAccount(int userId, [FromQuery] string isDisabled)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null) return BadRequest("No such user is found");

            if (string.IsNullOrEmpty(isDisabled)) return BadRequest("You have not specified a option");

            if (isDisabled == "true") user.isDisabled = true;

            if (isDisabled == "false") user.isDisabled = false;

            await _userManager.UpdateAsync(user);

            if (await _uow.Complete()) return NoContent();

            return BadRequest();
        }
    }
}