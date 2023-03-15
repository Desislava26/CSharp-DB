using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        static void Main()
        {
            var db = new SalesContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        
    }
}
