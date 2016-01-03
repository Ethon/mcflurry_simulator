package swk5.ufo.web.model;

import com.google.gson.annotations.SerializedName;

public class CategoryModel {
	@SerializedName("Id")
	private final int id;

	@SerializedName("Shortcut")
	private final String shortcut;

	@SerializedName("Name")
	private final String name;

	public CategoryModel(int id, String shortcut, String name) {
		super();
		this.id = id;
		this.shortcut = shortcut;
		this.name = name;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) {
			return true;
		}
		if (obj == null) {
			return false;
		}
		if (getClass() != obj.getClass()) {
			return false;
		}
		final CategoryModel other = (CategoryModel) obj;
		if (id != other.id) {
			return false;
		}
		if (name == null) {
			if (other.name != null) {
				return false;
			}
		} else if (!name.equals(other.name)) {
			return false;
		}
		if (shortcut == null) {
			if (other.shortcut != null) {
				return false;
			}
		} else if (!shortcut.equals(other.shortcut)) {
			return false;
		}
		return true;
	}

	public int getId() {
		return id;
	}

	public String getName() {
		return name;
	}

	public String getShortcut() {
		return shortcut;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + id;
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		result = prime * result + ((shortcut == null) ? 0 : shortcut.hashCode());
		return result;
	}

	@Override
	public String toString() {
		return "CategoryModel [id=" + id + ", shortcut=" + shortcut + ", name=" + name + "]";
	}

}
