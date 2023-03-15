using P01_HospitalDatabase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase
{
    public class StartUp
    {
        static void Main()
        {
            var db = new HospitalContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
