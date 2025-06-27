using AppRpgEtec.Models;

namespace AppRpgEtec.Services.Disputas
{
    public class DisputaService : Request
    {
        private readonly Request _request;
        private string _token;

        // 🔑 Sua URL base — ajuste para sua API real
        private const string _apiUrlBase = "http://10.0.2.2:5243/api/Disputas";

        public DisputaService(string token)
        {
            _request = new Request();
            _token = token;
        }

        // ✅ Novo método para LISTAR as disputas
        public async Task<List<Disputa>> GetDisputasAsync()
        {
            return await _request.GetAsync<List<Disputa>>(_apiUrlBase, _token);
        }

        public async Task<Disputa> PostDisputaComArmaAsync(Disputa d)
        {
            string urlComplementar = "/Arma";
            return await _request.PostAsync(_apiUrlBase + urlComplementar, d, _token);
        }

        public async Task<Disputa> PostDisputaComHabilidadesAsync(Disputa d)
        {
            string urlComplementar = "/Habilidade";
            return await _request.PostAsync(_apiUrlBase + urlComplementar, d, _token);
        }

        public async Task<Disputa> PostDisputaGeralAsync(Disputa d)
        {
            string urlComplementar = "/DisputaEmGrupo";
            return await _request.PostAsync(_apiUrlBase + urlComplementar, d, _token);
        }
    }
}
