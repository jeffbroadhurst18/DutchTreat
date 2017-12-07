using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data.Entities
{
    public class StoreUser: IdentityUser
    {
		public int FirstName { get; set; }
		public int LastName { get; set; }
	}
}
