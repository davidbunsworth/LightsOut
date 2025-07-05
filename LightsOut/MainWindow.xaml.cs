using System;
using System.Data.Common;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightsOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int gridSize = 2;
        private static readonly SolidColorBrush LitBrush = new SolidColorBrush(Colors.White);
        private static readonly SolidColorBrush UnlitBrush = new SolidColorBrush(Colors.Black);
        private Button[,] buttons = new Button[gridSize, gridSize];
        private bool hasWon = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = this.Width = gridSize * 50;
            Random random = new Random();

            // Add rows
            for (int i = 0; i < gridSize; i++)
            {
                LightsGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Add columns
            for (int j = 0; j < gridSize; j++)
            {
                LightsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Construct grid
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    Button btn = new Button();
                    btn.Background = random.Next(2) == 0 ? LitBrush : UnlitBrush;
                    Grid.SetColumn(btn, col);
                    Grid.SetRow(btn, row);
                    btn.Click += btnLight_Click;
                    LightsGrid.Children.Add(btn);
                    buttons[row, col] = btn;
                }
            }
        }

        private void btnLight_Click(object sender, RoutedEventArgs e)
        {
            if (hasWon) // Don't continue after winning
                return;

            if (sender is Button btn)
            {
                int row = Grid.GetRow(btn);
                int col = Grid.GetColumn(btn);

                // Self
                ToggleLight(buttons[row, col]);

                if (col > 0) // West
                    ToggleLight(buttons[row, col - 1]);

                if (col < gridSize - 1) // East
                    ToggleLight(buttons[row, col + 1]);

                if (row > 0) // North
                    ToggleLight(buttons[row - 1, col]);

                if (row < gridSize - 1) // South
                    ToggleLight(buttons[row + 1, col]);

                CheckLightsOut();
            }

        }

        private void ToggleLight(Button? btn)
        {
            if (btn != null)
            {
                btn.Background = IsLit(btn) ? UnlitBrush : LitBrush;
            }
        }

        private void CheckLightsOut()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (IsLit(buttons[i, j]))
                        return;
                }
            }

            hasWon = true;
            MessageBox.Show("WINNER!");
        }

        private bool IsLit(Button btn)
        {
            var brush = btn.Background as SolidColorBrush;

            if (brush != null)
            {
                return brush.Color == Colors.White;
            }

            return false;
        }
    }
}