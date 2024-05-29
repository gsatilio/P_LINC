using System.Threading.Channels;

namespace P_LINC
{
    public class Person
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public int Age { get; set; }

        public override string ToString() => $"Id: {Id} ".PadRight(5) + $"Nome: {Name}".PadRight(20) + $"Telefone: {Telephone}".PadRight(20) + $"Idade: {Age}".PadRight(20);
    }
}
