using System;
using CommunityToolkit.Mvvm.ComponentModel;
using OneApp.Contracts.v1.Response;

namespace OneApp.ViewModels;

[QueryProperty("Product", "Product")]
public partial class ProductDetailsViewModel: ObservableObject
{
	public ProductDetailsViewModel()
	{
		//Application.Current.MainPage.DisplayAlert("Test", $"ProductID: {Product.Name}", "Cancel");
	}

	[ObservableProperty]
	Product product;
}

