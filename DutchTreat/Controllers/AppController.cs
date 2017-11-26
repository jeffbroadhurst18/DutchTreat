using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
	public class AppController : Controller
	{
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
				_mailService.SendMail("jeffbroadhurst18@outlook.com", model.Subject, $"From: {model.Name} {model.Email}, Message: {model.Message}");
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