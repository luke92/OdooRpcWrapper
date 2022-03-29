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
            try
            {
                var credentials = GetCredentials();
                var api = Connect(credentials);
                PrintInvoiceV12(api);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static void PrintInvoiceV12(OdooAPI api)
        {
            Console.WriteLine("Invoice ODOO V12");
            var id = InputId("Enter Invoice Id");
            Console.WriteLine("Invoice ID: " + id);

            var fields = new List<string> { "name", "id" };
            var model = GetModel(api, "account.invoice", fields);

            //var filter = new object[] { new object[] { "id", id, "" } };

            Print(model, fields, null);
        }

        private static void PrintInvoices(OdooAPI api)
        {
            //Invoices
            //ODOO v14 Invoices
            Console.WriteLine("Invoices");
            var invoiceFields = new List<string> { "display_name" };
            var invoiceModel = GetModel(api, "account.move", invoiceFields);
            Print(invoiceModel, invoiceFields);
        }

        private static void PrintCustomers(OdooAPI api)
        {
            //Customers
            Console.WriteLine("Customers");
            var customerFields = new List<string> { "vat" };
            var customerModel = GetModel(api, "res.partner", customerFields);
            Print(customerModel, customerFields);
        }

        private static OdooAPI Connect(OdooConnectionCredentials credentials)
        {
            return new OdooAPI(credentials);
        }

        private static OdooConnectionCredentials GetCredentials()
        {
            var host = ConfigurationManager.AppSettings["host"];
            var database = ConfigurationManager.AppSettings["database"];
            var user = ConfigurationManager.AppSettings["username"];
            var pass = ConfigurationManager.AppSettings["password"];

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

        private static void Print(OdooModel model, List<string> fields, object[] filter = null)
        {
            if(filter == null)
                filter = new object[0];

            var records = model.Search(filter);
            foreach(var record in records)
            {
                Console.WriteLine("Id : " + record.Id);
                foreach(var field in fields)
                {
                    Console.WriteLine(field + " : " + record.GetValue(field));
                }
                Console.WriteLine();
            }
        }

        private static int InputId(string message)
        {
            var idString = "";
            var id = 0;
            do
            {
                Console.WriteLine(message);
                idString = Console.ReadLine();
            } while (!int.TryParse(idString, out id));
            return id;
        }
    }
}
