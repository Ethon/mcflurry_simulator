using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using UFO.Server;

namespace UFO.WebService {
    public class DatabaseConnection {

        private static IDatabase database;

        public static IDatabase GetDatabase() {
            if (database == null) {
                string providerName = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ProviderName;
                if (providerName == "Mysql.Data.MysqlClient") {
                    database = new MYSQLDatabase(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
                } else {
                    throw new NotSupportedException("Database not supported");
                }
            }

            return database;
        }
    }
    
}