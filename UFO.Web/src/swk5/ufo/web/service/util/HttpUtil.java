package swk5.ufo.web.service.util;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.ByteArrayEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.util.EntityUtils;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import swk5.ufo.web.service.ServiceCallException;

public class HttpUtil {
	private static String executeGetRequest(String url) throws ServiceCallException {
		try (CloseableHttpClient client = HttpClientBuilder.create().build()) {
			final HttpGet get = new HttpGet(url);
			get.addHeader("Content-Type", "application/json");
			get.addHeader("Cache-Control", "no-cache");

			final HttpResponse response = client.execute(get);
			return EntityUtils.toString(response.getEntity());
		} catch (final Exception e) {
			throw new ServiceCallException("GET error", e);
		}
	}

	private static String executePostRequest(String url, String reqBody) throws ServiceCallException {
		try (CloseableHttpClient client = HttpClientBuilder.create().build()) {
			final HttpPost post = new HttpPost(url);
			post.addHeader("Content-Type", "application/json");
			post.addHeader("Cache-Control", "no-cache");

			final HttpEntity entity = new ByteArrayEntity(reqBody.getBytes("UTF-8"));
			post.setEntity(entity);

			final HttpResponse response = client.execute(post);
			return EntityUtils.toString(response.getEntity());
		} catch (final Exception e) {
			throw new ServiceCallException("POST error", e);
		}
	}

	public static <T> T getFromJsonService(String url, Class<T> resultClass) throws ServiceCallException {
		final Gson gson = new GsonBuilder().create();
		return gson.fromJson(executeGetRequest(url), resultClass);
	}

	public static <T, S> T postToJsonService(String url, S bodyObject, Class<T> resultClass)
			throws ServiceCallException {
		final Gson gson = new GsonBuilder().create();
		final String result = executePostRequest(url, gson.toJson(bodyObject));
		return gson.fromJson(result, resultClass);
	}
}
