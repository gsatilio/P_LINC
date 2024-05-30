using MongoDB.Bson;
using MongoDB.Driver;

namespace P_LINCB
{
    public class BancoMongo
    {
        public static void ProcessDataToMongoDB()
        {

            var client = new MongoClient("mongodb://root:Mongo%402024%23@localhost:27017/");
            var db = client.GetDatabase("Motoristas");
            var collection = db.GetCollection<BsonDocument>("Dados");
            db.DropCollection("Dados");

            var listDocument = new List<BsonDocument>();

            foreach (var item in BancoSQL.GetSQLRecordsList())
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
            BancoSQL.InsertLog("Cópia de registros do banco SQL Server para o MongoDB", BancoSQL.GetCountRecords());
        }

        public static void ProcessDataToMongoDB_old()
        {

            var client = new MongoClient("mongodb://root:Mongo%402024%23@localhost:27017/");
            var db = client.GetDatabase("Motoristas");
            db.DropCollection("Dados");
            var collection = db.GetCollection<MotoristaHabilitado>("Dados");
            var document = BancoSQL.GetSQLRecordsObj();
            collection.InsertOne(document);
            BancoSQL.InsertLog("Cópia de registros do banco SQL Server para o MongoDB", BancoSQL.GetCountRecords());
        }
    }
}
