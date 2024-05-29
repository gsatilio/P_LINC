using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P_LINCB
{
    public class TestFilters
    {
        public static int getCountRecords(List<PenalidadesAplicadas> lista) => lista.Count();
        public static List<PenalidadesAplicadas> getRecordByDocumentStart(List<PenalidadesAplicadas> lista, string doc) => lista.Where(x => x.CPF.StartsWith(doc)).ToList();
        public static List<PenalidadesAplicadas> getRecordByYearOfValidity(List<PenalidadesAplicadas> lista, int year) => lista.Where(x => x.VigenciaCadastro.Year == year).ToList();
        public static List<PenalidadesAplicadas> getRecordByDescription(List<PenalidadesAplicadas> lista, string description) => lista.Where(x => x.RazaoSocial.ToLower().Contains(description.ToLower())).ToList();
        public static List<PenalidadesAplicadas> OrderByRazao(List<PenalidadesAplicadas> lista) => lista.OrderBy(x => x.RazaoSocial).ToList();

        //    Gerar XML baseado nos dados da lista
        public static string? ConvertToXML(List<PenalidadesAplicadas> lst)
        {
            if (lst.Count <= 0)
            {
                return null;
            }

            var penaltieApplied = new XElement("Root",
                    from data in lst
                    select new XElement("motorista",
                        new XElement("razao_social", data.RazaoSocial),
                        new XElement("cnpj", data.CNPJ),
                        new XElement("nome_motorista", data.NomeMotorista),
                        new XElement("cpf", data.CPF),
                        new XElement("data_de_vigencia", data.VigenciaCadastro)
                        )
                    );
            return penaltieApplied.ToString();
        }
    }
}
