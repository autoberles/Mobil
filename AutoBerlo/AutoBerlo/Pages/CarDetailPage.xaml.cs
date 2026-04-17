using AutoBerlo.ViewModels;

namespace AutoBerlo.Pages;

public partial class CarDetailPage : ContentPage
{
    public CarDetailPage(CarDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}