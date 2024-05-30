using Microsoft.Data.SqlClient;
using P_LINCB;
using System.Data;

int opt = -1;

var lst = ReadFile.GetData("C:\\5by5\\motoristas_habilitados.json");

/*
foreach (var item in lst)
{
    Console.WriteLine(item);
}
*/

do
{
    Console.Clear();
    Console.WriteLine("1 - Listar Registros que tenham o número do documento iniciando com 237 ");
    Console.WriteLine("2 - Listar Registros que tenham o ano de vigência em 2021 ");
    Console.WriteLine("3 - Quantas empresas tem no nome da razão social a descrição LTDA ");
    Console.WriteLine("4 - Ordenar a lista de registros pela razão social ");
    Console.WriteLine("5 - Inserir no SQL");
    Console.WriteLine("6 - Gerar XML");
    Console.WriteLine("7 - Inserir no MongoDB");
    Console.WriteLine("\nQuantidade de Linhas: " + TestFilters.getCountRecords(lst));
    opt = int.Parse(Console.ReadLine());
    switch (opt)
    {
        case 1:
            Console.WriteLine("\nListar Registros que tenham o número do documento iniciando com 237:");
            foreach (var item in TestFilters.getRecordByDocumentStart(lst, "237"))
            {
                Console.WriteLine(item);
            }
            break;
        case 2:
            Console.WriteLine("\nListar Registros que tenham o ano de vigência em 2021:");
            foreach (var item in TestFilters.getRecordByYearOfValidity(lst, 2021))
            {
                Console.WriteLine(item);
            }
            break;
        case 3:
            Console.WriteLine("\nQuantas empresas tem no nome da razão social a descrição LTDA:");
            Console.WriteLine(TestFilters.getRecordByDescription(lst, "LTDA").Count());
            break;
        case 4:
            Console.WriteLine("\nOrdenar a lista de registros pela razão social: ");
            foreach (var item in TestFilters.OrderByRazao(lst))
            {
                Console.WriteLine(item);
            }
            break;
        case 5:
            Console.WriteLine("Inserindo arquivo no SQL...");
            Banco.InsertToSQL(lst);
            break;
        case 6:
            Console.WriteLine(TestFilters.ConvertToXML(lst));
            break;
        case 7:
            Console.WriteLine("Inserindo registros no MongoDB...");
            Banco.ProcessDataToMongoDB();
            break;
    }
    Console.WriteLine("Continuar...");
    Console.ReadKey();
} while (opt != 0);

