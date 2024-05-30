using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;

namespace P_LINCB
{
    internal class Banco
    {
        private static BancoSQL _dbSQL = new BancoSQL();
        private static SqlConnection _connSQL = new SqlConnection(_dbSQL.Caminho());
        private static BancoMongo _dbMongo = new BancoMongo();

        public static void InsertToSQL(List<PenalidadesAplicadas> lst)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            // Inserção de registros por batch de 1000 registros
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

        public static void InsertLogSQL(string description, int records)
        {
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
            List<PenalidadesAplicadas> lst = new List<PenalidadesAplicadas>();

            #region SQL
            SqlCommand cmdSQL = new SqlCommand();

            _connSQL.Open();
            cmdSQL.CommandText = " SELECT RazaoSocial, CNPJ, NomeMotorista, CPF, DtVigencia FROM Tb_Dados2 "; ;
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
        public static int GetCountRecordsSQL()
        {
            int totalcount = 0;
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
                        totalcount = dr.GetInt32(0);
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

        public static void ProcessDataToMongoDB()
        {
            var client = new MongoClient(_dbMongo.Caminho());
            var db = client.GetDatabase("Motoristas");
            var collection = db.GetCollection<BsonDocument>("Dados");
            db.DropCollection("Dados");

            var listDocument = new List<BsonDocument>();

            foreach (var item in GetSQLRecordsList())
            {
                var document = new BsonDocument{
                            {"razao_social", item.RazaoSocial },
                            {"cnpj", item.CNPJ },
                            {"nome_motorista", item.NomeMotorista },
                            {"cpf", item.CPF },
                            {"vigencia_do_cadastro", item.VigenciaCadastro.ToString("dd/MM/yyyy") }
                };
                listDocument.Add(document);
            }
            collection.InsertMany(listDocument);
            InsertLogSQL("Cópia de registros do banco SQL Server para o MongoDB", GetCountRecordsSQL());
        }

        public static void ProcessDataToMongoDB_old()
        {
            var client = new MongoClient("mongodb://root:Mongo%402024%23@localhost:27017/");
            var db = client.GetDatabase("Motoristas");
            db.DropCollection("Dados");
            var collection = db.GetCollection<MotoristaHabilitado>("Dados");
            var document = GetSQLRecordsObj();
            collection.InsertOne(document);
            InsertLogSQL("Cópia de registros do banco SQL Server para o MongoDB", GetCountRecordsSQL());
        }
    }
}
