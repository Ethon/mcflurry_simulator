package swk5.ufo.web.service;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.CategoryModel;
import swk5.ufo.web.model.CountryModel;

public interface ArtistService {
	public ArtistModel createArtist(String name, String email, CategoryModel category, CountryModel country,
			String picturePath, String videoPath);

	public void deleteArtist(ArtistModel artist) throws ServiceCallException;

	public ArtistModel[] getAllArtists() throws ServiceCallException;

	public ArtistModel getArtistById(int id) throws ServiceCallException;

	public ArtistModel getArtistByName(String name) throws ServiceCallException;

	public void updateArtist(ArtistModel artist) throws ServiceCallException;

}
