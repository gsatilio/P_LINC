using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;

namespace P_LINCB
{
    public class BancoMongo
    {
        public static void ProcessDataToMongoDB()
        {
            var client = new MongoClient("mongodb://root:Mongo%402024%23@localhost:27017/");
            var db = client.GetDatabase("Motoristas");
            /*
            var collection = db.GetCollection<PenalidadesAplicadas>("Dados");
            foreach (var item in BancoSQL.GetSQLRecordsList())
            {
                collection.InsertOneAsync(item);
            }
            */

            var collection = db.GetCollection<MotoristaHabilitado>("Dados");
            db.DropCollection("Dados");
            var document = BancoSQL.GetSQLRecordsObj();
            collection.InsertOneAsync(document);
            BancoSQL.InsertLog("Cópia de registros do banco SQL Server para o MongoDB", BancoSQL.GetCountRecords());
        }
    }
}
