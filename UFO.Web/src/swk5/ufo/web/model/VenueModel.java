package swk5.ufo.web.model;

import com.google.gson.annotations.SerializedName;

public class VenueModel {
	@SerializedName("Id")
	private final int id;

	@SerializedName("Shortcut")
	private final String shortcut;

	@SerializedName("Name")
	private final String name;

	@SerializedName("Latitude")
	private final double latitude;

	@SerializedName("Longitude")
	private final double longitude;

	public VenueModel(int id, String shortcut, String name, double latitude, double longitude) {
		this.id = id;
		this.shortcut = shortcut;
		this.name = name;
		this.latitude = latitude;
		this.longitude = longitude;
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
		final VenueModel other = (VenueModel) obj;
		if (id != other.id) {
			return false;
		}
		if (Double.doubleToLongBits(latitude) != Double.doubleToLongBits(other.latitude)) {
			return false;
		}
		if (Double.doubleToLongBits(longitude) != Double.doubleToLongBits(other.longitude)) {
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

	public double getLatitude() {
		return latitude;
	}

	public double getLongitude() {
		return longitude;
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
		long temp;
		temp = Double.doubleToLongBits(latitude);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		temp = Double.doubleToLongBits(longitude);
		result = prime * result + (int) (temp ^ (temp >>> 32));
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		result = prime * result + ((shortcut == null) ? 0 : shortcut.hashCode());
		return result;
	}

	@Override
	public String toString() {
		return "VenueModel [id=" + id + ", shortcut=" + shortcut + ", name=" + name + ", latitude=" + latitude
				+ ", longitude=" + longitude + "]";
	}
}
