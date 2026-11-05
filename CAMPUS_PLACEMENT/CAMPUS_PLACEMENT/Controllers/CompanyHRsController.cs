using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.CompanyHR;
using CampusPlacementManagement.Services.Interfaces;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyHRsController : ControllerBase
    {
        private readonly ICompanyHRService _companyHRService;

        public CompanyHRsController(ICompanyHRService companyHRService)
        {
            _companyHRService = companyHRService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            CompanyHRRegisterDto dto)
        {
            var result = await _companyHRService.RegisterAsync(dto);

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
            return Ok(await _companyHRService.GetAllAsync());
        }

        [HttpGet("pending")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> GetPending()
        {
            return Ok(await _companyHRService.GetPendingAsync());
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Approve(int id)
        {
            var approvedBy = User.FindFirstValue(
                ClaimTypes.NameIdentifier)!;

            var result = await _companyHRService.ApproveAsync(
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
            var result = await _companyHRService.RejectAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpGet("me")]
        [Authorize(Roles = UserRoles.CompanyHR)]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier)!;

            var companyHR = await _companyHRService.GetMeAsync(userId);

            if (companyHR == null)
            {
                return NotFound(new
                {
                    message = "Company HR profile not found."
                });
            }

            return Ok(companyHR);
        }
    }
}