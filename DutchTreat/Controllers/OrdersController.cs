﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Controllers
{
	[Produces("application/json")]
	[Route("api/[Controller]")]
	[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
	public class OrdersController : Controller
	{
		private readonly IDutchRepository _repository;
		private readonly ILogger<OrdersController> _logger;
		private readonly IMapper _mapper;
		private readonly UserManager<StoreUser> _userManager;

		public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper,
			UserManager<StoreUser> userManager)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				var username = User.Identity.Name;

				var results = _repository.GetAllOrdersByUser(username,includeItems);
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
				var order = _repository.GetOrderById(User.Identity.Name, id);
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
		public async Task<IActionResult> Post([FromBody]OrderViewModel model)
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
					// User = list of claims from token. Convertes this to a store user
					var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
					newOrder.User = currentUser;

					//_repository.AddEntity(newOrder);
					_repository.AddOrder(newOrder);
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