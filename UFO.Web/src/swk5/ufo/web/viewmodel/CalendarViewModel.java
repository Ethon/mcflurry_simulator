package swk5.ufo.web.viewmodel;

import java.io.Serializable;
import java.time.Instant;
import java.time.ZoneId;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

import javax.annotation.PostConstruct;
import javax.faces.application.FacesMessage;
import javax.faces.bean.ManagedProperty;
import javax.faces.context.FacesContext;
import javax.faces.event.ActionEvent;

import org.primefaces.event.ScheduleEntryMoveEvent;
import org.primefaces.event.ScheduleEntryResizeEvent;
import org.primefaces.event.SelectEvent;
import org.primefaces.model.DefaultScheduleEvent;
import org.primefaces.model.DefaultScheduleModel;
import org.primefaces.model.ScheduleEvent;
import org.primefaces.model.ScheduleModel;

import swk5.ufo.web.DataStorage;
import swk5.ufo.web.model.ArtistModel;
import swk5.ufo.web.model.PerformanceModel;

public class CalendarViewModel implements Serializable {

	/**
	 *
	 */
	private static final long serialVersionUID = 199280051805712317L;

	private ScheduleModel eventModel;

	private ScheduleModel lazyEventModel;

	private ScheduleEvent event = new DefaultScheduleEvent();

	@ManagedProperty(value = "#{dataStorage}")
	private DataStorage dataStorage;

	public void addEvent(ActionEvent actionEvent) {
		if (event.getId() == null) {
			eventModel.addEvent(event);
		} else {
			eventModel.updateEvent(event);
		}

		event = new DefaultScheduleEvent();
	}

	private void addMessage(FacesMessage message) {
		FacesContext.getCurrentInstance().addMessage(null, message);
	}

	public DataStorage getDataStorage() {
		return dataStorage;
	}

	public ScheduleEvent getEvent() {
		return event;
	}

	public ScheduleModel getEventModel() {
		return eventModel;
	}

	public Date getInitialDate() {
		final Calendar calendar = Calendar.getInstance();
		calendar.set(calendar.get(Calendar.YEAR), Calendar.JULY, calendar.get(Calendar.DATE), 0, 0, 0);

		return calendar.getTime();
	}

	public ScheduleModel getLazyEventModel() {
		return lazyEventModel;
	}

	public Date getRandomDate(Date base) {
		final Calendar date = Calendar.getInstance();
		date.setTime(base);
		date.add(Calendar.DATE, ((int) (Math.random() * 30)) + 1); // set random
																	// day of
																	// month

		return date.getTime();
	}

	@PostConstruct
	public void init() {

		eventModel = new DefaultScheduleModel();
		final List<PerformanceModel> performances = dataStorage.getPerformances();

		for (final PerformanceModel cur : performances) {
			eventModel.addEvent(performanceToEvent(cur));
		}
	}

	public void onDateSelect(SelectEvent selectEvent) {
		event = new DefaultScheduleEvent("", (Date) selectEvent.getObject(), (Date) selectEvent.getObject());
	}

	public void onEventMove(ScheduleEntryMoveEvent event) {
		final FacesMessage message = new FacesMessage(FacesMessage.SEVERITY_INFO, "Event moved",
				"Day delta:" + event.getDayDelta() + ", Minute delta:" + event.getMinuteDelta());

		addMessage(message);
	}

	public void onEventResize(ScheduleEntryResizeEvent event) {
		final FacesMessage message = new FacesMessage(FacesMessage.SEVERITY_INFO, "Event resized",
				"Day delta:" + event.getDayDelta() + ", Minute delta:" + event.getMinuteDelta());

		addMessage(message);
	}

	public void onEventSelect(SelectEvent selectEvent) {
		event = (ScheduleEvent) selectEvent.getObject();
	}

	private DefaultScheduleEvent performanceToEvent(PerformanceModel p) {
		final Instant instant = p.getDate().atZone(ZoneId.systemDefault()).toInstant();
		final Date startDate = Date.from(instant);
		final DefaultScheduleEvent event = new DefaultScheduleEvent();

		final Calendar cal = Calendar.getInstance(); // creates calendar
		cal.setTime(startDate); // sets calendar time/date

		final ArtistModel artist = dataStorage.getArtistById(p.getArtistId());
		if (artist == null) {
			event.setTitle("Unknown artist " + String.valueOf(p.getArtistId()));
		} else {
			event.setTitle(artist.getName());
		}

		event.setStartDate(cal.getTime());
		cal.add(Calendar.HOUR_OF_DAY, 1); // adds one hour
		event.setEndDate(cal.getTime());

		return event;
	}

	public void setDataStorage(DataStorage dataStorage) {
		this.dataStorage = dataStorage;
	}

	public void setEvent(ScheduleEvent event) {
		this.event = event;
	}

	private Calendar today() {
		final Calendar calendar = Calendar.getInstance();
		calendar.set(calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DATE), 0, 0, 0);

		return calendar;
	}

}