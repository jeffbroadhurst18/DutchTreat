using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
	public class DutchContext : IdentityDbContext<StoreUser>  //dbcontext which works with identity. Store User is where the identity stuff is.
	{
		//Passes the options parameter to the base class
		public DutchContext(DbContextOptions<DutchContext> options): base(options)
		{

		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
	}
}
