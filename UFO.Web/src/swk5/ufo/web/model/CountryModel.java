package swk5.ufo.web.model;

import com.google.gson.annotations.SerializedName;

public class CountryModel {
	@SerializedName("Id")
	private final int id;

	@SerializedName("Name")
	private final String name;

	@SerializedName("FlagPath")
	private final String flagPath;

	public CountryModel(int id, String name, String flagPath) {
		super();
		this.id = id;
		this.name = name;
		this.flagPath = flagPath;
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
		final CountryModel other = (CountryModel) obj;
		if (flagPath == null) {
			if (other.flagPath != null) {
				return false;
			}
		} else if (!flagPath.equals(other.flagPath)) {
			return false;
		}
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
		return true;
	}

	public String getFlagPath() {
		return flagPath;
	}

	public int getId() {
		return id;
	}

	public String getName() {
		return name;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((flagPath == null) ? 0 : flagPath.hashCode());
		result = prime * result + id;
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		return result;
	}

	@Override
	public String toString() {
		return "CountryModel [id=" + id + ", name=" + name + ", flagPath=" + flagPath + "]";
	}
}
