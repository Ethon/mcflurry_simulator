package swk5.ufo.web.service.rest;

import swk5.ufo.web.model.VenueModel;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.ServiceConfig;
import swk5.ufo.web.service.VenueService;
import swk5.ufo.web.service.util.HttpUtil;

public class RestVenueService implements VenueService {

	@Override
	public VenueModel CreateVenue(String name, String shortcut, double latitude, double longitude) {
		throw new UnsupportedOperationException();
	}

	@Override
	public void DeleteVenue(VenueModel venue) throws ServiceCallException {
		throw new UnsupportedOperationException();
	}

	@Override
	public VenueModel[] getAllVenues() throws ServiceCallException {
		return HttpUtil.getFromJsonService(ServiceConfig.VENUE_GETALL, VenueModel[].class);
	}

	@Override
	public VenueModel getVenueById(int id) throws ServiceCallException {
		final String url = String.format(ServiceConfig.VENUE_GETBYID, id);
		return HttpUtil.getFromJsonService(url, VenueModel.class);
	}

	@Override
	public void UpdateVenue(VenueModel venue) throws ServiceCallException {
		throw new UnsupportedOperationException();
	}

}
