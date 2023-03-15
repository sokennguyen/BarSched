using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

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
            string cID = customer.Id;
            return myDB.GetBalanceOfCustomer(cID);
        }

        public List<Customer> ListCustomers()
        {
            return myDB.ListCustomers();
        }

        public string StringAllCustomer()
        {
            List<Customer> allCustomers = myDB.ListCustomers();

            StringBuilder custList = new StringBuilder();

            foreach (Customer c in allCustomers)
                custList.Append(c.ToString() + "\n");

            return custList.ToString();
        }
    }

    class Customer
    {
        private string name;
        private string id;
        private string phone;
        public Customer(string id, string nm, string phone)
        {
            this.id = id;
            this.name = nm;            
            this.phone = phone;
        }

        public override string ToString()
        {
            return id + " : " + name + " : " + phone;
        }
        public string Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }
        public string Phone
        {
            get { return phone; }
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
            List<Customer> allCust = ListCustomers();
            int proposedID = 0;
            Boolean validFound = false;
            while (!validFound)
            {
                proposedID++;
                validFound = true;
                foreach (Customer c in allCust)
                {
                    if (c.Id == proposedID.ToString())
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
            command.CommandText = "INSERT INTO Customer(Id, Name, Area, Balance) VALUES ('" + id + "','" + name + "','" + area + "','0,0')";
            command.ExecuteNonQuery();
        }







        //Public listed getters        
        public List<Customer> ListCustomers()
        {
            MySqlDataReader reader = GetReaderData("SELECT customer_id, customer_name, customer_phone FROM customer ORDER BY customer_id");

            Boolean NotEOF = reader.Read();

            List<Customer> custList = new List<Customer>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["Id"].ToString());
                custList.Add(new Customer(reader["customer_id"].ToString(), reader["customer_name"].ToString(), reader["customer_phone"].ToString()));
                NotEOF = reader.Read();
            }
            return custList;
        }
        















        //Public singular getters
        public double GetPriceOfProduct(string key)
        {
            return GetIntValueFromDB("Price", "Produt", "ProdID", key);
        }
        public double GetBalanceOfCustomer(string key)
        {
            return GetIntValueFromDB("Balance", "Customer", "Id", key);
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
            foreach (Customer c in ListCustomers())
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


        

    }

    class UI
    {
        
        private HairdresserProgram myHairDresserProgram;

        private void ListCustomers()
        {
            Console.WriteLine("Currently in the customer list we have:");
            Console.WriteLine(myHairDresserProgram.StringAllCustomer());
        }


        private void AddNewCustomers()
        {
            while (true)
            {
                Console.WriteLine("With this program you can add new customer into the database");
                Console.WriteLine("Currently in the customer list we have:");
                Console.WriteLine(myHairDresserProgram.StringAllCustomer());

                Console.WriteLine("Please enter the name of the new customer:");
                string name = Console.ReadLine();

                Console.WriteLine("Please enter the area of the new customer (north, south, west, east):");
                string area = Console.ReadLine();

                myHairDresserProgram.AddNewCustomer(name, area);
                Console.WriteLine("***********************************");
                Console.WriteLine("Currently in the customer list we have:");
                Console.WriteLine(myHairDresserProgram.StringAllCustomer());

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
                        ListCustomers();
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
