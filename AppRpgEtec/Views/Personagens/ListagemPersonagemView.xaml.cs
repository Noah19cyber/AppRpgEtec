using AppRpgEtec.Models;
using AppRpgEtec.ViewModels.Personagens;

namespace AppRpgEtec.Views.Personagens;

public partial class ListagemPersonagemView : ContentPage
{
    ListagemPersonagemViewModel viewModel;
    private Personagem personagemSelecionado;

    public ListagemPersonagemView()
    {
        InitializeComponent();
        viewModel = new ListagemPersonagemViewModel();
        BindingContext = viewModel;
        Title = "Personagens - App Rpg Etec";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.ObterPersonagens();
    }

    public async Task ExibirOpcoesAsync(Personagem personagem)
    {
        try
        {
            personagemSelecionado = null;
            string result = string.Empty;

            if (personagem.PontosVida > 0)
            {
                result = await Application.Current.MainPage
                    .DisplayActionSheet("Op��es para o personagem " + personagem.Nome,
                    "Cancelar",
                    "Editar Personagem",
                    "Restaurar Pontos de Vida",
                    "Zerar Ranking do Personagem",
                    "Remover Personagem");
            }
            else
            {
                result = await Application.Current.MainPage
                    .DisplayActionSheet("Op��es para o personagem " + personagem.Nome,
                    "Cancelar",
                    "Restaurar Pontos de Vida");
            }

            if (result != null)
                ProcessarOpcaoRespondidaAsync(personagem, result);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Ops...", ex.Message, "Ok");
        }
    }

    private async void ProcessarOpcaoRespondidaAsync(Personagem personagem, string opcao)
    {
        switch (opcao)
        {
            case "Editar Personagem":
                await Application.Current.MainPage.DisplayAlert("Editar", $"Editar {personagem.Nome}", "OK");
                break;

            case "Restaurar Pontos de Vida":
                personagem.PontosVida = 100;
                await Application.Current.MainPage.DisplayAlert("Vida", $"Pontos de vida restaurados para {personagem.Nome}", "OK");
                break;

            case "Zerar Ranking do Personagem":
                personagem.PontosVida = 0;
                await Application.Current.MainPage.DisplayAlert("Ranking", $"Ranking zerado para {personagem.Nome}", "OK");
                break;

            case "Remover Personagem":
                await Application.Current.MainPage.DisplayAlert("Remover", $"Remover {personagem.Nome}", "OK");
                break;
        }
    }
}
