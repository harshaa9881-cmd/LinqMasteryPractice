using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.College;
using CampusPlacementManagement.Services.Interfaces;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService _collegeService;

        public CollegesController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CollegeDto dto)
        {
            var result = await _collegeService.RegisterAsync(dto);

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
            return Ok(await _collegeService.GetAllAsync());
        }

        [HttpGet("pending")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> GetPending()
        {
            return Ok(await _collegeService.GetPendingAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin + "," + UserRoles.PlacementOfficer)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var isSuperAdmin = User.IsInRole(UserRoles.SuperAdmin);

            var college = await _collegeService.GetByIdAsync(
                id, userId, isSuperAdmin);

            if (college == null)
            {
                return NotFound(new { message = "College not found." });
            }

            return Ok(college);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _collegeService.ApproveAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _collegeService.RejectAsync(id);

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
            var result = await _collegeService.SuspendAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Update(int id, CollegeDto dto)
        {
            var result = await _collegeService.UpdateAsync(id, dto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}