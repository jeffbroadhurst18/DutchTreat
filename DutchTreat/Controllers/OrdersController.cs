using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using Microsoft.Extensions.Logging;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using AutoMapper;

namespace DutchTreat.Controllers
{
	[Produces("application/json")]
	[Route("api/Orders")]
	public class OrdersController : Controller
	{
		private readonly IDutchRepository _repository;
		private readonly ILogger<OrdersController> _logger;
		private readonly IMapper _mapper;

		public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				var results = _repository.GetAllOrders(includeItems);
				return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
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
					return Ok(Mapper.Map<Order, OrderViewModel>(order));
				}
				return NotFound();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get a result {ex.Message}");
				return BadRequest("Bad Result"); //Returns a 400.
			}
		}


		[HttpPost]
		public IActionResult Post([FromBody]OrderViewModel model)
		{
			//add to db
			try
			{
				if (ModelState.IsValid)
				{
					var newOrder = _mapper.Map<OrderViewModel, Order>(model);

					if (newOrder.OrderDate == DateTime.MinValue)
					{
						newOrder.OrderDate = DateTime.Now;
					}

					_repository.AddEntity(newOrder);
					if (_repository.SaveAll())
					{
						var vm = _mapper.Map<Order, OrderViewModel>(newOrder);
						//model.id will be updated after save.
						return Created($"/api/orders/{vm.OrderId}", vm); //Return 201 code which is created
					}
				}
				else
				{
					return BadRequest(ModelState);
				}
			}
			catch(Exception ex)
			{
				_logger.LogError($"Failed to save new order - {ex.Message}");
			}
			return BadRequest("Post failed");
		}
	}
}