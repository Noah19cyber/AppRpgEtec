using AppRpgEtec.ViewModels.DisputaNamespace;

namespace AppRpgEtec.Views.Disputas
{
    public partial class ListagemDisputaView : ContentPage
    {
        DisputaViewModel viewModel;

        public ListagemDisputaView()
        {
            InitializeComponent();
            viewModel = new DisputaViewModel();
            BindingContext = viewModel;
        }
    }
}
