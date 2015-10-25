using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    [Serializable]
    public class District {
        public District(uint id, string name) {
            Id = id;
            Name = name;
        }

        public uint Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public override string ToString() {
            return String.Format("District(id={0}, name='{1}')", Id, Name);
        }

        public override bool Equals(object obj) {
            District dis = obj as District;
            if(dis == null) {
                return false;
            }
            return Id.Equals(dis.Id) && Name.Equals(dis.Name);
        }

        public override int GetHashCode() {
            return (int)Id;
        }
    }

    public interface IDistrictDao {
        void DeleteAllDistricts();
        List<District> GetAllDistricts();
        District GetDistrictById(uint id);
        void UpdateDistrict(District dis);
        void DeleteDistrict(District dis);
        District CreateDistrict(string name);
    }

    public class DistrictDao : IDistrictDao {
        private const string DELETEALL_CMD = "TRUNCATE TABLE District";
        private const string CREATE_CMD = "INSERT INTO District(name) VALUES (@name)";
        private const string DELETE_CMD = "DELETE FROM District WHERE districtId = @id";
        private const string GETALL_CMD = "SELECT * FROM District";
        private const string GETBYID_CMD = "SELECT * FROM District WHERE districtId = @id";
        private const string UPDATE_CMD = "UPDATE District SET name=@name WHERE districtId=@id";

        private IDatabase db;

        private District readOne(DbDataReader reader) {
            uint id = (uint)reader["districtId"];
            string name = (string)reader["name"];
            return new District(id, name);
        }

        public DistrictDao(IDatabase db) {
            this.db = db;
        }

        public District CreateDistrict(string name) {
            DbCommand cmd = db.CreateCommand(CREATE_CMD);
            db.DefineParameter(cmd, "@name", System.Data.DbType.String, name);
            uint id = (uint)db.ExecuteNonQuery(cmd);
            return new District(id, name);
        }

        public void DeleteAllDistricts() {
            DbCommand cmd = db.CreateCommand(DELETEALL_CMD);
            db.ExecuteNonQuery(cmd);
        }

        public void DeleteDistrict(District dis) {
            DbCommand cmd = db.CreateCommand(DELETE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, dis.Id);
            cmd.ExecuteNonQuery();
        }

        public List<District> GetAllDistricts() {
            List<District> districts = new List<District>();
            DbCommand cmd = db.CreateCommand(GETALL_CMD);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                while (reader.Read()) {
                    districts.Add(readOne(reader));
                }
            }
            return districts;
        }

        public District GetDistrictById(uint id) {
            DbCommand cmd = db.CreateCommand(GETBYID_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, id);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return readOne(reader);
                } else {
                    return null;
                }
            }
        }

        public void UpdateDistrict(District dis) {
            DbCommand cmd = db.CreateCommand(UPDATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, dis.Id);
            db.DefineParameter(cmd, "@name", System.Data.DbType.String, dis.Name);
            db.ExecuteNonQuery(cmd);
        }
    }
}
