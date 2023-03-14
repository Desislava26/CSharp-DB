using P02_FootballBetting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting
{
    public class StartUp
    {
        static void Main()
        {
            var db = new FootballBettingContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
