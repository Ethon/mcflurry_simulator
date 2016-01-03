package swk5.ufo.web.service.rest;

import swk5.ufo.web.model.CategoryModel;
import swk5.ufo.web.service.CategoryService;
import swk5.ufo.web.service.ServiceCallException;
import swk5.ufo.web.service.ServiceConfig;
import swk5.ufo.web.service.util.HttpUtil;

public class RestCategoryService implements CategoryService {

	@Override
	public CategoryModel[] getAllCategories() throws ServiceCallException {
		return HttpUtil.getFromJsonService(ServiceConfig.CATEGORY_GETALL, CategoryModel[].class);
	}

	@Override
	public CategoryModel getCategoryById(int id) throws ServiceCallException {
		final String url = String.format(ServiceConfig.CATEGORY_GETBYID, id);
		return HttpUtil.getFromJsonService(url, CategoryModel.class);
	}

}
