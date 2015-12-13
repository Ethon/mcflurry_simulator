using System;
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
        List<Performance> GetPerformancesForDay(DateTime date);
    }

    internal class PerformanceService : IPerformanceService {
        private const uint LEGAL_HOUR_DELTA = 2;

        private IPerformanceDao pdao;

        private static bool IsValidTime(DateTime date) {
            return date.Minute == 0 && date.Second == 0 && date.Millisecond == 0;
        }

        private static bool IsValidDate(DateTime date) {
            return date.Year >= DateTime.Now.Year && date.Year < DateTime.Now.Year + 10;
        }

        private bool IsVenueTakenAtTime(DateTime date, uint venueId, uint exclude = 0) {
            Performance perf = pdao.GetPerformanceByVenueAndDate(venueId, date);
            return perf != null && perf.Id != exclude;
        }

        private bool IsArtistAllowedToPlay(DateTime date, uint artistId) {
            var performancesBefore = pdao.GetPerformancesByArtistBeforeDate(artistId, date);
            foreach (var per in performancesBefore) {
                TimeSpan curDelta = date.Subtract(per.Date);
                if(curDelta.TotalHours < LEGAL_HOUR_DELTA) {
                    return false;
                }
            }
            return true;
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
            if(!IsArtistAllowedToPlay(date, artist.Id)) {
                throw new DataValidationException("Can't create performance at time artist is not allowed to play again");
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
            if (IsVenueTakenAtTime(performance.Date, performance.VenueId, performance.Id)) {
                throw new DataValidationException("Can't update performance to venue with another performance at the same time");
            }
            if (!IsArtistAllowedToPlay(performance.Date, performance.ArtistId)) {
                throw new DataValidationException("Can't create performance at time artist is not allowed to play again");
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

        public List<Performance> GetPerformancesForDay(DateTime date) {
            return pdao.GetPerformancesForDay(date);
        }
    }
}
