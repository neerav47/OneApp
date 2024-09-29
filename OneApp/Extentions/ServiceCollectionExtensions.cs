using System;
using System.Net;
using OneApp.Services;
using Polly;
using Polly.Retry;

namespace OneApp.Extentions
{
	public static class ServiceCollectionExtensions
	{
		private static readonly List<HttpStatusCode> HttpStatusCodeListForRetry = new()
		{
			HttpStatusCode.RequestTimeout,
			HttpStatusCode.InternalServerError,
			HttpStatusCode.BadGateway,
			HttpStatusCode.ServiceUnavailable,
			HttpStatusCode.GatewayTimeout,
			HttpStatusCode.TooManyRequests
		};

		private const short MaxRetryAttempts = 3;

		public static void AddHttpClients(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddHttpClient<IDefaultHttpClient, DefaultHttpClient>()
				.AddPolicyHandler(GetRetryPolicy())
				.SetHandlerLifetime(TimeSpan.FromMinutes(60));
		}

		public static bool IsRetryableStatusCode(HttpStatusCode statusCode)
		{
			return HttpStatusCodeListForRetry.Contains(statusCode);
		}

		public static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return Policy.HandleResult<HttpResponseMessage>(r => IsRetryableStatusCode(r.StatusCode))
				.Or<HttpRequestException>()
				.WaitAndRetryAsync(retryCount: MaxRetryAttempts, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(x: 2, retryAttempt) / 2)); 
		}
	}
}

