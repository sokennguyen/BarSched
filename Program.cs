using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Reflection;

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
        public void RemoveStaffByName(string name)
        {
            myDB.DeleteStaff(name);    
        }

        public void DeleteStaff(Staff staff)
        {
            myDB.DeleteStaff(staff);
        }



        public bool SaveStaffChanges(List<Staff> staffList)
        {
            bool succeededSave=false;
            if (staffList.Count() > ListStaff().Count())
            {
                succeededSave=myDB.AddNewStaff(staffList[staffList.Count()-1].Name);
                succeededSave = true;
            }
            else if (staffList.Count() < ListStaff().Count())
            {
                succeededSave = myDB.AddNewStaff(staffList[staffList.Count() - 1].Name);
                return true;
            }
            else succeededSave = false;
            return succeededSave;
        }



        //public double GetBalance(Customer customer)
        //{
        //    string cID = customer.Id;
        //    return myDB.GetBalanceOfCustomer(cID);
        //}





        //List getters
        public List<Customer> ListCustomers()
        {
            return myDB.ListCustomers();
        }
        public List<Staff> ListStaff()
        {
            return myDB.ListStaff();
        }
        public List<Service> ListService()
        {
            return myDB.ListService();
        }
        public List<Package> ListPackage()
        {
            return myDB.ListPackage();
        }
        












        //public string StringAllCustomer()
        //{
        //    List<Customer> allCustomers = myDB.ListCustomers();

        //    StringBuilder custList = new StringBuilder();

        //    foreach (Customer c in allCustomers)
        //        custList.Append(c.ToString() + "\n");

        //    return custList.ToString();
        //}
    }
    
    class Customer
    {
        private string name;
        private int id;
        private string phone;
        public Customer(int id, string nm, string phone)
        {
            this.id = id;
            this.name = nm;            
            this.phone = phone;
        }

        public override string ToString()
        {
            return id + " : " + name + " : " + phone;
        }
        public int Id
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
    class Staff
    {
        private string id;
        private string name;   
        public Staff(string id, string nm)
        {
            this.id = id;
            this.name = nm;
        }

        public override string ToString()
        {
            return id + " : " + name;
        }
        public string Id
        {
            get { return id; }
            set { id= value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    class Service
    {
        static int packageCount=0;
        private string id;
        private string name;
        private int duration;
        private bool sink;
        private int package;
        private string packageName;
        public Service(string id, string nm, int duration, bool sink, int package, string packageName)
        {
            this.id = id;
            this.name = nm;
            this.duration= duration;
            this.sink = sink;
            this.package = package;
            this.packageName = packageName;
        }

        public override string ToString()
        {
            return id + " : " + name + " : " + duration + "mins : " + sink + ":" + package;
        }
        public string Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }
        public int Duration
        {
            get { return duration; }
        }
        public bool Sink
        {
            get { return sink; }
            set { sink=value; }
        }
        public int Package
        {
            get { return package; }
        }
        public string PackageName
        {
            get { return packageName; }
        }

    }
    class Package
    {
        private string id;
        private string name;
        public Package(string id, string nm)
        {
            this.id = id;
            this.name = nm;
        }

        public override string ToString()
        {
            return id + " : " + name;
        }
        public string Id
        {
            get { return id; }
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
            List<Customer> allCust = ListCustomers();
            int proposedID = 0;
            Boolean validFound = false;
            while (!validFound)
            {
                proposedID++;
                validFound = true;
                foreach (Customer c in allCust)
                {
                    if (c.Id == proposedID)
                    {
                        validFound = false;
                        break;
                    }
                }
            }
            return proposedID.ToString();
        }
        private string FindValidNewStaffId()
        {
            List<Staff> allStaff = ListStaff();
            int proposedID = 0;
            Boolean validFound = false;
            while (!validFound)
            {
                proposedID++;
                validFound = true;
                foreach (Staff s in allStaff)
                {
                    if (s.Id == proposedID.ToString())
                    {
                        validFound = false;
                        break;
                    }
                }
            }
            return proposedID.ToString();
        }













        public bool AddNewStaff(string name)
        {
            if (AddNewStaff(name, FindValidNewStaffId())) return true;
            return false;
        }

        private bool AddNewStaff(string name, string id)
        {
            try
            {
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO staff(staff_id, staff_name) VALUES ('" + id + "','" + name + "')";
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {

                throw new Exception();
                
            }
            
        }
        public void AddNewCustomer(string name, string area)
        {
            AddNewCustomer(name, area, FindValidNewStaffId());
        }

        private void AddNewCustomer(string name, string area, string id)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO Customer(Id, Name, Area, Balance) VALUES ('" + id + "','" + name + "','" + area + "','0,0')";
            command.ExecuteNonQuery();
        }





        

        //Public list getters        
        public List<Customer> ListCustomers()
        {
            MySqlDataReader reader = GetReaderData("SELECT customer_id, customer_name, customer_phone FROM customer ORDER BY customer_id");

            Boolean NotEOF = reader.Read();

            List<Customer> custList = new List<Customer>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["Id"].ToString());
                custList.Add(new Customer(Int32.Parse(reader["customer_id"].ToString()), reader["customer_name"].ToString(), reader["customer_phone"].ToString()));
                NotEOF = reader.Read();
            }
            reader.Close();
            return custList;
        }
        public List<Staff> ListStaff()
        {
            MySqlDataReader reader = GetReaderData("SELECT staff_id, staff_name FROM staff ORDER BY staff_id");

            Boolean NotEOF = reader.Read();

            List<Staff> staffList = new List<Staff>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["Id"].ToString());
                staffList.Add(new Staff(reader["staff_id"].ToString(), reader["staff_name"].ToString()));
                NotEOF = reader.Read();
            }
            reader.Close();
            return staffList;
        }
        public List<Service> ListService()
        {
            MySqlDataReader reader = GetReaderData("SELECT service_id, service_name, duration, sink_usage, ref_package_id, package_name FROM service LEFT JOIN package ON ref_package_id=package_id ORDER BY service_id");

            Boolean NotEOF = reader.Read();

            List<Service> serviceList = new List<Service>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["Id"].ToString());
                bool isUsingSink = false;
                if (reader["sink_usage"].ToString() == "1") isUsingSink = true;
                serviceList.Add(new Service(reader["service_id"].ToString(), reader["service_name"].ToString(), Int32.Parse(reader["duration"].ToString()), isUsingSink, Int32.Parse(reader["ref_package_id"].ToString()), reader["package_name"].ToString()));
                NotEOF = reader.Read();
            }
            return serviceList;
        }
        public List<Package> ListPackage()
        {
            MySqlDataReader reader = GetReaderData("SELECT package_id, package_name FROM package ORDER BY package_id");

            Boolean NotEOF = reader.Read();

            List<Package> packageList = new List<Package>();

            while (NotEOF)
            {
                //Console.WriteLine(reader["Name"].ToString() + ": " + reader["Id"].ToString());
                packageList.Add(new Package(reader["package_id"].ToString(), reader["package_name"].ToString()));
                NotEOF = reader.Read();
            }
            return packageList;
        }
        
















        //Public singular getters

        //public double GetPriceOfProduct(string key)
        //{
        //    return GetIntValueFromDB("Price", "Produt", "ProdID", key);
        //}
        //public double GetBalanceOfCustomer(string key)
        //{
        //    return GetIntValueFromDB("Balance", "Customer", "Id", key);
        //}
        public string GetCustomerName(string key)
        {
            return GetStringValueFromDB("customer_name", "customer", "customer_id", key);
        }
        public string GetStaffName(string key)
        {
            return GetStringValueFromDB("staff_name", "staff", "staff_id", key);
        }
        public string GetPackageName(string key)
        {
            return GetStringValueFromDB("package_name", "package", "package_id", key);
        }
        public string GetServiceName(string key)
        {
            return GetStringValueFromDB("service_name", "service", "service_id", key);
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






        //Build reader
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






        //Deletions
        public void DeleteCustomer(string name)
        {
            if (CustomerNameExists(name))
                DeleteRowFromTable("Customer", "Name", name);
        }
        public void DeleteStaff(string name)
        {
            if (StaffNameExists(name))
                DeleteRowFromTable("staff", "name", name);
        }
        public void DeleteStaff(Staff staff)
        {
            DeleteRowFromTable("staff", "staff_id", staff.Id.ToString());
        }





        private void DeleteRowFromTable(string from, string where, string rule)
        {
            string commandText = "DELETE FROM " + from + " WHERE " + where + " = '" + rule + "'";
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
        private void DeleteRowFromTable(string from, string where, int rule)
        {
            string commandText = "DELETE FROM " + from + " WHERE " + where + " = '" + rule.ToString() + "'";
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = commandText;

            command.ExecuteNonQuery();
        }










        //IsCustNameExist
        private bool CustomerNameExists(string name)
        {
            foreach (Customer c in ListCustomers())
                if (c.Name == name)
                    return true;
            return false;
        }
        private bool CustomerNameExists(int id)
        {
            foreach (Customer c in ListCustomers())
                if (c.Id == id)
                    return true;
            return false;
        }


        private bool StaffNameExists(string name)
        {
            foreach (Customer c in ListCustomers())
                if (c.Name == name)
                    return true;
            return false;
        }
        private bool StaffIdExists(int id)
        {
            foreach (Customer c in ListCustomers())
                if (c.Id == id)
                    return true;
            return false;
        }







    }

    class UI
    {
        
        private HairdresserProgram myHairDresserProgram;

        private void ListCustomers()
        {
            Console.WriteLine("Currently in the customer list we have:");
            //Console.WriteLine(myHairDresserProgram.StringAllCustomer());
        }


        private void AddNewCustomers()
        {
            while (true)
            {
                Console.WriteLine("With this program you can add new customer into the database");
                Console.WriteLine("Currently in the customer list we have:");
                //Console.WriteLine(myHairDresserProgram.StringAllCustomer());

                Console.WriteLine("Please enter the name of the new customer:");
                string name = Console.ReadLine();

                Console.WriteLine("Please enter the area of the new customer (north, south, west, east):");
                string area = Console.ReadLine();

                myHairDresserProgram.AddNewCustomer(name, area);
                Console.WriteLine("***********************************");
                Console.WriteLine("Currently in the customer list we have:");
                //Console.WriteLine(myHairDresserProgram.StringAllCustomer());

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
