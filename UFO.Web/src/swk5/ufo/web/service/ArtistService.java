package swk5.ufo.web.service;

import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.CategoryModel;
import swk5.ufo.web.model.CountryModel;

public interface ArtistService {
	public ArtistModel CreateArtist(String name, String email, CategoryModel category, CountryModel country,
			String picturePath, String videoPath);

	public void DeleteArtist(ArtistModel artist) throws ServiceCallException;

	public ArtistModel[] getAllArtists() throws ServiceCallException;

	public ArtistModel getArtistById(int id) throws ServiceCallException;

	public void UpdateArtist(ArtistModel artist) throws ServiceCallException;

}
