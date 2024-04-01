using System;
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

namespace mychess
{
    public partial class MainWindow : Window
    {
        private const int BoardSize = 8;
        private const int CellSize = 50;

        private string[,] board;
        private bool isWhiteTurn = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            DrawBoard();
        }

        private void InitializeBoard()
        {
            board = new string[BoardSize, BoardSize];

            for (int i = 0; i < BoardSize; i++)
            {
                board[1, i] = "B";
                board[6, i] = "W";
            }
        }

        private void DrawBoard()
        {
            ChessGrid.Children.Clear();

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Fill = (row + col) % 2 == 0 ? Brushes.White : Brushes.Gray
                    };

                    Grid.SetRow(rect, row);
                    Grid.SetColumn(rect, col);
                    ChessGrid.Children.Add(rect);

                    if (!string.IsNullOrEmpty(board[row, col]))
                    {
                        TextBlock textBlock = new TextBlock
                        {
                            Text = board[row, col],
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };

                        Grid.SetRow(textBlock, row);
                        Grid.SetColumn(textBlock, col);
                        ChessGrid.Children.Add(textBlock);
                    }
                }
            }
        }

        private void MovePawn(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (IsValidMove(fromRow, fromCol, toRow, toCol))
            {
                board[toRow, toCol] = board[fromRow, fromCol];
                board[fromRow, fromCol] = null;
                DrawBoard();
                isWhiteTurn = !isWhiteTurn;
            }
            else
            {
                MessageBox.Show("Invalid move!");
            }
        }

        private bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (toRow < 0 || toRow >= BoardSize || toCol < 0 || toCol >= BoardSize)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(board[toRow, toCol]))
            {
                return false;
            }

            if (isWhiteTurn && board[fromRow, fromCol] == "B")
            {
                return false;
            }

            if (!isWhiteTurn && board[fromRow, fromCol] == "W")
            {
                return false;
            }

            if (fromCol != toCol)
            {
                return false;
            }

            if (Math.Abs(fromRow - toRow) == 1)
            {
                return true;
            }

            return false;
        }

        private void ChessGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cellClicked = e.OriginalSource as Rectangle;
            if (cellClicked != null)
            {
                int row = Grid.GetRow(cellClicked);
                int col = Grid.GetColumn(cellClicked);
                if (board[row, col] == "W" || board[row, col] == "B")
                {
                    MovePawn(row, col, isWhiteTurn ? row - 1 : row + 1, col);
                }
            }
        }
    }
}