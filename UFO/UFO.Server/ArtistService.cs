﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public interface IArtistService {
        Artist CreateArtist(string name, string email, Category category, Country country, string picturePath, string videoPath);
        Artist GetArtistById(uint id);
        List<Artist> GetAllArtists();
        void UpdateArtist(Artist artist);
        void DeleteArtist(Artist artist);
    }

    internal class ArtistService : IArtistService {
        private IArtistDao aDao;
        private IPerformanceDao pDao;

        private static Regex nameRegex = new Regex("^[a-zA-Z_öÖäÄüÜß& ]*$");
        private static Regex emailRegex = new Regex("\\w+@\\w+.\\w+");

        private static bool IsValidName(string name) {
            return name.Length > 0;
        }

        private static bool IsValidEmail(string email) {
            return emailRegex.IsMatch(email);
        }

        private static bool IsValidPicturePath(string picturePath) {
            return true;
        }

        private static bool IsValidVideoPath(string videoPath) {
            return true;
        }


        public ArtistService(IDatabase db) {
            if(db is MYSQLDatabase) { 
                aDao = new ArtistDao(db);
                pDao = new PerformanceDao(db);
            } else {
                throw new NotSupportedException("Database not supported");
            }
        }

        public Artist CreateArtist(string name, string email, Category category, Country country, string picturePath, string videoPath) {
            if (!IsValidName(name)) {
                throw new DataValidationException("Can't create artist with invalid name '" + name + "'");
            } else if (!IsValidEmail(email)) {
                throw new DataValidationException("Can't create artist with invalid emails '" + email + "'");
            } else if (!IsValidPicturePath(picturePath)) {
                throw new DataValidationException("Can`t create artist with invalid picturePath '" + picturePath + "'");
            } else if (!IsValidVideoPath(videoPath)) {
                throw new DataValidationException("Can`t create artist with invalid videoPath '" + videoPath + "'");
            }
            
            return aDao.CreateArtist(name,email,category.Id,country.Id,picturePath,videoPath);
        }

        public Artist GetArtistById(uint id) {
            return aDao.GetArtistById(id);
        }
        
        public List<Artist> GetAllArtists() {
            return aDao.GetAllArtists();
        }

        public void UpdateArtist(Artist artist) {
            if (!IsValidName(artist.Name)) {
                throw new DataValidationException("Can't update artist to invalid name '" + artist.Name + "'");
            }
            if (!IsValidEmail(artist.Email)) {
                throw new DataValidationException("Can't update artist to invalid email '" + artist.Email + "'");
            }
            if (!IsValidPicturePath(artist.PicturePath)) {
                throw new DataValidationException("Can`t update artist with invalid picturePath '" + artist.PicturePath + "'");
            }
            if (!IsValidVideoPath(artist.VideoPath)) {
                throw new DataValidationException("Can`t update artist with invalid videoPath '" + artist.VideoPath + "'");
            }
            if (!aDao.UpdateArtist(artist)) {
                throw new DatabaseException("Can`t update artist with invalid ID: '" + artist + "'");
            }
        }

        public void DeleteArtist(Artist artist) {
            var beforeNow = pDao.GetPerformancesByArtistBeforeDate(artist.Id, DateTime.Now);
            var afterNow = pDao.GetPerformancesByArtistAfterDate(artist.Id, DateTime.Now);
            if (beforeNow.Count >= 0) {
                if (!aDao.MarkAsDeleted(artist)) {
                    throw new DatabaseException("Can`t delete artist with invalid ID: '" + artist + "'");
                }
            } else {
                if (!aDao.DeleteArtist(artist)) {
                    throw new DatabaseException("Can`t delete artist with invalid ID: '" + artist + "'");
                }
            }
            foreach (var item in afterNow) {
                pDao.DeletePerformance(item);
            }
        }
    }
}
