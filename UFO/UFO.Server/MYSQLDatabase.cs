using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server {
    public sealed class MYSQLDatabase : IDatabase {

        private MySqlConnection con;

        public MYSQLDatabase(string connectionString) {
            con = new MySqlConnection(connectionString);
            con.Open();
        }

        public DbCommand CreateCommand(string sql) {
            return new MySqlCommand(sql, con);
        }

        private DbConnection GetOpenConnection() {
            return con;
        }

        private void ReleaseConnection() {
            if (con != null) {
                con.Close();
                con = null;
            }
        }

        private bool IsSharedConnection() {
            return true;
        }

        public int DeclareParamater(DbCommand command, string name, DbType type) {
            if (!command.Parameters.Contains(name)) {
                string dbType = type.ToString();
                if(type == DbType.DateTime) {
                    return command.Parameters.Add(new MySqlParameter(name, MySqlDbType.DateTime));
                } 
                else if(type == DbType.Double) {
                    return command.Parameters.Add(new MySqlParameter(name, MySqlDbType.Double));
                }
                else { 
                    return command.Parameters.Add(new MySqlParameter(name, dbType));
                }
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
            DbConnection conn = GetOpenConnection();
            command.Connection = conn;
            CommandBehavior commandBehavior = IsSharedConnection() ? CommandBehavior.Default : CommandBehavior.CloseConnection;
            return command.ExecuteReader(commandBehavior);
        }

        public int ExecuteNonQuery(DbCommand command) {
            DbConnection conn = GetOpenConnection();
            MySqlCommand mysqlCommand = (MySqlCommand)command;
            command.Connection = conn;
            int executeResult = command.ExecuteNonQuery();
            if ((int)mysqlCommand.LastInsertedId > 0) {
                return (int)mysqlCommand.LastInsertedId;
            } else {
                return executeResult;
            }
        }

        public void Dispose() {
            ReleaseConnection();
        }

        public object ConvertDateTimeToDbFormat(DateTime dt) {
            return new MySqlDateTime(dt);
        } 

        public DateTime ConvertDateTimeFromDbFormat(object obj) {
            if(obj is DateTime) {
                return (DateTime)obj;
            }
            MySqlDateTime dt = (MySqlDateTime)obj;
            return dt.GetDateTime();
        }

        public int TruncateTable(string tableName) {
            return CreateCommand("SET FOREIGN_KEY_CHECKS = 0; TRUNCATE " +
                tableName + "; SET FOREIGN_KEY_CHECKS = 1;").ExecuteNonQuery();
        }
    }
}
