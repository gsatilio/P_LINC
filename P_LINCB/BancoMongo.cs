namespace P_LINCB
{
    internal class BancoMongo
    {
        readonly string Conexao = "mongodb://root:Mongo%402024%23@localhost:27017/";

        public BancoMongo()
        {

        }
        public string Caminho()
        {
            return Conexao;
        }
    }
}
