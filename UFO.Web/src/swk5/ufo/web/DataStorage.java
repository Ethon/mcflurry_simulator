package swk5.ufo.web;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.PerformanceModel;
import swk5.ufo.web.model.VenueModel;
import swk5.ufo.web.service.ArtistService;
import swk5.ufo.web.service.PerformanceService;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.VenueService;
import swk5.ufo.web.service.rest.RestArtistService;
import swk5.ufo.web.service.rest.RestPerformanceService;
import swk5.ufo.web.service.rest.RestVenueService;

public class DataStorage {
	private final ArtistService artistService;
	private final List<ArtistModel> artists;
	private final Map<Integer, ArtistModel> artistById;
	private final Map<String, ArtistModel> artistByName;

	private final VenueService venueService;
	private final List<VenueModel> venues;
	private final Map<Integer, VenueModel> venueById;

	private final PerformanceService performanceService;
	private final List<PerformanceModel> performances;
	private final Map<Integer, List<PerformanceModel>> performancesForVenue;

	public DataStorage() throws ServiceCallException {
		artistService = new RestArtistService();
		artists = Arrays.asList(artistService.getAllArtists());
		artistById = new HashMap<>();
		artistByName = new HashMap<>();

		venueService = new RestVenueService();
		venues = Arrays.asList(venueService.getAllVenues());
		venueById = new HashMap<>();

		performanceService = new RestPerformanceService();
		performances = Arrays.asList(performanceService.getAllPerformances());
		performancesForVenue = new HashMap<>();
	}

	public ArtistModel getArtistById(int id) {
		ArtistModel artist = artistById.get(id);
		if (artist == null) {
			for (final ArtistModel artistModel : artists) {
				if (artistModel.getId() == id) {
					artist = artistModel;
					artistById.put(id, artist);
					break;
				}
			}
		}
		return artist;
	}

	public ArtistModel getArtistByName(String name) throws ServiceCallException {
		ArtistModel artist = artistByName.get(name);
		if (artist == null) {
			artist = artistService.getArtistByName(name);
			artistByName.put(name, artist);
		}
		return artist;
	}

	public List<ArtistModel> getArtists() {
		return artists;
	}

	public List<PerformanceModel> getPerformances() {
		return performances;
	}

	public List<PerformanceModel> getPerformancesForVenue(int venueId) {
		List<PerformanceModel> performances = performancesForVenue.get(venueId);
		if (performances == null) {
			performances = new ArrayList<>();
			for (final PerformanceModel performanceModel : this.performances) {
				if (performanceModel.getVenueId() == venueId) {
					performances.add(performanceModel);
				}
			}
			performancesForVenue.put(venueId, performances);
		}
		return performances;
	}

	public VenueModel getVenueById(int id) {
		VenueModel venue = venueById.get(id);
		if (venue == null) {
			for (final VenueModel venueModel : venues) {
				if (venueModel.getId() == id) {
					venue = venueModel;
					venueById.put(id, venue);
					break;
				}
			}
		}
		return venue;
	}

	public List<VenueModel> getVenues() {
		return venues;
	}
}
