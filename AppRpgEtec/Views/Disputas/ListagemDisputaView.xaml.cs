using AppRpgEtec.Models;
using AppRpgEtec.ViewModels.DisputaNamespace;

namespace AppRpgEtec.Views.Disputas
{
    public partial class ListagemDisputaView : ContentPage
    {
        private DisputaViewModel _viewModel;

        public ListagemDisputaView()
        {
            InitializeComponent();

            // ✅ Garante que o BindingContext está definido corretamente
            _viewModel = new DisputaViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel?.OnViewAppearing();
        }

        private async void OnPersonagemSelecionado(object sender, SelectionChangedEventArgs e)
        {
            var personagem = e.CurrentSelection.FirstOrDefault() as Personagem;
            if (personagem != null)
                _viewModel.PersonagemSelecionado = personagem;

            await _viewModel.ObterHabilidadesAsync(_viewModel.Atacante.Id);
        }
        private async void OnSearchPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;
            if (searchBar != null)
                await _viewModel.PesquisarPersonagens(searchBar.Text);
        }

    }
}


