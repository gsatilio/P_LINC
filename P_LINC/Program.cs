// See https://aka.ms/new-console-template for more information
using P_LINC;

Console.WriteLine("Inicio do processamento");

var people = Adm.LoadData();
Adm.PrintData(people);

Console.WriteLine("\nTodas as pessoas que tem mais de 18 anos");
foreach(var p in Adm.FilterByAgeAdult(people))
{
    Console.WriteLine(p);
}

Console.WriteLine("\nTodas as pessoas que tem menos de 18 anos");
foreach (var p in Adm.FilterByAgeMinor(people))
{
    Console.WriteLine(p);
}

Console.WriteLine("\nTodas as pessoas que o nome inicia com a letra S");
foreach (var p in Adm.FilterByName(people, "S"))
{
    Console.WriteLine(p);
}

Console.WriteLine("\nListar todas as pessoas ordenar por nome");
foreach (var p in Adm.OrderByName(people))
{
    Console.WriteLine(p);
}

Console.WriteLine("\nListar todas as pessoas ordenar por nome descendente");
foreach (var p in Adm.OrderByDescName(people))
{
    Console.WriteLine(p);
}

Console.WriteLine("\nListar todas as pessoas que tenham a letra a no nome e o nome tem que ter mais de 3 caracteres");
foreach (var p in Adm.FilterByNameCharLength(people, "A"))
{
    Console.WriteLine(p);
}
