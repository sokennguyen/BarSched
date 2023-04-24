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
using System.Collections;

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
        
        //Delete
        public bool DeleteCustomer(Customer customer)
        {
            return myDB.DeleteCustomer(customer);
        }
        public bool DeleteStaff(Staff staff)
        {
            return myDB.DeleteStaff(staff);
        }
        public bool DeleteService(Service service)
        {
            return myDB.DeleteService(service);
        }
        public bool DeletePackage(Package package)
        {
            return myDB.DeletePackage(package);
        }

        //Update
        public bool UpdateStaff(Staff staff)
        {
            return myDB.UpdateStaff(staff);
        }
        public bool UpdateCustomer(Customer customer)
        {
            return myDB.UpdateCustomer(customer);
        }
        public bool UpdateService(Service service)
        {
            return myDB.UpdateService(service);
        }
        public bool UpdatePackage(Package package)
        {
            return myDB.UpdatePackage(package);
        }

        //Add
        public bool SaveCustomerChanges(List<Customer> custList)
        {
            bool succeededSave = false;
            if (custList.Count() > ListCustomers().Count())
            {
                succeededSave = myDB.AddNewCustomer(custList[custList.Count() - 1].Name, custList[custList.Count() - 1].Phone);
            }
            else succeededSave = false;
            return succeededSave;
        }
        public bool SaveStaffChanges(List<Staff> staffList)
        {
            bool succeededSave = false;
            if (staffList.Count() > ListStaff().Count())
            {
                succeededSave = myDB.AddNewStaff(staffList[staffList.Count() - 1].Name);
            }
            else succeededSave = false;
            return succeededSave;
        }
        public bool SaveServiceChanges(List<Service> servList)
        {
            return myDB.AddNewService(servList[servList.Count() - 1].Name, servList[servList.Count() - 1].Duration, servList[servList.Count() - 1].Sink, servList[servList.Count() - 1].PackageId);
        }
        public bool SavePackageChanges(List<Package> packList)
        {
            bool succeededSave = false;
            if (packList.Count() > ListPackage().Count())
            {
                succeededSave = myDB.AddNewPackage(packList[packList.Count() - 1].Name);
            }
            else succeededSave = false;
            return succeededSave;
        }
        //Search
        public List<Staff> SearchStaff(Staff stf)
        {
            return myDB.SearchStaff(stf);
        }


        





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
            set { id = value;}
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
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
        
        private string id;
        private string name;
        private int duration;
        private bool sink;
        private int packageId;
        public Service(string id, string nm, int duration, bool sink, int package)
        {
            this.id = id;
            this.name = nm;
            this.duration= duration;
            this.sink = sink;
            this.packageId = package;
        }

        public override string ToString()
        {
            return id + " : " + name + " : " + duration + "mins : " + sink + ":" + packageId;
        }
        public string Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public bool Sink
        {
            get { return sink; }
            set { sink=value; }
        }
        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
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
            set { name = value;}
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
            connection = new MySqlConnection("Server=2.tcp.eu.ngrok.io;Port=11786;User ID=root;Database=barber");
            //connection = new MySqlConnection("Server=localhost;Port=3306;User ID=root;Database=barber");            
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
        private string FindValidNewServId()
        {
            List<Service> allServ = ListService();
            int proposedID = 0;
            Boolean validFound = false;
            while (!validFound)
            {
                proposedID++;
                validFound = true;
                foreach (Service s in allServ)
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
        private string FindValidNewPackId()
        {
            List<Package> allPack = ListPackage();
            int proposedID = 0;
            Boolean validFound = false;
            while (!validFound)
            {
                proposedID++;
                validFound = true;
                foreach (Package p in allPack)
                {
                    if (p.Id == proposedID.ToString())
                    {
                        validFound = false;
                        break;
                    }
                }
            }
            return proposedID.ToString();
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
            MySqlDataReader reader = GetReaderData("SELECT service_id, service_name, duration, sink_usage, ref_package_id FROM service ORDER BY service_id");

            Boolean NotEOF = reader.Read();

            List<Service> serviceList = new List<Service>();

            while (NotEOF)
            {
                serviceList.Add(new Service(reader["service_id"].ToString(), reader["service_name"].ToString(), Int32.Parse(reader["duration"].ToString()), bool.Parse(reader["sink_usage"].ToString()), Int32.Parse(reader["ref_package_id"].ToString()) ));
                NotEOF = reader.Read();
            }
            reader.Close();
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
            reader.Close();
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






        //Adding
        public bool AddNewStaff(string name)
        {
            if (AddNewStaff(name, FindValidNewStaffId())) return true;
            return false;
        }
        private bool AddNewStaff(string name, string id)
        {            
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO staff(staff_id, staff_name) VALUES ('" + id + "','" + name + "')";
                command.ExecuteNonQuery();
                return true;
        }

        public bool AddNewCustomer(string name, string phone)
        {
            return AddNewCustomer(name, phone, FindValidNewCustId());
        }
        private bool AddNewCustomer(string name, string phone, string id)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO customer(customer_id, customer_name, customer_phone) VALUES ('" + id + "','" + name + "','" + phone + "')";
            command.ExecuteNonQuery();
            return true;
        }

        public bool AddNewService(string name, int duration, bool sink, int package)
        {
            return AddNewService(name, duration, sink, package, FindValidNewServId());
        }
        private bool AddNewService(string name, int duration, bool sink, int package, string id)
        {
            int inpSink = 0;
            if (sink == true) inpSink = 1;
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO service(service_id, service_name, duration, sink_usage, ref_package_id) VALUES ('" + id + "','" + name + "','" + duration + "','" + inpSink.ToString() + "','" + package + "')";
            command.ExecuteNonQuery();
            return true;
        }

        public bool AddNewPackage(string name)
        {
            return (AddNewPackage(name, FindValidNewPackId())) ;
        }
        private bool AddNewPackage(string name, string id)
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO package(package_id, package_name) VALUES ('" + id + "','" + name + "')";
            command.ExecuteNonQuery();
            return true;
        }



        //Deletions
        public bool DeleteCustomer(Customer customer)
        {
            return DeleteRowFromTable("customer", "customer_id", customer.Id.ToString());
        }
        public bool DeleteStaff(Staff staff)
        {
            return DeleteRowFromTable("staff", "staff_id", staff.Id.ToString());
        }
        public bool DeleteService(Service service)
        {
            return DeleteRowFromTable("service", "service_id", service.Id.ToString());
        }
        public bool DeletePackage(Package package)
        {
            return DeleteRowFromTable("package", "package_id", package.Id.ToString());
        }



        private bool DeleteRowFromTable(string from, string where, string rule)
        {
            
            try
            {
                string commandText = "DELETE FROM " + from + " WHERE " + where + " = '" + rule + "'";
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        //Updating
        public bool UpdateStaff(Staff staff)
        {
            try
            {
                string commandText = "UPDATE staff SET staff_name='" + staff.Name + "' WHERE staff_id='" + staff.Id + "'";
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                string commandText = "UPDATE customer SET customer_name='" + customer.Name + "', customer_phone ='" + customer.Phone + "' WHERE customer_id='" + customer.Id + "'";
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }
        public bool UpdateService(Service service)
        {
            int isSink = 0;
            try
            {
                if (service.Sink == true) isSink = 1;                
                string commandText = "UPDATE service SET service_name='" + service.Name + "', duration ='" + service.Duration + "', sink_usage ='" + isSink.ToString() + "', ref_package_id ='" + service.PackageId + "' WHERE service_id='" + service.Id + "'";
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }
        public bool UpdatePackage(Package package)
        {
            try
            {
                string commandText = "UPDATE package SET package_name='" + package.Name + "' WHERE package_id='" + package.Id + "'";
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }



        //Search
        public List<Staff> SearchStaff(Staff stf)
        {
            List<Staff> foundList = new List<Staff>();
            //There should be only 1 matchable ID
            foreach (Staff s in ListStaff())
                if (s.Id == stf.Id)
                {
                    foundList.Add(s);
                    return foundList;
                }
            //Everything else can have multiple matches
            foreach (Staff s in ListStaff())
                if (s.Name.ToLower().Contains(stf.Name.ToLower()) && stf.Name != "")
                        foundList.Add(s);
            if (foundList.Count() == 0) return ListStaff();
            else return foundList;
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
