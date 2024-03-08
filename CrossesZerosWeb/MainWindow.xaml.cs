using CrossesZeros;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Microsoft.UI.Windowing; // Needed for AppWindow.
using WinRT.Interop;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CrossesZerosXAML_WinUI3
{
    public struct CellTag
    {
        public int x;
        public int y;
        public CellTag(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public sealed partial class MainWindow : Window
    {
        private Game game = null;
        private double gameGridSize = 0;
        private AppWindow m_AppWindow;
        public MainWindow()
        {
            this.InitializeComponent();
            m_AppWindow = GetAppWindowForCurrentWindow();
            var titleBar = m_AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;

            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            BeginNewGame();
        }
        
        private void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            gameGridSize = Math.Min(MainGrid.ActualWidth, MainGrid.ActualHeight) * 0.75;
            GameGrid.Width = gameGridSize;
            GameGrid.Height = gameGridSize;
            GameBorder.Height = gameGridSize;
            GameBorder.Width = gameGridSize;
            //RedrawField();
        }
        

        private void Rect_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var rect = sender as Rectangle;
            var tag = (CellTag)rect.Tag;
            ContentDialog dialog = new ContentDialog();
            if (game.Player2 is ConsolePlayer)
                game.MakeGameStep(tag.x, tag.y);
            else
            {
                game.MakeGameStep(tag.x, tag.y);
                if (!game.GameCompleted)
                    game.MakeGameStep(-1, -1);
            }

            UpdateGameField(game.Field);
        }

        private async void Game_GameCompletedEvent(object sender)
        {
            string winner = "";
            if (game.Winner != Role.None)
                winner = $"The winner is - {game.Winner}!";
            else
                winner = "Draw!";
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = GamePage.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Game over! " + winner;
            dialog.PrimaryButtonText = "Restart";
            dialog.CloseButtonText = "Ok...";
            dialog.DefaultButton = ContentDialogButton.Primary;
            //dialog.Content = new ContentDialogContent();

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
                BeginNewGame();
        }

        private void UpdateGameField(GameField field)
        {
            for (int i = 0; i < game.Field.FieldSize; i++)
                for (int j = 0; j < game.Field.FieldSize; j++)
                {
                    FontIcon icon = new FontIcon()
                    {
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        FontSize = gameGridSize / field.FieldSize / 2,
                        Foreground = new SolidColorBrush(Colors.Black)
                    };

                    switch (field.Cells[i, j].CellRole)
                    {
                        case Role.Crosses:
                            icon.Glyph = "\uE947";
                            break;
                        case Role.Zeros:
                            icon.Glyph = "\uEA3A";
                            break;
                        default:
                            break;

                    }
                    Grid.SetColumn(icon, i);
                    Grid.SetRow(icon, j);
                    icon.HorizontalAlignment = HorizontalAlignment.Center;
                    icon.VerticalAlignment = VerticalAlignment.Center;
                    GameGrid.Children.Add(icon);
                }
        }

        private void BeginNewGame()
        {
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
            int FieldSize = (int)GameFieldSizeBox.Value;

            for (int i = 0; i < FieldSize; i++)
            {
                RowDefinition row = new RowDefinition();
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1.0, GridUnitType.Star);
                row.Height = new GridLength(1.0, GridUnitType.Star);
                GameGrid.RowDefinitions.Add(row);
                GameGrid.ColumnDefinitions.Add(column);
            }

            RedrawField();


            ConsolePlayer player1 = new ConsolePlayer(Role.Crosses);
            Player player2 = null;

            switch (opponentComboBox.SelectedIndex)
            {
                case 0:
                    player2 = new ConsolePlayer(Role.Zeros);
                    break;
                case 1:
                    player2 = new StupidBotPlayer(Role.Zeros);
                    break;
                case 2:
                    player2 = new SmarterBotPlayer(Role.Zeros);
                    break;
                case 3:
                    player2 = new SmartBotPlayer(Role.Zeros);
                    break;
                default:
                    break;
            }
            game = new Game(FieldSize, player1, player2);
            game.GameCompletedEvent += Game_GameCompletedEvent;
        }

        private void RedrawField()
        {
            GameGrid.Children.Clear();

            int FieldSize = (int)GameFieldSizeBox.Value;

            for (int i = 0; i < FieldSize; i++)
                for (int j = 0; j < FieldSize; j++)
                {
                    var size = gameGridSize / FieldSize;
                    Rectangle rect = new Rectangle();
                    rect.Fill = (AcrylicBrush)GamePage.Resources["CustomAcrylicInAppLuminosity"];
                    rect.RadiusX = 10; rect.RadiusY = 10;
                    rect.PointerPressed += Rect_PointerPressed;
                    rect.Height = size * 0.9;
                    rect.Width = size * 0.9;
                    rect.HorizontalAlignment = HorizontalAlignment.Center;
                    rect.VerticalAlignment = VerticalAlignment.Center;
                    rect.Tag = new CellTag(i, j);
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, j);
                    GameGrid.Children.Add(rect);
                }
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            gameGridSize = Math.Min(MainGrid.ActualWidth, MainGrid.ActualHeight) * 0.75;
            GameGrid.Width = gameGridSize;
            GameGrid.Height = gameGridSize;
            GameBorder.Height = gameGridSize;
            GameBorder.Width = gameGridSize;
            if(game != null)
            {
                RedrawField();
                UpdateGameField(game.Field);
            }
                
        }
    }
}
