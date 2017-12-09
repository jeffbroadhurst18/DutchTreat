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

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
		private readonly IDutchRepository _repository;
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<StoreUser> _signInManager;
		private readonly IMapper _mapper;

		public AccountController(IDutchRepository repository, ILogger<AccountController> logger,
			IMapper mapper, SignInManager<StoreUser> signInManager)
		{
			_logger = logger;
			_signInManager = signInManager;
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
					if (Request.Query.Keys.Contains("ReturnUrl")){
						Redirect(Request.Query["ReturnUrl"].First());
					}
					else
					{
						RedirectToAction("Shop", "App");
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

    }
}