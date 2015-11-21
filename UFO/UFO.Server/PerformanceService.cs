﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public interface IPerformanceService {
        Performance CreatePerformance(DateTime date, Artist artist, Venue venue);
        List<Performance> GetAllPerformances();
        Performance GetPerformanceById(uint id);
        void UpdatePerformance(Performance performance);
        void DeletePerformance(Performance performance);
    }

    internal class PerformanceService : IPerformanceService {
        private IPerformanceDao pdao;

        private static bool IsValidTime(DateTime date) {
            return date.Minute == 0 && date.Second == 0 && date.Millisecond == 0;
        }

        private static bool IsValidDate(DateTime date) {
            return date.Year >= DateTime.Now.Year && date.Year < DateTime.Now.Year + 10;
        }

        private bool IsVenueTakenAtTime(DateTime date, uint venueId) {
            return pdao.GetPerformanceByVenueAndDate(venueId, date) != null;
        }

        public PerformanceService(IDatabase db) {
            if (db is MYSQLDatabase) {
                pdao = new PerformanceDao(db);
            } else {
                throw new NotSupportedException("Database not supported");
            }
        }

        public Performance CreatePerformance(DateTime date, Artist artist, Venue venue) {
            if(!IsValidTime(date)) {
                throw new DataValidationException("Can't create performance at invalid time '" + date.ToLongTimeString() + "'");
            }
            if(!IsValidDate(date)) {
                throw new DataValidationException("Can't create performance at invalid date '" + date.ToLongDateString() + "'");
            }
            if(IsVenueTakenAtTime(date, venue.Id)) {
                throw new DataValidationException("Can't create performance at venue with another performance at the same time");
            }
            return pdao.CreatePerformance(date, artist.Id, venue.Id);
        }

        public List<Performance> GetAllPerformances() {
            return pdao.GetAllPerformances();
        }

        public Performance GetPerformanceById(uint id) {
            return pdao.GetPerformanceById(id);
        }

        public void UpdatePerformance(Performance performance) {
            if (!IsValidTime(performance.Date)) {
                throw new DataValidationException("Can't update performance to invalid time '" + performance.Date.ToLongTimeString() + "'");
            }
            if (!IsValidDate(performance.Date)) {
                throw new DataValidationException("Can't update performance to invalid date '" + performance.Date.ToLongDateString() + "'");
            }
            if (IsVenueTakenAtTime(performance.Date, performance.VenueId)) {
                throw new DataValidationException("Can't update performance to venue with another performance at the same time");
            }
            if (!pdao.UpdatePerformance(performance)) {
                throw new DatabaseException("Can`t update performance with invalid ID: '" + performance + "'");
            }
        }

        public void DeletePerformance(Performance performance) {
            if (!pdao.DeletePerformance(performance)) {
                throw new DatabaseException("Can`t delete performance with invalid ID: '" + performance + "'");
            }
        }
    }
}
