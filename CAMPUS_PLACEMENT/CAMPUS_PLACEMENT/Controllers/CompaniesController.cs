using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.Company;
using CampusPlacementManagement.Services.Interfaces;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CompanyDto dto)
        {
            var result = await _companyService.RegisterAsync(dto);

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
            return Ok(await _companyService.GetAllAsync());
        }

        [HttpGet("pending")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> GetPending()
        {
            return Ok(await _companyService.GetPendingAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin + "," + UserRoles.CompanyHR)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var isSuperAdmin = User.IsInRole(UserRoles.SuperAdmin);

            var company = await _companyService.GetByIdAsync(
                id, userId, isSuperAdmin);

            if (company == null)
            {
                return NotFound(new { message = "Company not found." });
            }

            return Ok(company);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = UserRoles.SuperAdmin)]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _companyService.ApproveAsync(id);

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
            var result = await _companyService.RejectAsync(id);

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
            var result = await _companyService.SuspendAsync(id);

            if (!result.Success)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.SuperAdmin + "," + UserRoles.CompanyHR)]
        public async Task<IActionResult> Update(int id, CompanyDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var isSuperAdmin = User.IsInRole(UserRoles.SuperAdmin);

            var result = await _companyService.UpdateAsync(
                id, userId, isSuperAdmin, dto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}