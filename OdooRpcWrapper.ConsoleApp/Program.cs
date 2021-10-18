using System;
using System.Collections.Generic;
using System.Configuration;
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
            //ODOO 14 Invoices
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
            var host = ConfigurationManager.AppSettings["host"];
            var database = ConfigurationManager.AppSettings["database"];
            var user = ConfigurationManager.AppSettings["user"];
            var pass = ConfigurationManager.AppSettings["pass"];

            var clientOdoo = new
            {
                Host = host,
                Database = database,
                Username = user,
                Password = pass
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
            var filter = new object[0];
            var records = model.Search(filter);
            foreach(var record in records)
            {
                Console.WriteLine(record.GetValue("display_name"));
            }
        }
    }
}
