using DutchTreat.Data.Entities;
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
