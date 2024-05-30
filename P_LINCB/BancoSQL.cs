using Microsoft.Data.SqlClient;
using System.Data;

namespace P_LINCB
{
    internal class BancoSQL
    {
        readonly string Conexao = "Data Source=127.0.0.1; Initial Catalog=dbMotoristas; User Id=SA; Password=SqlServer2019!;TrustServerCertificate=True";

        public BancoSQL()
        {

        }
        public string Caminho()
        {
            return Conexao;
        }

        public static void InserirSQLProcedure(SqlConnection _connSQL, PenalidadesAplicadas obj)
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
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            // Inserção de registros por batch de 1000 registros
            BancoSQL _dbSQL = new BancoSQL();
            SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
            SqlCommand cmdSQL = new SqlCommand();

            int count = lst.Count();    // Qtde de registros na minha lista  (ex 47.590)
            int batchSize = 1000;       // Tamanho do batch (ex 1000)
            int batches = (int)Math.Floor((double)count / batchSize);   // Qtde de batches por batchSize (ex 47.590 / 1000 = 47 batches)

            #region SQL
            _connSQL.Open();
            cmdSQL.CommandText = " DELETE FROM Tb_Dados2 ";
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
            #endregion


            for (int j = 0; j <= batches; j++)
            {
                string aux = " INSERT INTO Tb_Dados2 (RazaoSocial, CNPJ, NomeMotorista, CPF, DtVigencia) VALUES ";

                foreach (var item in lst.Skip(batchSize * j).Take(batchSize))
                {
                    if (item.RazaoSocial != null)
                    {
                        // replace na apóstrofe para não dar erro de string no SQL
                        aux += $"('{item.RazaoSocial.Replace("'", "''")}', " +
                                $"'{item.CNPJ}', " +
                                $"'{item.NomeMotorista.Replace("'", "''")}', " +
                                $"'{item.CPF}', " +
                                $"'{item.VigenciaCadastro.ToString("MM/dd/yyyy")}'),";
                    }
                }
                // Ex. irá ficar Insert Into Tabela (....) Values (xxxx), (yyyy), (zzzz), ....
                #region SQL
                _connSQL.Open();
                cmdSQL.CommandText = aux.Substring(0, aux.Length - 1); // Remove a ultima virgula da string de insert values
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
                #endregion
            }
            watch.Stop();
            Console.WriteLine($"Levou {watch.ElapsedMilliseconds} milissegundos!");
        }

        public static void InsertLog(string description, int records)
        {
            BancoSQL _dbSQL = new BancoSQL();
            SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
            SqlCommand cmdSQL = new SqlCommand();
            string aux = " INSERT INTO Controle_Processamento (Description, Date, NumberOfRecords) VALUES ";
            aux += $"('{description}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {records})";

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

        public static MotoristaHabilitado GetSQLRecordsObj()
        {
            MotoristaHabilitado obj = new MotoristaHabilitado();
            List<PenalidadesAplicadas> lst = new List<PenalidadesAplicadas> ();

            #region SQL
            BancoSQL _dbSQL = new BancoSQL();
            SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
            SqlCommand cmdSQL = new SqlCommand();
            string aux = " SELECT RazaoSocial, CNPJ, NomeMotorista, CPF, DtVigencia FROM Tb_Dados2 ";

            _connSQL.Open();
            cmdSQL.CommandText = aux;
            cmdSQL.Connection = _connSQL;
            try
            {
                using (SqlDataReader dr = cmdSQL.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lst.Add(new PenalidadesAplicadas(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetDateTime(4)));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            _connSQL.Close();
            #endregion
            obj.PenalidadesAplicadas = lst;
            return obj;
        }

        public static List<PenalidadesAplicadas> GetSQLRecordsList()
        {
            List<PenalidadesAplicadas> lst = new List<PenalidadesAplicadas>();

            BancoSQL _dbSQL = new BancoSQL();
            SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
            SqlCommand cmdSQL = new SqlCommand();
            string aux = " SELECT RazaoSocial, CNPJ, NomeMotorista, CPF, DtVigencia FROM Tb_Dados2 ";

            _connSQL.Open();
            cmdSQL.CommandText = aux;
            cmdSQL.Connection = _connSQL;
            try
            {
                using (SqlDataReader dr = cmdSQL.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lst.Add(new PenalidadesAplicadas(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetDateTime(4)));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            _connSQL.Close();
            return lst;
        }
        public static int GetCountRecords()
        {
            int totalcount = 0;
            BancoSQL _dbSQL = new BancoSQL();
            SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
            SqlCommand cmdSQL = new SqlCommand();
            string aux = " SELECT COUNT(*) FROM Tb_Dados2 ";

            _connSQL.Open();
            cmdSQL.CommandText = aux;
            cmdSQL.Connection = _connSQL;
            try
            {
                using (SqlDataReader dr = cmdSQL.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        totalcount =  dr.GetInt32(0);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            _connSQL.Close();
            return totalcount;
        }
    }
}
