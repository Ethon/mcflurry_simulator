﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Server.Data {
    [Serializable]
    public class Artist {
        public Artist(uint id,string name, string email, uint categoryId, uint countryId, string picturePath, string videoPath) {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.CategoryId = categoryId;
            this.CountryId = countryId;
            this.PicturePath = picturePath;
            this.VideoPath = videoPath;
        }

        public uint Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public string Email {
            get; set;
        }

        public uint CategoryId {
            get; set;
        }

        public uint CountryId {
            get; set;
        }

        public string PicturePath {
            get; set;
        }

        public string VideoPath {
            get; set;
        }

        public override string ToString() {
            return String.Format("Artist(id={0}, name='{1}', email='{2}',categoryId='{3}',countryId='{4}',picturePath='{5}',videoPath='{6}) ", Id, Name, Email,CategoryId,CountryId,PicturePath,VideoPath);
        }

        public override bool Equals(object obj) {
            Artist a = obj as Artist;
            if (a == null) {
                return false;
            }
            return Id.Equals(a.Id) && Name.Equals(a.Name) && Email.Equals(a.Email) && CategoryId.Equals(a.CategoryId) && CountryId.Equals(a.CountryId) && PicturePath.Equals(a.PicturePath) && VideoPath.Equals(a.VideoPath);
        }

        public override int GetHashCode() {
            return (int)Id;
        }

    }
}

public interface IArtistDao {
    List<Artist> GetAllArtists();
    Artist GetArtistById(uint id);
    Artist GetArtistByName(string name);
    bool UpdateArtist(Artist artist);
    bool DeleteArtist(Artist artist);
    Artist CreateArtist(string name,string email,uint categoryId, uint countryId,string picturePath,string videoPath);
    void DeleteAllArtists();
}

public class ArtistDao : IArtistDao {

    private const string SQL_FIND_BY_ID = "SELECT * FROM Artist WHERE artistId=@artistId";
    private const string SQL_FIND_BY_NAME = "SELECT * FROM Artist WHERE name LIKE @name";
    private const string SQL_FIND_ALL = "SELECT * FROM Artist";
    private const string SQL_UPDATE = "UPDATE Artist SET name=@name,email=@email,categoryId=@categoryId,countryId=@countryId,picturePath=@picturePath,videoPath=@videoPath WHERE artistId=@artistId";
    private const string SQL_INSERT = "INSERT INTO Artist (name,email,categoryId,countryId,picturePath,videoPath) VALUES(@name,@email,@categoryId,@countryId,@picturePath,@videoPath)";
    private const string SQL_DELETE = "DELETE FROM Artist WHERE artistId=@artistId";

    private IDatabase database;

    public ArtistDao(IDatabase database) {
        this.database = database;
    }

    public Artist CreateArtist(string name, string email, uint categoryId, uint countryId, string picturePath, string videoPath) {
        DbCommand cmd = database.CreateCommand(SQL_INSERT);
        database.DefineParameter(cmd, "@name", DbType.String, name);
        database.DefineParameter(cmd, "@email", DbType.String, email);
        database.DefineParameter(cmd, "@categoryId", DbType.UInt32, categoryId);
        database.DefineParameter(cmd, "@countryId", DbType.UInt32, countryId);
        database.DefineParameter(cmd, "@picturePath", DbType.String, picturePath);
        database.DefineParameter(cmd, "@videoPath", DbType.String, videoPath);
        int lastInsertId = database.ExecuteNonQuery(cmd);
        return new Artist((uint)lastInsertId, name, email, categoryId, countryId, picturePath, videoPath);
    }

    public bool DeleteArtist(Artist artist) {
        if(artist != null) { 
            DbCommand cmd = database.CreateCommand(SQL_DELETE);
            database.DefineParameter(cmd, "@artistId", DbType.String, artist.Id);
            return database.ExecuteNonQuery(cmd) == 1;
        } else {
            return false;
        }
    }

    public List<Artist> GetAllArtists() {
        List<Artist> artists = new List<Artist>();
        DbCommand cmd = database.CreateCommand(SQL_FIND_ALL);
        using (IDataReader reader = database.ExecuteReader(cmd)) {
            while (reader.Read()) {
                Artist newArtist = new Artist((uint)reader["artistId"], (string)reader["name"], (string)reader["email"], (uint)reader["categoryId"], (uint)reader["countryId"], (string)reader["picturePath"], (string)reader["videoPath"]);
                artists.Add(newArtist);
            }
            return artists;
        }
    }

    public Artist GetArtistById(uint id) {
        DbCommand cmd = database.CreateCommand(SQL_FIND_BY_ID);
        database.DefineParameter(cmd, "@artistId", DbType.UInt32, id);
        using (IDataReader reader = database.ExecuteReader(cmd)) {
            if (reader.Read()) {
                return new Artist((uint)reader["artistId"], (string)reader["name"], (string)reader["email"], (uint)reader["categoryId"], (uint)reader["countryId"], (string)reader["picturePath"], (string)reader["videoPath"]);
            } else {
                return null;
            }
        }
    }

    public Artist GetArtistByName(string name) {
        
        DbCommand cmd = database.CreateCommand(SQL_FIND_BY_NAME);
        database.DefineParameter(cmd, "@name", DbType.String, name);
        using (IDataReader reader = database.ExecuteReader(cmd)) {
            if (reader.Read()) {
                return new Artist((uint)reader["artistId"], (string)reader["name"], (string)reader["email"], (uint)reader["categoryId"], (uint)reader["countryId"], (string)reader["picturePath"], (string)reader["videoPath"]);
            } else {
                return null;
            }
        }
    }

    public bool UpdateArtist(Artist artist) {
        if (artist != null) {
            DbCommand cmd = database.CreateCommand(SQL_UPDATE);
            database.DefineParameter(cmd, "@name", DbType.String, artist.Name);
            database.DefineParameter(cmd, "@email", DbType.String, artist.Email);
            database.DefineParameter(cmd, "@categoryId", DbType.UInt32, artist.CategoryId);
            database.DefineParameter(cmd, "@countryId", DbType.UInt32, artist.CountryId);
            database.DefineParameter(cmd, "@picturePath", DbType.String, artist.PicturePath);
            database.DefineParameter(cmd, "@videoPath", DbType.String, artist.VideoPath);
            database.DefineParameter(cmd, "@artistId", DbType.String, artist.Id);
            return database.ExecuteNonQuery(cmd) == 1;
        }
        return false;
    }

    public void DeleteAllArtists() {
        database.TruncateTable("Artist");
    }
    
}