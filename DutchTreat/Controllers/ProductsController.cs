using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using Microsoft.Extensions.Logging;
using DutchTreat.Data.Entities;

namespace DutchTreat.Controllers
{
	[Produces("application/json")]
	[Route("api/Products")]
	public class ProductsController : Controller
	{
		private readonly IDutchRepository _repository;
		private readonly ILogger<ProductsController> _logger;

		public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
		{
			_logger = logger;
			_repository = repository;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				return Ok(_repository.GetAllProducts());
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}
	}
}
