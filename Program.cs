using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_barber_proto
{
    class HairdresserProgram
    {
        private DBService myDB;

        public HairdresserProgram()
        {
            myDB = new DBService();
        }

        public void CloseProgram()
        {
            myDB.CloseDB();
        }

        public void AddNewCustomer(string name, string area)
        {
            myDB.AddNewCustomer(name, area);
        }

        public void RemoveCustomerByName(string name)
        {
            myDB.DeleteCustomer(name);
        }

        public double GetBalance(Customer customer)
        {
            string cID = customer.CustID;
            return myDB.GetBalanceOfCustomer(cID);
        }

        public List<Customer> GetCustomers()
        {
            //DBService myDataBase = new DBService();
            return myDB.ListAllCustomersSorted();
        }

        public string ListAllCustomers()
        {
            //DBService myDataBase = new DBService();
            List<Customer> allCustomers = myDB.ListAllCustomersSorted();

            StringBuilder custList = new StringBuilder();

            foreach (Customer c in allCustomers)
                custList.Append(c.ToString() + "\n");

            return custList.ToString();
        }
    }

    class Customer
    {
        private string name;
        private string custID;

        public Customer(string nm, string id)
        {
            name = nm;
            custID = id;
        }

        public override string ToString()
        {
            return name + ": " + custID;
        }
        public string CustID
        {
            get { return custID; }
        }

        public string Name
        {
            get { return name; }
        }

    }

    class DBService
    {
        private MySqlConnection connection;
        public DBService()
        {
            OpenConnection();
        }

        public void CloseDB()
        {
            connection.Close();
        }
        private void OpenConnection()
        {
            //for final iteration
            //MySqlConnection connection = new MySqlConnection("Server=6.tcp.eu.ngrok.io;Port=12710;User ID=root;Database=ds_assignment_auction");
            connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Database=barber");            
            connection.Open();
        }

        private string FindValidNewCustId()
        {
            List<Customer> allCust = ListAllCustomers();
            int proposedID = 0;
            Boolean validFound = false;
            while (!validFound)
            {
                proposedID++;
                validFound = true;
                foreach (Customer c in allCust)
                {
                    if (c.CustID == proposedID.ToString())
                    {
                        validFound = false;
                        break;
                    }
                }
            }
            return proposedID.ToString();
        }

        public void AddNewCustomer(string name, string area)
        {
            AddNewCustomer(name, area, FindValidNewCustId());
        }

        private void AddNewCustomer(string name, string area, string id)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO Customer(CustID, Name, Area, Balance) VALUES ('" + id + "','" + name + "','" + area + "','0,0')";
            command.ExecuteNonQuery();
        }        

        public double GetPriceOfProduct(string key)
        {
            return GetIntValueFromDB("Price", "Produt", "ProdID", key);
        }


        public double GetBalanceOfCustomer(string key)
        {
            return GetIntValueFromDB("Balance", "Customer", "CustID", key);
        }

        public string GetCustomerName(string key)
        {
            return GetStringValueFromDB("customer_name", "customer", "customer_id", key);
        }

        //Query Writer
        private double GetIntValueFromDB(string what, string from, string where, string rule)
        {
            string commandText = "SELECT " + what + " FROM " + from + " WHERE " + where + " = " + rule;
            return GetIntData(commandText, what);
        }
        private string GetStringValueFromDB(string what, string from, string where, string rule)
        {
            string commandText = "SELECT " + what + " FROM " + from + " WHERE " + where + " = " + rule;
            return GetStringData(commandText, what);
        }





        //DB Fetcher
        private double GetIntData(string query, string what)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            Boolean NotEOF = reader.Read();

            double retVal = 0.0;

            if (NotEOF)
                retVal = Convert.ToDouble(reader[what].ToString());
            reader.Close();
            return retVal;
        }
        private string GetStringData(string query, string what)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            Boolean NotEOF = reader.Read();

            string retVal = "";

            if (NotEOF)
                retVal = reader[what].ToString();
            reader.Close();
            return retVal;
        }







        private MySqlDataReader GetReaderData(string query)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = query;
            command.CommandType = CommandType.Text;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            return reader;
        }


        public void DeleteCustomer(string name)
        {
            if (CustomerNameExists(name))
                DeleteRowFromTable("Customer", "Name", name);
        }

        private bool CustomerNameExists(string name)
        {
            foreach (Customer c in ListAllCustomers())
                if (c.Name == name)
                    return true;
            return false;
        }

        private void DeleteRowFromTable(string from, string where, string rule)
        {
            string commandText = "DELETE FROM " + from + " WHERE " + where + " = '" + rule + "'";
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = commandText;

            command.ExecuteNonQuery();

        }


        public List<Customer> ListAllCustomers()
        {
            MySqlDataReader reader = GetReaderData("SELECT CustID, Name FROM Customer");

            Boolean NotEOF = reader.Read();

            List<Customer> custList = new List<Customer>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["CustID"].ToString());
                custList.Add(new Customer(reader["Name"].ToString(), reader["CustID"].ToString()));
                NotEOF = reader.Read();
            }

            return custList;
        }

        public List<Customer> ListAllCustomersSorted()
        {
            MySqlDataReader reader = GetReaderData("SELECT CustID, Name FROM Customer ORDER BY CustID");

            Boolean NotEOF = reader.Read();

            List<Customer> custList = new List<Customer>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["CustID"].ToString());
                custList.Add(new Customer(reader["Name"].ToString(), reader["CustID"].ToString()));
                NotEOF = reader.Read();
            }

            return custList;
        }

    }

    class UI
    {
        private HairdresserProgram myHairDresserProgram;

        private void ListAllCustomers()
        {
            Console.WriteLine("Currently in the customer list we have:");
            Console.WriteLine(myHairDresserProgram.ListAllCustomers());
        }


        private void AddNewCustomers()
        {
            while (true)
            {
                Console.WriteLine("With this program you can add new customer into the database");
                Console.WriteLine("Currently in the customer list we have:");
                Console.WriteLine(myHairDresserProgram.ListAllCustomers());

                Console.WriteLine("Please enter the name of the new customer:");
                string name = Console.ReadLine();

                Console.WriteLine("Please enter the area of the new customer (north, south, west, east):");
                string area = Console.ReadLine();

                myHairDresserProgram.AddNewCustomer(name, area);
                Console.WriteLine("***********************************");
                Console.WriteLine("Currently in the customer list we have:");
                Console.WriteLine(myHairDresserProgram.ListAllCustomers());

                Console.WriteLine("***********************************");
                Console.WriteLine("Do you want to continue adding new customers? (Y/N)");
                string cont = Console.ReadLine();
                if (cont != "Y")
                    break;

                Console.Clear();
            }
        }

        public void Run()
        {
            myHairDresserProgram = new HairdresserProgram();
            while (true)
            {
                Console.WriteLine("With this program you can:");
                Console.WriteLine("A. List all customers in the database");
                Console.WriteLine("B. Add a new customer in the database");
                Console.WriteLine("What do you want to do? (A/B)");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "A":
                        ListAllCustomers();
                        break;
                    case "B":
                        AddNewCustomers();
                        break;
                    default:
                        Console.WriteLine("Invalid selection, do you want to end the program (Y/N)?");
                        if (Console.ReadLine() == "Y")
                            return;
                        break;
                }
            }
        }
    }   
}
