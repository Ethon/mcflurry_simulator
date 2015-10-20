using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server {
    class MYSQLDatabase : IDatabase {

        private string connectionString;

        public MYSQLDatabase(string connectionString) {
            this.connectionString = connectionString;
        }
        private DbConnection CreateOpenConnection() {
            DbConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public DbCommand CreateCommand(string sql) {
            
            return new MySqlCommand(sql);
        }


        protected DbConnection GetOpenConnection() {
            return CreateOpenConnection();
        }

        protected void ReleaseConnection(DbConnection connection) {
            if (connection != null) {
                connection.Close();
            }
        }

        protected bool IsSharedConnection() {
            return false;
        }

        

        public int DeclareParamater(DbCommand command, string name, DbType type) {
            if (!command.Parameters.Contains(name)) {
                string dbType = type.ToString();
                return command.Parameters.Add(new MySqlParameter(name, dbType));
            } else {
                throw new ArgumentException(string.Format("Parameter {0} already exists", name));
            }
        }

        public void DefineParameter(DbCommand command, string name, DbType type, object value) {
            
            int paramIndex = DeclareParamater(command, name, type);
            command.Parameters[paramIndex].Value = value;
        }

        public void SetParameter(DbCommand command, string name, object value) {
            if (command.Parameters.Contains(name)) {
                command.Parameters[name].Value = value;
            } else {
                throw new ArgumentException(string.Format("Can`t find parameter {0}", name));
            }
        }

        public IDataReader ExecuteReader(DbCommand command) {
            DbConnection conn = null;
            try {
                conn = GetOpenConnection();
                command.Connection = conn;
                CommandBehavior commandBehavior = IsSharedConnection() ? CommandBehavior.Default : CommandBehavior.CloseConnection;
                return command.ExecuteReader(commandBehavior);
            } catch {
                ReleaseConnection(conn);
                throw;
            }
        }

        public int ExecuteNonQuery(DbCommand command) {
            DbConnection conn = null;
            MySqlCommand mysqlCommand = (MySqlCommand)command;
            try {
                conn = GetOpenConnection();
                command.Connection = conn;
                int executeResult = command.ExecuteNonQuery();
                if ((int)mysqlCommand.LastInsertedId > 0) {
                    return (int)mysqlCommand.LastInsertedId;
                } else {
                    return executeResult;
                }
            } finally {
                ReleaseConnection(conn);
            }
        }

    }
}
