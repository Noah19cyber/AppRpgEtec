﻿using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    [QueryProperty("PersonagemSelecionadoId", "pId")]
    public class CadastroPersonagemViewModel : BaseViewModel
    {
        private PersonagemService pService;
        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; set; }

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); }, () => CadastroHabilitado);
            CancelarCommand = new Command(async () => await CancelarCadastro());
        }

        private async Task CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;

        public int Id { get => id; set { id = value; OnPropertyChanged(); } }
        public string Nome { get => nome; set { nome = value; OnPropertyChanged(); ((Command)SalvarCommand).ChangeCanExecute(); } }
        public int PontosVida { get => pontosVida; set { pontosVida = value; OnPropertyChanged(); OnPropertyChanged(nameof(CadastroHabilitado)); } }
        public int Forca { get => forca; set { forca = value; OnPropertyChanged(); } }
        public int Defesa { get => defesa; set { defesa = value; OnPropertyChanged(); } }
        public int Inteligencia { get => inteligencia; set { inteligencia = value; OnPropertyChanged(); } }
        public int Disputas { get => disputas; set { disputas = value; OnPropertyChanged(); } }
        public int Vitorias { get => vitorias; set { vitorias = value; OnPropertyChanged(); } }
        public int Derrotas { get => derrotas; set { derrotas = value; OnPropertyChanged(); } }

        private ObservableCollection<TipoClasse> listaTiposClasse;
        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get => listaTiposClasse;
            set { listaTiposClasse = value; OnPropertyChanged(); }
        }

        public async Task ObterClasses()
        {
            ListaTiposClasse = new ObservableCollection<TipoClasse>
            {
                new TipoClasse { Id = 1, Descricao = "Cavaleiro" },
                new TipoClasse { Id = 2, Descricao = "Mago" },
                new TipoClasse { Id = 3, Descricao = "Clerigo" }
            };
            await Task.CompletedTask;
        }

        private TipoClasse tipoClasseSelecionado;
        public TipoClasse TipoClasseSelecionado
        {
            get => tipoClasseSelecionado;
            set { tipoClasseSelecionado = value; OnPropertyChanged(); }
        }

        public async Task SalvarPersonagem()
        {
            try
            {
                Personagem model = new Personagem
                {
                    Nome = this.nome,
                    PontosVida = this.pontosVida,
                    Defesa = this.defesa,
                    Derrotas = this.derrotas,
                    Disputas = this.disputas,
                    Forca = this.forca,
                    Inteligencia = this.inteligencia,
                    Vitorias = this.vitorias,
                    Id = this.id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id
                };

                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", $"{ex.Message}\nDetalhes: {ex.InnerException}", "Ok");
            }
        }

        public async void CarregarPersonagem()
        {
            try
            {
                Personagem p = await pService.GetPersonagemAsync(int.Parse(personagemSelecionadoId));

                Nome = p.Nome;
                PontosVida = p.PontosVida;
                Defesa = p.Defesa;
                Derrotas = p.Derrotas;
                Disputas = p.Disputas;
                Forca = p.Forca;
                Inteligencia = p.Inteligencia;
                Vitorias = p.Vitorias;
                Id = p.Id;

                TipoClasseSelecionado = ListaTiposClasse.FirstOrDefault(tc => tc.Id == (int)p.Classe);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", $"{ex.Message}\nDetalhes: {ex.InnerException}", "Ok");
            }
        }

        private string personagemSelecionadoId;
        public string PersonagemSelecionadoId
        {
            set
            {
                if (value != null)
                {
                    personagemSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarPersonagem();
                }
            }
        }

        public bool CadastroHabilitado =>
            !string.IsNullOrEmpty(Nome) &&
            PontosVida > 0 &&
            Forca >= 0 &&
            Defesa >= 0 &&
            Inteligencia >= 0 &&
            TipoClasseSelecionado != null;
    }
}
