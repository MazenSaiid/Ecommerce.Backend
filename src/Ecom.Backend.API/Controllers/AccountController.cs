using AutoMapper;
using Ecom.Backend.API.Errors;
using Ecom.Backend.API.Extensions;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized(new CommonResponseError(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new CommonResponseError(401));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)

            });

        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (CheckExistingEmail(registerDto.Email).Result.Value)
            {
                return BadRequest(new ValidationError { Errors = new[]
                {
                    "This Email already exists"
                }
                });
            }
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email,
                Email = registerDto.Email,

            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(new CommonResponseError(400));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }
        [Authorize]
        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindEmailByClaimPrinciple(HttpContext.User);
            if (user is null)
                return NotFound(new CommonResponseError(404));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

        [HttpGet]
        [Route("CheckExistingEmail")]
        public async Task<ActionResult<bool>> CheckExistingEmail([FromQuery] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result is null ? false: true;
        }
        [Authorize]
        [HttpGet]
        [Route("GetUserAddress")]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimPrincipleWithAddress(HttpContext.User);
            if (user is null)
                return NotFound(new CommonResponseError(404));
            var result = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(result);
        }
        [Authorize]
        [HttpPut]
        [Route("UpdateUserAddress")]
        public async Task<IActionResult> UpdateUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindUserByClaimPrincipleWithAddress(HttpContext.User);
            user.Address = _mapper.Map<Address>(addressDto);
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
                return Ok(_mapper.Map<AddressDto>(user.Address));
            return BadRequest("Couldn't Update Address, Please Try Again");
        }
    }
}