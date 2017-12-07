using DutchTreat.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
	public class OrderItemViewModel
	{
		public int Id { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public decimal UnitPrice { get; set; }

		[Required]
		public int ProductId { get; set; }

		public string ProductCategory { get; set; }
		public string ProductSize { get; set; }
		public string ProductTitle { get; set; }
		public string ProductArtist { get; set; } //by prefixing with 'Product' we know they come from product entity
		public string ProductArtId { get; set; }
	}
}