package swk5.ufo.web.model;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

import com.google.gson.annotations.SerializedName;

public class PerformanceModel {

	@SerializedName("Id")
	private final int id;

	@SerializedName("Date")
	private final String date;

	@SerializedName("ArtistId")
	private final int artistId;

	@SerializedName("VenueId")
	private final int venueId;

	public PerformanceModel(int id, String date, int artistId, int venueId) {
		this.id = id;
		this.date = date;
		this.artistId = artistId;
		this.venueId = venueId;
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
		final PerformanceModel other = (PerformanceModel) obj;
		if (artistId != other.artistId) {
			return false;
		}
		if (date == null) {
			if (other.date != null) {
				return false;
			}
		} else if (!date.equals(other.date)) {
			return false;
		}
		if (id != other.id) {
			return false;
		}
		if (venueId != other.venueId) {
			return false;
		}
		return true;
	}

	public int getArtistId() {
		return artistId;
	}

	public LocalDateTime getDate() {
		final DateTimeFormatter formatter = DateTimeFormatter.ofPattern("uuuu-MM-dd'T'H:m:s");
		return LocalDateTime.parse(date, formatter);
	}

	public int getId() {
		return id;
	}

	public int getVenueId() {
		return venueId;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + artistId;
		result = prime * result + ((date == null) ? 0 : date.hashCode());
		result = prime * result + id;
		result = prime * result + venueId;
		return result;
	}

	@Override
	public String toString() {
		return "PerformanceModel [id=" + id + ", date=" + date + ", artistId=" + artistId + ", venueId=" + venueId
				+ "]";
	}
}
