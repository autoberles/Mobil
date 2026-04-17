using AutoBerlo.ViewModels;

namespace AutoBerlo.Pages;

public partial class PaymentPage : ContentPage
{
    public PaymentPage(PaymentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}