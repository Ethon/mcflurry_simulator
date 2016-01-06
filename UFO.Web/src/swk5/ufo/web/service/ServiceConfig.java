package swk5.ufo.web.service;

public class ServiceConfig {

	public final static String SERVICE_BASE = "http://localhost:56566/";

	// VenueService
	public final static String VENUE_GETBYID = SERVICE_BASE + "api/Venue/GetVenueById/%s";
	public final static String VENUE_GETALL = SERVICE_BASE + "api/Venue/GetAllVenues";
	public final static String VENUE_UPDATE = SERVICE_BASE + "api/Venue/UpdateVenue";
	public final static String VENUE_DELETE = SERVICE_BASE + "api/Venue/DeleteVenue";
	public final static String VENUE_CREATE = SERVICE_BASE
			+ "api/Venue/CreateVenue?name=%s&shortcut=%s&latitude=%s&longitude=%s";

	// ArtistService
	public final static String ARTIST_GETBYID = SERVICE_BASE + "api/Artist/GetArtistById/%s";
	public final static String ARTIST_GETALL = SERVICE_BASE + "api/Artist/GetAllArtists";
	public final static String ARTIST_UPDATE = SERVICE_BASE + "api/Artist/UpdateArtist";
	public final static String ARTIST_DELETE = SERVICE_BASE + "api/Artist/DeleteArtist";
	public final static String ARTIST_CREATE = SERVICE_BASE
			+ "api/Artist/CreateArtist?name=%s&email=%s&picturePath=%s&videoPath=%s";

	// CountryService
	public final static String COUNTRY_GETBYID = SERVICE_BASE + "api/Country/GetCountryById/%s";
	public final static String COUNTRY_GETALL = SERVICE_BASE + "api/Country/GetAllCountries";
	public final static String COUNTRY_UPDATE = SERVICE_BASE + "api/Country/UpdateCountry";
	public final static String COUNTRY_DELETE = SERVICE_BASE + "api/Country/DeleteCountry";
	public final static String COUNTRY_CREATE = SERVICE_BASE + "api/Country/CreateCountry?name=%s&flagPath=%s";

	// UserService
	public final static String USER_GETBYID = SERVICE_BASE + "api/User/GetUserById//%s";
	public final static String USER_GETBYMAIL = SERVICE_BASE + "api/User/GetUserByEmailAdress?email=%s";
	public final static String USER_GETALL = SERVICE_BASE + "api/User/GetAllUsers";
	public final static String USER_CHECKCREDENTIALS = SERVICE_BASE + "api/User/CheckCredentials?email=%s&password=%s";

	// CategoryService
	public final static String CATEGORY_GETBYID = SERVICE_BASE + "api/Category/%s";
	public final static String CATEGORY_GETALL = SERVICE_BASE + "api/Category";

	// PerformanceService
	public final static String PERFORMANCE_GETBYID = SERVICE_BASE + "api/Performance/GetPerformanceById/%s";
	public final static String PERFORMANCE_GETFORDAY = SERVICE_BASE + "api/Performance/GetPerformancesForDay?date=%s";
	public final static String PERFORMANCE_GETALL = SERVICE_BASE + "api/Performance/GetAllPerformances";
	public final static String PERFORMANCE_UPDATE = SERVICE_BASE + "api/Performance/UpdatePerformance";
	public final static String PERFORMANCE_DELETE = SERVICE_BASE + "api/Country/DeletePerformance";
	public final static String PERFORMANCE_CREATE = SERVICE_BASE + "api/Country/CreatePerformance";
}
