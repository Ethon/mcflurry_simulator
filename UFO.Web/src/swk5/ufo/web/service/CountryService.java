package swk5.ufo.web.service;

import swk5.ufo.web.model.CountryModel;

public interface CountryService {
	public CountryModel CreateCountry(String name, String flagPath) throws ServiceCallException;

	public void DeleteCountry(CountryModel country) throws ServiceCallException;

	public CountryModel[] getAllCountries() throws ServiceCallException;

	public CountryModel getCountryById(int id) throws ServiceCallException;

	public void UpdateCountry(CountryModel country) throws ServiceCallException;
}
