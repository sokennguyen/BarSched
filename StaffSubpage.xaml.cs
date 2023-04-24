using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for StaffSubpage.xaml
    /// </summary>
    public partial class StaffSubpage : UserControl
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        public StaffSubpage()
        {
            InitializeComponent();
            data.ItemsSource = HairdresserProgram.ListStaff();
        }



        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Staff staff;
            if (AddBox.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string value = AddBox.Text;
            List<Staff> AlteredStaffList = new List<Staff>();

            var InsertRecord = MessageBox.Show("Do you want to add " + value + " as a new staff?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (InsertRecord == MessageBoxResult.Yes)
            {
                staff = new Staff(null, value);
                AlteredStaffList = HairdresserProgram.ListStaff();
                AlteredStaffList.Add(staff);
                if (HairdresserProgram.SaveStaffChanges(AlteredStaffList))
                    data.ItemsSource = HairdresserProgram.ListStaff();
                else
                {
                    data.ItemsSource = HairdresserProgram.ListStaff();
                    MessageBox.Show("Something went wrong", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            AddBox.Text = "";
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            bool isComplete=false;
            int countSelected = data.SelectedItems.Count;
            if (data.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + data.SelectedItems.Count + " Employees?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    foreach (var row in data.SelectedItems)
                    {
                        Staff staff = row as Staff;
                        isComplete = HairdresserProgram.DeleteStaff(staff);
                    }
                    if (isComplete==false)
                        MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    data.ItemsSource = HairdresserProgram.ListStaff();
                }
            }


        }    
        private void data_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Staff stf = e.Row.DataContext as Staff;
            if (HairdresserProgram.UpdateStaff(stf)==false)
                MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (IdSearchBox.Text == "" && NameSearchBox.Text == "")
                data.ItemsSource = HairdresserProgram.ListStaff();
            else
            {
                Staff stf = new Staff(IdSearchBox.Text, NameSearchBox.Text);
                data.ItemsSource = HairdresserProgram.SearchStaff(stf);
            }
            
        }
    }
}
