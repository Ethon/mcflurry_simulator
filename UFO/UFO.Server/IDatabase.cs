using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server {
    public interface IDatabase : IDisposable {
        DbCommand CreateCommand(string sql);
        int DeclareParamater(DbCommand command, string name, DbType type);
        void DefineParameter(DbCommand command, string name, DbType type, object value);
        void SetParameter(DbCommand command, string name, object value);

        IDataReader ExecuteReader(DbCommand command);
        int ExecuteNonQuery(DbCommand command);

        int TruncateTable(string tableName);

        object ConvertDateTimeToDbFormat(DateTime dt);
        DateTime ConvertDateTimeFromDbFormat(object obj);
    }
}
