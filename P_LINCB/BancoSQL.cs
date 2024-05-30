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
    }
}
