using AppRpgEtec.Models;
using AppRpgEtec.Services.Disputas;
using AppRpgEtec.Services.PersonagemHabilidades;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.DisputaNamespace
{
    public class DisputaViewModel : INotifyPropertyChanged
    {
        private readonly PersonagemService pService;
        private readonly DisputaService dService;
        private readonly PersonagemHabilidadeService phService;

        public ObservableCollection<Disputa> Disputas { get; set; } = new();
        public ObservableCollection<Personagem> PersonagensEncontrados { get; set; } = new();
        public ObservableCollection<PersonagemHabilidade> Habilidades { get; set; } = new();

        public Personagem Atacante { get; set; }
        public Personagem Oponente { get; set; }
        public Disputa DisputaPersonagens { get; set; }

        public ICommand PesquisarPersonagensCommand { get; set; }
        public ICommand DisputaComArmaCommand { get; set; }
        public ICommand DisputaComHabilidadeCommand { get; set; }
        public ICommand DisputaGeralCommand { get; set; }

        private string _descricaoPersonagemAtacante;
        public string DescricaoPersonagemAtacante
        {
            get => _descricaoPersonagemAtacante;
            set => SetProperty(ref _descricaoPersonagemAtacante, value);
        }

        private string _descricaoPersonagemOponente;
        public string DescricaoPersonagemOponente
        {
            get => _descricaoPersonagemOponente;
            set => SetProperty(ref _descricaoPersonagemOponente, value);
        }

        private Personagem _personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get => _personagemSelecionado;
            set
            {
                _personagemSelecionado = value;
                OnPropertyChanged();
                SelecionarPersonagem();
            }
        }

        private PersonagemHabilidade habilidadeSelecionada;
        public PersonagemHabilidade HabilidadeSelecionada
        {
            get => habilidadeSelecionada;
            set
            {
                habilidadeSelecionada = value;
                OnPropertyChanged();
            }
        }

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            dService = new DisputaService(token);
            phService = new PersonagemHabilidadeService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();
            DisputaPersonagens = new Disputa();

            PesquisarPersonagensCommand = new Command<string>(async (pesquisa) => await PesquisarPersonagens(pesquisa));
            DisputaComArmaCommand = new Command(async () => await ExecutarDisputaArmada());
            DisputaComHabilidadeCommand = new Command(async () => await ExecutarDisputaHabilidades());
            DisputaGeralCommand = new Command(async () => await ExecutarDisputaGeral());
        }

        private bool _escolherAtacante = true;

        private void SelecionarPersonagem()
        {
            if (PersonagemSelecionado == null) return;

            if (_escolherAtacante)
            {
                Atacante = PersonagemSelecionado;
                DescricaoPersonagemAtacante = $"{Atacante.Nome} ({Atacante.Classe})";
            }
            else
            {
                Oponente = PersonagemSelecionado;
                DescricaoPersonagemOponente = $"{Oponente.Nome} ({Oponente.Classe})";
            }

            _escolherAtacante = !_escolherAtacante;
        }

        public async Task ObterDisputas()
        {
            Disputas.Clear();
            var lista = await dService.GetDisputasAsync();
            foreach (var disputa in lista)
                Disputas.Add(disputa);
        }

        private async Task PesquisarPersonagens(string nome)
        {
            PersonagensEncontrados.Clear();
            var resultado = await pService.PesquisarPorNomeAsync(nome);
            foreach (var p in resultado)
                PersonagensEncontrados.Add(p);
        }

        public async Task ObterHabilidadesAsync(int personagemId)
        {
            try
            {
                Habilidades = await phService.GetPersonagemHabilidadesAsync(personagemId);
                OnPropertyChanged(nameof(Habilidades));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        private async Task ExecutarDisputaArmada()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;

                DisputaPersonagens = await dService.PostDisputaComArmaAsync(DisputaPersonagens);

                await Shell.Current.DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
                await ObterDisputas();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        private async Task ExecutarDisputaHabilidades()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;
                DisputaPersonagens.HabilidadeId = HabilidadeSelecionada?.HabilidadeId ?? 0;

                DisputaPersonagens = await dService.PostDisputaComHabilidadesAsync(DisputaPersonagens);

                await Shell.Current.DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
                await ObterDisputas();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        private async Task ExecutarDisputaGeral()
        {
            try
            {
                var lista = await pService.GetPersonagensAsync();
                DisputaPersonagens.ListaIdPersonagens = lista.Select(x => x.Id).ToList();

                DisputaPersonagens = await dService.PostDisputaGeralAsync(DisputaPersonagens);

                string resultados = string.Join(" | ", DisputaPersonagens.Resultados);

                await Shell.Current.DisplayAlert("Resultado", resultados, "Ok");
                await ObterDisputas();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
