package swk5.ufo.web.service.rest;

import java.time.LocalDateTime;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.PerformanceModel;
import swk5.ufo.web.model.VenueModel;
import swk5.ufo.web.service.PerformanceService;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.ServiceConfig;
import swk5.ufo.web.service.util.HttpUtil;

public class RestPerformanceService implements PerformanceService {

	@Override
	public PerformanceModel createPerformance(LocalDateTime date, ArtistModel artist, VenueModel venue) {
		throw new UnsupportedOperationException();
	}

	@Override
	public void deletePerformance(PerformanceModel performance) throws ServiceCallException {
		throw new UnsupportedOperationException();
	}

	@Override
	public PerformanceModel[] getAllPerformances() throws ServiceCallException {
		return HttpUtil.getFromJsonService(ServiceConfig.PERFORMANCE_GETALL, PerformanceModel[].class);
	}

	@Override
	public PerformanceModel getPerformanceById(int id) throws ServiceCallException {
		final String url = String.format(ServiceConfig.PERFORMANCE_GETBYID, id);
		return HttpUtil.getFromJsonService(url, PerformanceModel.class);
	}

	@Override
	public PerformanceModel[] getPerformancesForDay(LocalDateTime date) throws ServiceCallException {
		final String dateString = String.format("%d-%d-%d", date.getYear(), date.getMonth().getValue(),
				date.getDayOfWeek().getValue());
		final String url = String.format(ServiceConfig.PERFORMANCE_GETFORDAY, dateString);
		return HttpUtil.getFromJsonService(url, PerformanceModel[].class);
	}

	@Override
	public void updatePerformance(PerformanceModel performance) throws ServiceCallException {
		throw new UnsupportedOperationException();
	}
}
