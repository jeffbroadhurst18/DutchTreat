using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
	public interface IDutchRepository
	{
		IEnumerable<Product> GetAllProducts();
		IEnumerable<Product> GetProductsByCategory(string category);
		bool SaveAll();
		IEnumerable<Order> GetAllOrders(bool includeItems);
		Order GetOrderById(int id);
		void AddEntity(object model);
		IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
		Order GetOrderById(string name, int orderId);
		void AddOrder(Order newOrder);
	}
}