package swk5.ufo.web.service;

import java.util.Locale.Category;

public interface CategoryService {
	public Category[] getAllCategories() throws ServiceCallException;

	public Category getCategoryById(int id) throws ServiceCallException;
}
