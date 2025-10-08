using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant
{
    public static class connStr
    {
        public static string ConnectionString { get; } = @"host=localhost;
                                                           uid=root;
                                                           pwd=;
                                                           database=restaurant;";
    }
}
