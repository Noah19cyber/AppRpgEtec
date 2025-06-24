namespace AppRpgEtec.Models
{
    public class Disputa
    {
        public int Id { get; set; }
        public int IdPersonagemAtacante { get; set; }
        public int IdPersonagemOponente { get; set; }
        public string Resultado { get; set; }

        public int AtacanteId { get; set; }
        public int OponenteId { get; set; }
        public int? HabilidadeId { get; set; }  // nullable
        public List<int> ListaIdPersonagens { get; set; } = new();
        public string Narracao { get; set; }
        public List<string> Resultados { get; set; } = new();

    }
}
