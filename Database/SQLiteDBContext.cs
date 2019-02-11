using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Hudson.Database
{
    public class SQLiteDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            string DbLocation = Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.1", @"Data\Database.sqlite");
            Options.UseSqlite("Data Source=" + DbLocation);
        }
    }
}
