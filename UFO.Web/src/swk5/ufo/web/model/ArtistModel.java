package swk5.ufo.web.model;

import com.google.gson.annotations.SerializedName;

public class ArtistModel {

	@SerializedName("Id")
	private final int id;

	@SerializedName("Name")
	private final String name;

	@SerializedName("Email")
	private final String email;

	@SerializedName("CategoryId")
	private final int categoryId;

	@SerializedName("CountryId")
	private final int countryId;

	@SerializedName("PicturePath")
	private final String picturePath;

	@SerializedName("VideoPath")
	private final String videoPath;

	@SerializedName("IsDeleted")
	private final boolean isDeleted;

	public ArtistModel(int id, String name, String email, int categoryId, int countryId, String picturePath,
			String videoPath, boolean isDeleted) {
		this.id = id;
		this.name = name;
		this.email = email;
		this.categoryId = categoryId;
		this.countryId = countryId;
		this.picturePath = picturePath;
		this.videoPath = videoPath;
		this.isDeleted = isDeleted;
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
		final ArtistModel other = (ArtistModel) obj;
		if (categoryId != other.categoryId) {
			return false;
		}
		if (countryId != other.countryId) {
			return false;
		}
		if (email == null) {
			if (other.email != null) {
				return false;
			}
		} else if (!email.equals(other.email)) {
			return false;
		}
		if (id != other.id) {
			return false;
		}
		if (isDeleted != other.isDeleted) {
			return false;
		}
		if (name == null) {
			if (other.name != null) {
				return false;
			}
		} else if (!name.equals(other.name)) {
			return false;
		}
		if (picturePath == null) {
			if (other.picturePath != null) {
				return false;
			}
		} else if (!picturePath.equals(other.picturePath)) {
			return false;
		}
		if (videoPath == null) {
			if (other.videoPath != null) {
				return false;
			}
		} else if (!videoPath.equals(other.videoPath)) {
			return false;
		}
		return true;
	}

	public int getCategoryId() {
		return categoryId;
	}

	public int getCountryId() {
		return countryId;
	}

	public String getEmail() {
		return email;
	}

	public int getId() {
		return id;
	}

	public String getName() {
		return name;
	}

	public String getPicturePath() {
		return picturePath;
	}

	public String getVideoPath() {
		return videoPath;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + categoryId;
		result = prime * result + countryId;
		result = prime * result + ((email == null) ? 0 : email.hashCode());
		result = prime * result + id;
		result = prime * result + (isDeleted ? 1231 : 1237);
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		result = prime * result + ((picturePath == null) ? 0 : picturePath.hashCode());
		result = prime * result + ((videoPath == null) ? 0 : videoPath.hashCode());
		return result;
	}

	public boolean isDeleted() {
		return isDeleted;
	}

	@Override
	public String toString() {
		return "ArtistModel [id=" + id + ", name=" + name + ", email=" + email + ", categoryId=" + categoryId
				+ ", countryId=" + countryId + ", picturePath=" + picturePath + ", videoPath=" + videoPath
				+ ", isDeleted=" + isDeleted + "]";
	}
}
