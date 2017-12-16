using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.ViewModels;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
	public class AppController : Controller
	{
		private readonly IMailService _mailService;
		private readonly IDutchRepository _repository;

		public AppController(IMailService mailService, IDutchRepository repository)
		{
			_mailService = mailService;
			_repository = repository;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("contact")] //can access via /contact not /app/contact
		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost("contact")]
		public IActionResult Contact(ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				_mailService.SendMessage("jeffbroadhurst18@outlook.com", model.Subject, $"From: {model.Name} {model.Email}, Message: {model.Message}");
				ViewBag.UserMessage = "Mail has been sent";
				ModelState.Clear();
			}

			return View();
		}

		public IActionResult About()
		{
			ViewBag.Title = "About Us";
			return View();
		}

		[Authorize]
		public IActionResult Shop()
		{
			return View();
		}
	}
}