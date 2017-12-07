using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
	public class DutchRepository : IDutchRepository
	{
		private readonly DutchContext _ctx;
		private readonly ILogger<DutchRepository> _logger;

		public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
		{
			_ctx = ctx;
			_logger = logger;
		}

		public void AddEntity(object model)
		{
			_ctx.Add(model); //The context can work out which type the object is and save it appropriately.
		}

		public IEnumerable<Order> GetAllOrders(bool includeItems)
		{
			if (includeItems)
			{
				var result = _ctx.Orders.Include(c => c.Items).ThenInclude(v => v.Product).ToList();
				return result;
				//includes data from join
			}
			else
			{
				var result = _ctx.Orders.ToList();
				return result;
			}
		}

		public IEnumerable<Product> GetAllProducts()
		{
			try
			{
				_logger.LogInformation("GetAllProducts");
				var products = _ctx.Products.OrderBy(c => c.Title).ToList();
				return products;
			}
			catch(Exception ex)
			{
				_logger.LogError("Failed to get all products: {0} {1}", ex.Message, ex.StackTrace);
				return null;
			}
		}

		public Order GetOrderById(int id)
		{
			try
			{
				return _ctx.Orders.Include(b => b.Items)
					.ThenInclude(r => r.Product).Where(o => o.Id == id)
					.FirstOrDefault();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get order: {ex.Message} {ex.StackTrace}");
				return null;
			}
		}

		public IEnumerable<Product> GetProductsByCategory(string category)
		{
			var products = _ctx.Products.Where(c => c.Category == category)
							.OrderBy(p => p.Title).ToList();
			return products;
		}

		public bool SaveAll()
		{
			return _ctx.SaveChanges() > 0;
		}
    }
}
