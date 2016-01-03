package swk5.ufo.web.service.rest;

import java.util.Locale.Category;

import swk5.ufo.web.service.CategoryService;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.ServiceConfig;
import swk5.ufo.web.service.util.HttpUtil;

public class RestCategoryService implements CategoryService {

	@Override
	public Category[] getAllCategories() throws ServiceCallException {
		return HttpUtil.getFromJsonService(ServiceConfig.CATEGORY_GETALL, Category[].class);
	}

	@Override
	public Category getCategoryById(int id) throws ServiceCallException {
		final String url = String.format(ServiceConfig.CATEGORY_GETBYID, id);
		return HttpUtil.getFromJsonService(url, Category.class);
	}

}
