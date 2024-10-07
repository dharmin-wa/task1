using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user_GDPR.Models
{
	public class Users
    {
		public ObjectId Id { get; set; }
		public string FirstName { get; set; }
		public string lastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string MobileNo { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsDeleted { get; set; }
        public bool IsUserConsent { get; set; }
        public DateTime CreatedAt { get; set; }
	}
}
