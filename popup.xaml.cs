using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySqlConnector;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Windows.Markup;


namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for popup.xaml
    /// </summary>
    public partial class popup : Window
    {

        HairdresserProgram HairdresserProgram = new HairdresserProgram();



        public popup()
        {
            InitializeComponent();
            LoadDataq();





        }
        /*private void LoadData()
        {
            string connectionString = "server=localhost;user=root;database=barber;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT service_id, service_name FROM service;";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    ObservableCollection<DataRowView> data = new ObservableCollection<DataRowView>();
                    foreach (DataRowView rowView in dataTable.DefaultView)
                    {
                        data.Add(rowView);
                    }

                    serviceDropdown.ItemsSource = data;
                }
            }
            

        }*/
        private void LoadDataq()
        {
            string connectionString = "server=localhost;user=root;database=barber;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Load services
                string serviceQuery = "SELECT service_name FROM service;";
                using (MySqlDataAdapter serviceAdapter = new MySqlDataAdapter(serviceQuery, connection))
                {
                    DataTable serviceDataTable = new DataTable();
                    serviceAdapter.Fill(serviceDataTable);

                    ObservableCollection<DataRowView> serviceData = new ObservableCollection<DataRowView>();
                    foreach (DataRowView rowView in serviceDataTable.DefaultView)
                    {
                        serviceData.Add(rowView);
                    }

                    serviceDropdown.ItemsSource = serviceData;
                }

                // Load packages
                string packageQuery = "SELECT package_name FROM package;";
                using (MySqlDataAdapter packageAdapter = new MySqlDataAdapter(packageQuery, connection))
                {
                    DataTable packageDataTable = new DataTable();
                    packageAdapter.Fill(packageDataTable);

                    ObservableCollection<DataRowView> packageData = new ObservableCollection<DataRowView>();
                    foreach (DataRowView rowView in packageDataTable.DefaultView)
                    {
                        packageData.Add(rowView);
                    }

                    packageDropdown.ItemsSource = packageData;
                }
            }
        }


        private void serviceDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedService = serviceDropdown.SelectedItem.ToString();

        }
        private void packageDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedService = packageDropdown.SelectedItem.ToString();

        }




        private void button_Click(object sender, RoutedEventArgs e)
        {
            string service = serviceDropdown.SelectedItem != null ? ((DataRowView)serviceDropdown.SelectedItem)["service_name"].ToString() : "";
            string package = packageDropdown.SelectedItem != null ? ((DataRowView)packageDropdown.SelectedItem)["package_name"].ToString() : "";
            string staff = Staff.Text;
            string starttime = Starttime.Text;
            string customer = newcust.Text;

            string sentence = $"{customer}, {staff}, {starttime}- {service} {package}";

            // Add the sentence to the listbox
            appointmentsListBox.Items.Add(sentence);

            Appointment appoinment;
            if (service == "" || package == "" || newcust.Text == "" || Staff.Text == ""||Starttime.Text=="")
            {
                MessageBox.Show("Please fill in all the fields", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string value = service;
            List<Appointment> AlteredAppointmentList = new List<Appointment>();

            var InsertRecord = MessageBox.Show("Do you want to add this appointment?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (InsertRecord == MessageBoxResult.Yes)
            {
                appoinment = new Appointment(null, customer, Staff.Text, package, Starttime.Text, service);
                AlteredAppointmentList = HairdresserProgram.ListAppointment();
                AlteredAppointmentList.Add(appoinment);
                if (!HairdresserProgram.SaveAppointmentChanges(AlteredAppointmentList))               
                {
                    MessageBox.Show("Inserting data is conflicting with the database", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            
            

            // Close the pop-up window
            this.Close();
            //string connectionString = "server=localhost;user=root;database=barber;";
            //using (MySqlConnection connection = new MySqlConnection(connectionString))
            //{
            //    connection.Open();

            //    /*string customerName = newcust.Text;*/
            //    string query = " INSERT INTO appointment_sevice(service_id) VALUES( @serviceId); ";
            //    using (MySqlCommand command = new MySqlCommand(query, connection))
            //    {

            //        command.Parameters.AddWithValue("@serviceId", serviceDropdown.SelectedValue);

            //        int rowsAffected = command.ExecuteNonQuery();

            //        if (rowsAffected > 0)
            //        {
            //            MessageBox.Show("Appointment saved successfully.");
            //        }
            //    }
            //}
            /*string customerId = "";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT customer_id FROM customer WHERE customer_name = '{newcust.Text}'";
                MySqlCommand command = new MySqlCommand(query, connection);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    customerId = result.ToString();
                }
            }
            string insertQuery = "INSERT INTO customer (customer_name) VALUES (@customerName);" +
                     "INSERT INTO appointment (customer_id, service_id) VALUES (LAST_INSERT_ID(), @serviceId);";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                   /* command.Parameters.AddWithValue("@customerName", newcust.Text);
                    command.Parameters.AddWithValue("@serviceId", serviceDropdown.SelectedValue);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Appointment saved successfully.");
                    }
                }
            }*/


        }











    }



}




