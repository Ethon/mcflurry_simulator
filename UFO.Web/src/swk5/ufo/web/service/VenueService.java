package swk5.ufo.web.service;

import swk5.ufo.web.model.VenueModel;

public interface VenueService {
	public VenueModel CreateVenue(String name, String shortcut,double latitude, double longitude);

	public void DeleteVenue(VenueModel venue) throws ServiceCallException;

	public VenueModel[] getAllVenues() throws ServiceCallException;

	public VenueModel getVenueById(int id) throws ServiceCallException;

	public void UpdateVenue(VenueModel venue) throws ServiceCallException;

}
