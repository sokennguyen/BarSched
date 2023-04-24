using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for ServiceSubpage.xaml
    /// </summary>
    public partial class ServiceSubpage : UserControl
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        public ServiceSubpage()
        {
            InitializeComponent();
            data.ItemsSource = HairdresserProgram.ListService();

            //string dbug = "";
            //foreach (Service s in HairdresserProgram.ListService())
            //{
            //    dbug += s.ToString();
            //}
            //MessageBox.Show(dbug);
        }


        
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Service service;
            if (AddNameBox.Text == "" || AddDurationBox.Text == "" || AddPackageBox.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (int.Parse(AddPackageBox.Text) > HairdresserProgram.ListPackage().Count())
            {
                MessageBox.Show("This package doesn't exist", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string value = AddNameBox.Text;
            List<Service> AlteredServiceList = new List<Service>();

            var InsertRecord = MessageBox.Show("Do you want to add " + value + " as a new Service?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (InsertRecord == MessageBoxResult.Yes)
            {
                service = new Service(null, value, AddDurationBox.Text, AddSinkBox.IsChecked.Value, AddPackageBox.Text);
                AlteredServiceList = HairdresserProgram.ListService();
                AlteredServiceList.Add(service);
                if (HairdresserProgram.SaveServiceChanges(AlteredServiceList))
                {
                    data.ItemsSource = HairdresserProgram.ListService();
                }
                else
                {
                    data.ItemsSource = HairdresserProgram.ListService();
                    MessageBox.Show("Something went wrong", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            AddNameBox.Text = "";
            AddDurationBox.Text = "";
            AddSinkBox.IsChecked = false;
            AddPackageBox.Text = "";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            bool isComplete = false;
            int countSelected = data.SelectedItems.Count;
            if (data.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + data.SelectedItems.Count + " services?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    foreach (var row in data.SelectedItems)
                    {
                        Service service = row as Service;
                        isComplete = HairdresserProgram.DeleteService(service);
                    }
                    if (isComplete == false)
                        MessageBox.Show(countSelected + " services have being deleted!");
                    data.ItemsSource = HairdresserProgram.ListService();
                }
            }
        }
        private void data_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Service serv = e.Row.DataContext as Service;
            if (HairdresserProgram.UpdateService(serv) == false)
                MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Service serv = new Service(IdSearchBox.Text, NameSearchBox.Text, DurationSearchBox.Text, SinkSearchBox.IsChecked.Value, PackageSearchBox.Text);
            data.ItemsSource = HairdresserProgram.SearchService(serv);
        }
    }
}
