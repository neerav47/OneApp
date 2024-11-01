using System;
using System.Net;
using OneApp.Services;
using OneApp.Services.Interfaces;
using OneApp.ViewModels;
using OneApp.Views;
using OneApp.Views.BottomSheets;
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
			serviceCollection.AddHttpClient<IDefaultHttpClient, DefaultHttpClient>(client =>
			{
				client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(GetRetryPolicy())
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

		public static void AddServies(this IServiceCollection serviceCollection)
		{
            serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>();
			serviceCollection.AddTransient<IProductService, ProductsService>();
        }

		public static void AddPages(this IServiceCollection serviceCollection)
		{
            serviceCollection.AddTransient<MainPage>();
            serviceCollection.AddTransient<LoginPage>();
            serviceCollection.AddTransient<AppShell>();
			serviceCollection.AddTransient<Products>();
			serviceCollection.AddTransient<ProductDetails>();
        }

		public static void AddViewModels(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<LoginViewModel>();
			serviceCollection.AddTransient<ProductsViewModel>();
			serviceCollection.AddTransient<ProductDetailsViewModel>();
		}
	}
}

