using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.CompilerServices;
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

        public override string ToString() => $"RazaoSocial: {RazaoSocial}".PadRight(60) + $"CNPJ: {CNPJ}".PadRight(20) + $"Nome: {NomeMotorista}".PadRight(30) + $"CPF: {CPF}".PadRight(20) + $"Vigência Cadastro: {VigenciaCadastro}".PadRight(20);

        public static void InserirSQL(SqlConnection _connSQL, PenalidadesAplicadas obj)
        {
            try
            {
                _connSQL.Open();
                SqlCommand sql_cmnd = new SqlCommand("INSERIR_DADOSJSON", _connSQL);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = obj.RazaoSocial;
                sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = obj.CNPJ;
                sql_cmnd.Parameters.AddWithValue("@NomeMotorista", SqlDbType.VarChar).Value = obj.NomeMotorista;
                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = obj.CPF;
                sql_cmnd.Parameters.AddWithValue("@Vigencia", SqlDbType.Date).Value = obj.VigenciaCadastro;
                sql_cmnd.ExecuteNonQuery();
                _connSQL.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void InsertToSQL(List<PenalidadesAplicadas> lst)
        {
            // Inserção de registros por batch de 1000 registros
            int count = 0;
            Banco _dbSQL = new Banco();
            SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
            SqlCommand cmdSQL = new SqlCommand();

            count = lst.Count();
            int batchSize = 1000;
            int batches = (int)Math.Floor((double)count / batchSize);

            for (int j = 0; j <= batches; j++)
            {
                string aux = " INSERT INTO Tb_Dados2 (RazaoSocial, CNPJ, NomeMotorista, CPF, DtVigencia) VALUES";

                foreach (var item in lst.Skip(1000 * j).Take(1000))
                {
                    if (item.RazaoSocial != null)
                    {
                        // replace na apóstrofe para não dar erro de string no SQL
                        aux += $"('{item.RazaoSocial.Replace("'", "''")}', '{item.CNPJ}', '{item.NomeMotorista.Replace("'", "''")}', '{item.CPF}', '{item.VigenciaCadastro.ToString("MM/dd/yyyy")}'),";
                    }
                }

                aux = aux.Remove(aux.Length - 1);
                _connSQL.Open();
                cmdSQL.CommandText = aux;
                cmdSQL.Connection = _connSQL;
                try
                {
                    cmdSQL.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
                _connSQL.Close();
            }
        }
    }
}