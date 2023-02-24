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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;
using System.Windows.Media.Animation;
using MySqlConnector;

namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void ButtonFirst_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=4.tcp.eu.ngrok.io;Port=19082;User ID=root;Database=ds_assignment_auction");
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT auction_id,product_name,auction_start_time,auction_end_time FROM auction",connection);
            Week_Schedule.ItemsSource=command.ExecuteReader();
            Week_Schedule.Visibility = Visibility.Visible;
        }
        private void ButtonSecond_Click(object sender, RoutedEventArgs e)
        {
            Week_Schedule.Visibility= Visibility.Collapsed;
        }
        private OleDbConnection OpenDB()
        {
            string connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = C:\repos\access_db\barberDB.mdb";
            OleDbConnection connection = new OleDbConnection(connectionStr);
            connection.Open();
            return connection;
        }

    }
}
