package swk5.ufo.web.service;

import java.time.LocalDateTime;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.PerformanceModel;
import swk5.ufo.web.model.VenueModel;

public interface PerformanceService {
	public PerformanceModel CreatePerformance(LocalDateTime date, ArtistModel artist, VenueModel venue);

	public void DeletePerformance(PerformanceModel performance) throws ServiceCallException;

	public PerformanceModel[] getAllPerformances() throws ServiceCallException;

	public PerformanceModel getPerformanceById(int id) throws ServiceCallException;

	public void GetPerformancesForDay(LocalDateTime date);

	public void UpdatePerformance(PerformanceModel performance) throws ServiceCallException;
}
