package swk5.ufo.web.service.rest;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.CategoryModel;
import swk5.ufo.web.model.CountryModel;
import swk5.ufo.web.service.ArtistService;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.ServiceConfig;
import swk5.ufo.web.service.util.HttpUtil;

public class RestArtistService implements ArtistService {

	@Override
	public ArtistModel CreateArtist(String name, String email, CategoryModel category, CountryModel country,
			String picturePath, String videoPath) {
		throw new UnsupportedOperationException();
	}

	@Override
	public void DeleteArtist(ArtistModel artist) throws ServiceCallException {
		throw new UnsupportedOperationException();

	}

	@Override
	public ArtistModel[] getAllArtists() throws ServiceCallException {
		return HttpUtil.getFromJsonService(ServiceConfig.ARTIST_GETALL, ArtistModel[].class);
	}

	@Override
	public ArtistModel getArtistById(int id) throws ServiceCallException {
		final String url = String.format(ServiceConfig.ARTIST_GETBYID, id);
		return HttpUtil.getFromJsonService(url, ArtistModel.class);
	}

	@Override
	public void UpdateArtist(ArtistModel artist) throws ServiceCallException {
		throw new UnsupportedOperationException();

	}

}
