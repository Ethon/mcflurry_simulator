package swk5.ufo.web.service;

import java.time.LocalDateTime;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.PerformanceModel;
import swk5.ufo.web.model.VenueModel;

public interface PerformanceService {
	public PerformanceModel createPerformance(LocalDateTime date, ArtistModel artist, VenueModel venue);

	public void deletePerformance(PerformanceModel performance) throws ServiceCallException;

	public PerformanceModel[] getAllPerformances() throws ServiceCallException;

	public PerformanceModel getPerformanceById(int id) throws ServiceCallException;

	public PerformanceModel[] getPerformancesForDay(LocalDateTime date) throws ServiceCallException;

	public void updatePerformance(PerformanceModel performance) throws ServiceCallException;
}
