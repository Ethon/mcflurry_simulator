package swk5.ufo.web.service.rest;

import swk5.ufo.web.model.CountryModel;
import swk5.ufo.web.service.CountryService;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.ServiceConfig;
import swk5.ufo.web.service.util.HttpUtil;

public class RestCountryService implements CountryService {

	@Override
	public CountryModel CreateCountry(String name, String flagPath) throws ServiceCallException {
		throw new UnsupportedOperationException();
	}

	@Override
	public void DeleteCountry(CountryModel country) throws ServiceCallException {
		throw new UnsupportedOperationException();

	}

	@Override
	public CountryModel[] getAllCountries() throws ServiceCallException {
		throw new UnsupportedOperationException();
	}

	@Override
	public CountryModel getCountryById(int id) throws ServiceCallException {
		final String url = String.format(ServiceConfig.COUNTRY_GETBYID, id);
		return HttpUtil.getFromJsonService(url, CountryModel.class);
	}

	@Override
	public void UpdateCountry(CountryModel country) throws ServiceCallException {
		throw new UnsupportedOperationException();
	}

}
