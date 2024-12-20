﻿using Microsoft.Maui.Controls;
using OneApp.ViewModels;

namespace OneApp.Views;

public partial class Products : ContentPage
{
	private readonly ProductsViewModel _productVM;
	public Products(ProductsViewModel productVM)
	{
		InitializeComponent();
		this._productVM = productVM;
		BindingContext = productVM;
    }

	protected override void OnAppearing()
    {
		base.OnAppearing();
		if (!_productVM.Products.Any())
		{
			_productVM.IsLoading = true;
            Task.Run(_productVM.Load);
            Task.Run(_productVM.LoadProductTypes);
			_productVM.IsLoading = false;
        }
	}
}
