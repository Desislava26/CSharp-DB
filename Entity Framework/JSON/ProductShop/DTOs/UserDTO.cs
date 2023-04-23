using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? Age { get; set; }
    }
}
// SqlException: The MERGE statement conflicted with the FOREIGN KEY constraint "FK_Products_Users_BuyerId".
// The conflict occurred in database "ProductShop", table "dbo.Users", column 'Id'.
//The statement has been terminated.