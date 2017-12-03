using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
	[Produces("application/json")]
	[Route("api/Orders")]
	public class OrdersController : Controller
	{
		private readonly IDutchRepository _repository;
		private readonly ILogger<OrdersController> _logger;

		public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				return Ok(_repository.GetAllOrders());
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}

		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			try
			{
				var order = _repository.GetOrderById(id);
				if (order != null)
				{
					return Ok(order);
				}
				return NotFound();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}
	}
}