package swk5.ufo.web.service;

import swk5.ufo.web.model.CategoryModel;

public interface CategoryService {
	public CategoryModel[] getAllCategories() throws ServiceCallException;

	public CategoryModel getCategoryById(int id) throws ServiceCallException;
}
