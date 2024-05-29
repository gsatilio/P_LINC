using System.Collections.Generic;

namespace P_LINC
{
    public class Adm
    {
        public static void PrintData(List<Person> person)
        {
            foreach (var p in person)
            {
                Console.WriteLine(p);
            }
        }
        public static List<Person> LoadData()
        {
            var people = new List<Person>();
            people.Add(new Person() { Id = 1, Name = "Maria", Age = 15, Telephone = "989894" });
            people.Add(new Person() { Id = 2, Name = "José", Age = 20, Telephone = "5105151" });
            people.Add(new Person() { Id = 3, Name = "Guilherme", Age = 30, Telephone = "12121212" });
            people.Add(new Person() { Id = 4, Name = "Joao", Age = 25, Telephone = "9842211" });
            people.Add(new Person() { Id = 5, Name = "Simone", Age = 17, Telephone = "230320325" });
            people.Add(new Person() { Id = 6, Name = "Silvinho", Age = 95, Telephone = "000" });
            return people;
        }

        public static List<Person> FilterByAgeAdult(List<Person> people) => people.FindAll(x => x.Age >= 18);
        public static List<Person> FilterByAgeMinor(List<Person> people) => people.Where(x => x.Age < 18).ToList();
        public static List<Person> FilterByName(List<Person> people, string nome) => people.FindAll(x => x.Name.StartsWith(nome));
        public static List<Person> OrderByName(List<Person> people) => people.OrderBy(x => x.Name).ToList();
        public static List<Person> OrderByDescName(List<Person> people) => people.OrderByDescending(x => x.Name).ToList();
        public static List<Person> FilterByNameCharLength(List<Person> people, string nome) => people.FindAll(x => x.Name.ToLower().IndexOf(nome.ToLower()) != -1 && x.Name.Length > 3);
        //public static List<Person> FilterByNameCharLength(List<Person> people, string nome) => people.FindAll(x => x.Name.ToLower().Contains(nome.ToLower()) && x.Name.Length > 3);

    }
}
