using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DutchTreat.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Controllers
{
    [Produces("application/json")]
    [Route("api/orders/{orderid}/items")]
    public class OrderItemsController : Controller
    {
		private readonly IDutchRepository _repository;
		private readonly ILogger<OrderItemsController> _logger;
		private readonly IMapper _mapper;

		public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Get(int orderId)
		{
			var order = _repository.GetOrderById(orderId); // This returns the child items from that id
			if (order != null)
			{ return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items)); }
			return NotFound();
		}

		[HttpGet("{id}")]
		public IActionResult Get(int orderId, int id)
		{
			var order = _repository.GetOrderById(orderId); // This returns the child items from that id
			if (order != null)
			{
				var item = order.Items.Where(c => c.Id == id).FirstOrDefault();
				return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
			}
			return NotFound();
		}
	}
}