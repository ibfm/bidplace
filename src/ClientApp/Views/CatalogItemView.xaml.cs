﻿namespace BidPlace.ClientApp.Views;

public partial class CatalogItemView
{
    public CatalogItemView(CatalogItemViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
