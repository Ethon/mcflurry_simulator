using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    class Program {
        static void Main(string[] args) {
            IDatabase database;
            string providerName = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ProviderName;
            if (providerName == "Mysql.Data.MysqlClient") {
                database = new MYSQLDatabase(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
            } else {
                throw new NotSupportedException("Database not supported");
            }
            Console.ReadKey();
        }
    }
}
