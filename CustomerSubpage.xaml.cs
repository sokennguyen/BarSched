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
    public partial class CustomerSubpage : UserControl
    {
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        public CustomerSubpage()
        {
            InitializeComponent();
            RenderTable();
        }
        private void RenderTable()
        {
            int rowIndex = 1;
            List<Customer> custList = HairdresserProgram.ListCustomers();
            foreach (Customer customer in custList)
            {
                AddRowDef("table");
                AddTextBlock(customer.Id.ToString(), "table",rowIndex,0);
                AddTextBlock(customer.Name.ToString(), "table", rowIndex,1);
                AddTextBlock(customer.Phone.ToString(), "table", rowIndex,2);
                rowIndex++;
            }
        }
        private void AddRowDef(string gridName)
        {
            var grid = (Grid)this.FindName(gridName);
            RowDefinition RowDef = new RowDefinition();
            RowDef.Height = new GridLength(50);
            grid.RowDefinitions.Add(RowDef);
        }
        private void AddTextBlock(string inpString, string gridName, int rowIndex, int colIndex)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = inpString;
            var grid = (Grid)this.FindName(gridName);
            Grid.SetRow(textBlock, rowIndex);
            Grid.SetColumn(textBlock, colIndex);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Children.Add(textBlock);
        }
    }
}
