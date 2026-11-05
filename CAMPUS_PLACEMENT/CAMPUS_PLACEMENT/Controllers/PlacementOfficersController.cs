using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.PlacementOfficer;
using CampusPlacementManagement.Services.Interfaces;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlacementOfficersController : ControllerBase
    {
        private readonly IPlacementOfficerService _officerService;

        public PlacementOfficersController(
            IPlacementOfficerService officerService)
        {
            _officerService = officerService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            PlacementOfficerRegisterDto dto)
        {
            var result = await _officerService.RegisterAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _officerService.GetAllAsync());
        }

        [HttpGet("pending")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> GetPending()
        {
            return Ok(await _officerService.GetPendingAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin + "," + UserRoles.PlacementOfficer)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var isSuperAdmin = User.IsInRole(UserRoles.SuperAdmin);

            var officer = await _officerService.GetByIdAsync(
                id, userId, isSuperAdmin);

            if (officer == null)
            {
                return NotFound(new
                {
                    message = "Placement officer not found."
                });
            }

            return Ok(officer);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Approve(int id)
        {
            var approvedBy = User.FindFirstValue(
                ClaimTypes.NameIdentifier)!;

            var result = await _officerService.ApproveAsync(
                id, approvedBy);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _officerService.RejectAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}/suspend")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Suspend(int id)
        {
            var result = await _officerService.SuspendAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpGet("me")]
        [Authorize(Roles = UserRoles.PlacementOfficer)]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier)!;

            var officer = await _officerService.GetMeAsync(userId);

            if (officer == null)
            {
                return NotFound(new
                {
                    message = "Placement officer profile not found."
                });
            }

            return Ok(officer);
        }
    }
}