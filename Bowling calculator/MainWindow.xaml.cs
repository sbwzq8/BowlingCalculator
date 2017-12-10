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

namespace Bowling_calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Label> frameLabels = new List<Label>();
        List<Frame> frames = new List<Frame>();
        List<Border> borders = new List<Border>();
        List<String> balls = new List<string>();

        //int maxBalls = 21;
        //int currentBall = 0;
        int currentFrame = 0;
        bool frameGetsSpare = false;


        public MainWindow()
        {
            InitializeComponent();
            populateGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Button btn = (Button)sender;
            //string g = btn.Content.ToString();
            string s = (sender as Button).Content.ToString();
            if( (frameGetsSpare == false) && (s == "/") )
            {
                return;
            }
            if( currentFrame < 9 && s == "-" && frameGetsSpare == false)
            {
                frames[currentFrame].Ball1 = 0;
                frames[currentFrame].Label.Content = "-";
                frameGetsSpare = true;
                return;
            }
            if( currentFrame < 9 && s == "-" && frameGetsSpare == true)
            {
                frames[currentFrame].Ball2 = 0;
                frames[currentFrame].Label.Content += " -";
                currentFrame += 1;
                frameGetsSpare = false;
                return;
            }
            if (currentFrame < 9 && frameGetsSpare == false && s == "X")
            {
                frames[currentFrame].Ball1 = 10;
                frames[currentFrame].Label.Content = "X";
                currentFrame += 1;
                return;
            }
            if(currentFrame < 9 && frameGetsSpare == true && s == "/")
            {
                frames[currentFrame].Ball2 = 10;
                frames[currentFrame].Label.Content += " /";
                currentFrame += 1;
                frameGetsSpare = false;
                return;
            }
            if (currentFrame < 9 && frameGetsSpare == false && s != "X")
            {
                frames[currentFrame].Ball1 = int.Parse(s);
                frames[currentFrame].Label.Content = s;
                frameGetsSpare = true;
                return;
            }
            if(currentFrame < 9 && frameGetsSpare == true && s != "/")
            {
                int.TryParse(frames[currentFrame].Label.Content.ToString(), out int ball);
                if (s == "X" || ( ball + int.Parse(s) > 9 ) )
                {
                    return;
                } 
                frames[currentFrame].Ball2 = int.Parse(s);
                frames[currentFrame].Label.Content += " " + s;
                currentFrame += 1;
                frameGetsSpare = false;
                return;
            }

        }

        private void populateGrid()
        {
            for(int i=0; i < 9; i++)
            {
                Frame frame = new Frame();
                frames.Add(frame);
            }
            Frame frame10 = new TenthFrame();
            frames.Add(frame10);

            for(int i = 0, hSpacing = 12; i<10; i++)
            {
                //Label label = new Label();

                //label.HorizontalContentAlignment = HorizontalAlignment.Center;
                //label.VerticalContentAlignment = VerticalAlignment.Center;
                //label.Content = 9;

                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                //border.Padding = new Thickness(0);
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(hSpacing, 40, 0, 0);
                border.Width = 50;
                border.Height = 50;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.VerticalAlignment = VerticalAlignment.Top;

                border.Child = frames[i].Label;

                mainGrid.Children.Add(border);
                hSpacing += 55;
            }
        }

    }
    class Frame
    {
        private int ball1;
        private int ball2;
        private int rawFrameScore;

        //private bool wasStrike = false;

        private Label label = new Label();

        public Frame()
        {
            //label.Content = "X";
            this.label.HorizontalAlignment = HorizontalAlignment.Right;
            this.label.VerticalAlignment = VerticalAlignment.Top;
        }

        public int Ball1
        {
            get { return ball1; }
            set { ball1 = value; }
        }
        public int Ball2
        {
            get { return ball2; }
            set { ball2 = value; }
        }
        //public bool WasStrike
        //{
        //    get { return wasStrike; }
        //    set { wasStrike = value; }
        //}
        public Label Label
        {
            get { return label; }
            set { label = value; }
        }
    }

    class TenthFrame : Frame
    {
        private int ball3;

        public TenthFrame() : base()
        {
            
        }

        public int Ball3
        {
            get { return ball3; }
            set { ball3 = value; }
        }
    }
}