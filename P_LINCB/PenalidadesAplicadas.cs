using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
namespace P_LINCB
{
    public class PenalidadesAplicadas
    {
        [JsonProperty("razao_social")]
        public string RazaoSocial { get; set; }

        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }

        [JsonProperty("nome_motorista")]
        public string NomeMotorista { get; set; }

        [JsonProperty("cpf")]
        public string CPF { get; set; }

        [JsonProperty("vigencia_do_cadastro")]
        public DateTime VigenciaCadastro { get; set; }

        public PenalidadesAplicadas(string razaoSocial, string cNPJ, string nomeMotorista, string cPF, DateTime vigenciaCadastro)
        {
            RazaoSocial = razaoSocial;
            CNPJ = cNPJ;
            NomeMotorista = nomeMotorista;
            CPF = cPF;
            VigenciaCadastro = vigenciaCadastro;
        }

        public override string ToString() => $"RazaoSocial: {RazaoSocial}".PadRight(60) + $"CNPJ: {CNPJ}".PadRight(20) + $"Nome: {NomeMotorista}".PadRight(30) + $"CPF: {CPF}".PadRight(20) + $"Vigência Cadastro: {VigenciaCadastro}".PadRight(20);

    }
}