using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdooRpcWrapper.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var credentials = GetCredentials();
            var api = Connect(credentials);
            var invoiceModel = GetModel(api, "account.move", new List<string>{ "display_name" });
            Print(invoiceModel);
            Console.ReadKey();
        }

        private static OdooAPI Connect(OdooConnectionCredentials credentials)
        {
            return new OdooAPI(credentials);
        }

        private static OdooConnectionCredentials GetCredentials()
        {
            var clientOdoo = new
            {
                Host = "demo.odoo.com",
                Database = "demo",
                Username = "user@gmail.com",
                Password = "password"
            };

           return new OdooConnectionCredentials(clientOdoo.Host, clientOdoo.Database, clientOdoo.Username, clientOdoo.Password);
        }

        private static OdooModel GetModel(OdooAPI api, string model, List<string> fields)
        {
            var modelOdoo =  api.GetModel(model);
            modelOdoo.AddFields(fields);
            return modelOdoo;
        }

        private static void Print(OdooModel model)
        {
            var records = model.Search(null);
            foreach(var record in records)
            {
                Console.WriteLine(record.GetValue("display_name"));
            }
        }
    }
}
