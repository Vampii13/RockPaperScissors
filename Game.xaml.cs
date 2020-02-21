using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RockPaperScissors
{
    /// <summary>
    /// Interaktionslogik für Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        int status = 0;                                     // Who's turn is it?
        public History h = null;                            // For the List
        List<History> history = new List<History>();        // For the DataGrid
        List<BitmapImage> sings = new List<BitmapImage>();  // A List with all signs

        // Points
        int pOne = 0;
        int pTwo = 0;

        // how many players?
        int playerN = 2;
        public Game(int player)
        {
            InitializeComponent();
            FillDropDowns();
            ListOfImages();
            HideResult();
            playerN = player;
            //UpdateHistory();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // First player
            if (status == 0)
            {
                if (ChooseOne.SelectedItem != null)  // First player chooses a sign
                {
                    h = new History();
                    h.Player1 = ChooseOne.SelectedItem as Zeichen;
                    status++;
                    if (playerN == 1)       // Computer
                    {
                        // to choose a random sign
                        Random r = new Random();
                        int p2 = r.Next(0, sings.Count());
                        Console.WriteLine(p2);

                        ChooseTwo.SelectedIndex = p2;
                        h.Player2 = ChooseTwo.SelectedItem as Zeichen;

                        status = 2;
                        CheckWin();

                        history.Add(h);
                        UpdateHistory();
                    }
                    HideResult();
                } else {
                    status = 0;
                }
            } else if (status == 1 )     // Second player
            {
                if (ChooseTwo.SelectedItem != null)
                {
                    h.Player2 = ChooseTwo.SelectedItem as Zeichen;
                    status++;
                    HideResult();
                    CheckWin();

                    history.Add(h);
                    UpdateHistory();
                } else {
                    status = 0;
                }
            } else if (status == 2)     // Show results
            {
                status = 0;
                HideResult();
            }
        }

        // Fills ChooseOne and ChooseTwo
        public void FillDropDowns()
        {
            using(var con = new RockPaperScissorEntities())
            {
                ChooseOne.ItemsSource = con.Zeichen.ToList();
                ChooseTwo.ItemsSource = con.Zeichen.ToList();
            }
        }

        // get all the Images from the db
        public void ListOfImages()
        {
            using (var con = new RockPaperScissorEntities())
            {
                List<Zeichen> z = con.Zeichen.ToList();
                for (int i = 0; i < z.Count(); i++)
                {
                    var pic = z.ElementAt(i).Bild.ToArray();
                    MemoryStream ms = new MemoryStream(pic);

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();

                    sings.Add(bitmapImage);
                }
            }
        }

        // Updates picture of sign for player one
        private void ChooseOne_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(ChooseOne.SelectedItem != null)
            {
                SignOne.Source = sings.ElementAt(ChooseOne.SelectedIndex);
            }
        }

        // Updates picture of sign for player two
        private void ChooseTwo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ChooseTwo.SelectedItem != null)
            {
                SignTwo.Source = sings.ElementAt(ChooseTwo.SelectedIndex);
            }
        }

        // which player is showing
        public void HideResult()
        {
            if(status == 0)             // First player
            {
                Hide1.Visibility = Visibility.Hidden;
                Hide2.Visibility = Visibility.Visible;
                ChooseOne.Visibility = Visibility.Visible;
                ChooseTwo.Visibility = Visibility.Visible;
            } else if (status == 1)     // Second player
            {
                Hide2.Visibility = Visibility.Hidden;
                Hide1.Visibility = Visibility.Visible;
            }
            else                        // Results
            {
                Hide1.Visibility = Visibility.Hidden;
                Hide2.Visibility = Visibility.Hidden;
                ChooseOne.Visibility = Visibility.Hidden;
                ChooseTwo.Visibility = Visibility.Hidden;
            }
        }

        // Fills the DataGrid 
        public void UpdateHistory()
        {
            HistoryList.ItemsSource = null;
            HistoryList.ItemsSource = history;
        }

        // Checks who wins
        public void CheckWin()
        {
            using(var con = new RockPaperScissorEntities())
            {
                try
                {
                    // Searches the right rule
                    var rule = con.Situation.Where(x => (x.Sieger == h.Player1.Id || x.Sieger == h.Player2.Id) && (x.Verlierer == h.Player1.Id || x.Verlierer == h.Player2.Id)).ToList();
                    Situation s = rule.ElementAt(0);

                    // gets the winner
                    var winner = con.Zeichen.Where(x => x.Id == s.Sieger).ToList();

                    // looks who gets the point
                    if(h.Player1.Id == s.Sieger)
                    {
                        pOne++;
                        PointsOne.Content = pOne;
                    } else
                    {
                        pTwo++;
                        PointsTwo.Content = pTwo;
                    }
                } catch(Exception e)
                {
                    Console.WriteLine("Untentschieden");
                }
            }
        }
    }
}
