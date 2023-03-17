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
            RenderTable();
        }
        HairdresserProgram HairdresserProgram = new HairdresserProgram();
        private void RenderTable()
        {
            int rowIndex = 1;
            List<Package> packageList = HairdresserProgram.ListPackage();
            foreach (Package package in packageList)
            {
                AddRowDef("table");

                if (rowIndex % 2 == 0) AddRowBgColor("green", "table", rowIndex, 3);
                else AddRowBgColor("yellow", "table", rowIndex, 3);

                AddTextBlock(package.Id.ToString(), "table", rowIndex, 0);
                AddTextBlock(package.Name.ToString(), "table", rowIndex, 1);
                rowIndex++;
            }

        }
        private void textBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {

            }
        }

        protected void AddRowDef(string gridName)
        {
            var grid = (Grid)this.FindName(gridName);
            RowDefinition RowDef = new RowDefinition();
            RowDef.Height = new GridLength(50);
            grid.RowDefinitions.Add(RowDef);
        }
        protected void AddRowBgColor(string color, string gridName, int rowIndex, int colSpan)
        {
            Rectangle bgRec = new Rectangle();
            bgRec.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            Grid.SetRow(bgRec, rowIndex);
            Grid.SetColumnSpan(bgRec, colSpan);
            var grid = (Grid)this.FindName(gridName);
            grid.Children.Add(bgRec);
        }
        protected void AddTextBlock(string inpString, string gridName, int rowIndex, int colIndex)
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
