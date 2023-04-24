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

namespace WPF_barber_proto
{
    /// <summary>
    /// Interaction logic for PackageSubpage.xaml
    /// </summary>
    public partial class PackageSubpage : UserControl
    {
        public PackageSubpage()
        {
            InitializeComponent();
            data.ItemsSource = HairdresserProgram.ListPackage();
        }
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Package package;
            if (AddBox.Text == "")
            {
                MessageBox.Show("Please fill in all the fields", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string value = AddBox.Text;
            List<Package> AlteredPackageList = new List<Package>();

            var InsertRecord = MessageBox.Show("Do you want to add " + value + " as a new package?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (InsertRecord == MessageBoxResult.Yes)
            {
                package = new Package(null, value);
                AlteredPackageList = HairdresserProgram.ListPackage();
                AlteredPackageList.Add(package);
                if (HairdresserProgram.SavePackageChanges(AlteredPackageList))
                    data.ItemsSource = HairdresserProgram.ListPackage();                    
                else
                {
                    data.ItemsSource = HairdresserProgram.ListPackage();
                    MessageBox.Show("Inserting data is conflicting with the database", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            AddBox.Text = "";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            bool isComplete = false;
            int countSelected = data.SelectedItems.Count;
            if (data.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + data.SelectedItems.Count + " Packages?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    foreach (var row in data.SelectedItems)
                    {
                        Package package = row as Package;
                        isComplete = HairdresserProgram.DeletePackage(package);
                    }
                    if (isComplete == false)
                        MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    data.ItemsSource = HairdresserProgram.ListPackage();
                }
            }
        }
        private void data_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Package pkg = e.Row.DataContext as Package;
            if (HairdresserProgram.UpdatePackage(pkg) == false)
                MessageBox.Show("Unable to execute query, remove linked data first to proceed.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {            
                if (IdSearchBox.Text == "" && NameSearchBox.Text == "")
                    data.ItemsSource = HairdresserProgram.ListPackage();
                else
                {
                    Package pkg = new Package(IdSearchBox.Text, NameSearchBox.Text);
                    data.ItemsSource = HairdresserProgram.SearchPackage(pkg);
                }
        }


        //VVVVVVVV Textblock solution VVVVVVVVVV


        //private void RenderTable()
        //{
        //    int rowIndex = 1;
        //    List<Package> packageList = HairdresserProgram.ListPackage();
        //    foreach (Package package in packageList)
        //    {
        //        AddRowDef("table");

        //        if (rowIndex % 2 == 0) AddRowBgColor("green", "table", rowIndex, 3);
        //        else AddRowBgColor("yellow", "table", rowIndex, 3);

        //        AddTextBlock(package.Id.ToString(), "table", rowIndex, 0);
        //        AddTextBlock(package.Name.ToString(), "table", rowIndex, 1);
        //        rowIndex++;
        //    }

        //}
        //protected void AddRowDef(string gridName)
        //{
        //    var grid = (Grid)this.FindName(gridName);
        //    RowDefinition RowDef = new RowDefinition();
        //    RowDef.Height = new GridLength(50);
        //    grid.RowDefinitions.Add(RowDef);
        //}
        //protected void AddRowBgColor(string color, string gridName, int rowIndex, int colSpan)
        //{
        //    Rectangle bgRec = new Rectangle();
        //    bgRec.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
        //    Grid.SetRow(bgRec, rowIndex);
        //    Grid.SetColumnSpan(bgRec, colSpan);
        //    var grid = (Grid)this.FindName(gridName);
        //    grid.Children.Add(bgRec);
        //}
        //protected void AddTextBlock(string inpString, string gridName, int rowIndex, int colIndex)
        //{
        //    TextBlock textBlock = new TextBlock();
        //    textBlock.Text = inpString;
        //    var grid = (Grid)this.FindName(gridName);
        //    Grid.SetRow(textBlock, rowIndex);
        //    Grid.SetColumn(textBlock, colIndex);
        //    textBlock.VerticalAlignment = VerticalAlignment.Center;
        //    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
        //    grid.Children.Add(textBlock);
        //}


    }
}
