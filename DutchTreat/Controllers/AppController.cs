using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.ViewModels;
using DutchTreat.Services;

namespace DutchTreat.Controllers
{
		public class AppController : Controller
	{
		private readonly IMailService _mailService;

		public AppController(IMailService mailService)
		{
			_mailService = mailService;
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
	}
}