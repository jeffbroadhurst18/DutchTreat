using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
		private readonly IDutchRepository _repository;
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<StoreUser> _signInManager;
		private readonly UserManager<StoreUser> _userManager;
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;

		public AccountController(IDutchRepository repository, ILogger<AccountController> logger,
			IMapper mapper, SignInManager<StoreUser> signInManager,
			UserManager<StoreUser> userManager,
			IConfiguration config)
		{
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
		}

		public IActionResult Login()
        {
			if (this.User.Identity.IsAuthenticated) //Gets the infor for the current user
			{
				RedirectToAction("Index", "App");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
					model.RememberMe, false);

				if (result.Succeeded)
				{
					if (Request.Query.Keys.Contains("ReturnUrl"))
					{
						Redirect(Request.Query["ReturnUrl"].First());
					}
					else
					{
						return RedirectToAction("Shop", "App");
					}
				}
			}

			ModelState.AddModelError("", "Failed to login");

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "App");
		}

		[HttpPost]
		public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.Username);
				if (user != null)
				{
					var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password, false);

					if (result.Succeeded)
					{
						// Create the token
						var claims = new[]
						{
							new Claim(JwtRegisteredClaimNames.Sub,user.Email),
							new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
							new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
						};

						var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
						var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
						var token = new JwtSecurityToken(
							_config["Tokens:Issuer"],
							_config["Tokens:Audience"], claims, expires: DateTime.Now.AddMinutes(20),
							signingCredentials:creds
							);

						var results = new
						{
							token = new JwtSecurityTokenHandler().WriteToken(token),
							SecurityTokenNoExpirationException = token.ValidTo
						};

						return Created("", results);
					}
				}
			}

			return BadRequest();
		}

    }
};