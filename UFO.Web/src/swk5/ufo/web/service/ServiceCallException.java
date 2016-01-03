package swk5.ufo.web.service;

public class ServiceCallException extends Exception {

	private static final long serialVersionUID = -2371294419863475356L;

	public ServiceCallException() {
	}

	public ServiceCallException(String arg0) {
		super(arg0);
	}

	public ServiceCallException(String arg0, Throwable arg1) {
		super(arg0, arg1);
	}

	public ServiceCallException(String arg0, Throwable arg1, boolean arg2, boolean arg3) {
		super(arg0, arg1, arg2, arg3);
	}

	public ServiceCallException(Throwable arg0) {
		super(arg0);
	}
}
