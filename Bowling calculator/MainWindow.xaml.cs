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
        //List<Border> borders = new List<Border>();
        List<int?> balls = new List<int?>();
        
        int currentFrame = 0;

        int currentScoringFrame = 0;
        int score = 0;

        bool frameGetsExtraBall = false;
        bool doneBowling = false;
        


        public MainWindow()
        {
            InitializeComponent();
            populateGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(doneBowling == true)
            {
                TenthFrame tenth = (TenthFrame)frames[9];
                foreach(Frame frame in frames)
                {
                    balls.Add(frame.Ball1);
                    balls.Add(frame.Ball2);
                }
                balls.Add(tenth.Ball3);
                return;
            }
            //Button btn = (Button)sender;
            //string g = btn.Content.ToString();
            string s = (sender as Button).Content.ToString();
            //Frame frame = frames[currentFrame];
            
            //Set ball values for first 9 frames
            if(currentFrame < 9)
            {
                Frame frame = frames[currentFrame];
                //Case for no ball thrown in frame yet. Don't allow spare button to be clicked
                if (frame.Ball1 == null)
                {
                    switch (s)
                    {
                        case "-":
                            frame.Ball1 = 0;
                            frame.Label.Content = s;
                            break;
                        case "X":
                            frame.Ball1 = 10;
                            frame.Label.Content = s;
                            frame.Strike = true;
                            currentFrame += 1;
                            break;
                        case "/":
                            return;
                        default:
                            int pins;
                            int.TryParse(s, out pins);
                            frame.Ball1 = pins;
                            frame.Label.Content = pins;
                            break;
                    }
                    return;
                }
                //Case for second ball in frame. First was not strike. Do not allow strike
                else
                {
                    int pins;
                    int.TryParse(s, out pins);
                    switch (s)
                    {
                        case "-":
                            frame.Ball2 = 0;
                            frame.Label.Content += " " + s;
                            break;
                        case "X":
                            return;
                        case "/":
                            frame.Ball2 = 10 - frame.Ball1;
                            frame.Label.Content += " " + s;
                            frame.Spare = true;
                            break;
                        default:
                            if(frame.Ball1 + pins > 9)
                            {
                                return;
                            }
                            frame.Ball2 = pins;
                            frame.Label.Content += " " + s;
                            break;
                    }
                }
                currentFrame += 1;
            }
            //Handle cases for tenth frame
            else
            {
                TenthFrame frame = (TenthFrame)frames[currentFrame];
                //first ball not thrown in frame. Dont allow spare.
                if(frame.Ball1 == null)
                {
                    switch (s)
                    {
                        case "-":
                            frame.Ball1 = 0;
                            frame.Label.Content = s;
                            break;
                        case "X":
                            frame.Ball1 = 10;
                            frame.Label.Content = s;
                            frameGetsExtraBall = true;
                            break;
                        case "/":
                            break;
                        default:
                            int pins;
                            int.TryParse(s, out pins);
                            frame.Ball1 = pins;
                            frame.Label.Content = pins;
                            break;
                    }
                    //return;
                }
                //First ball not strike. Do not allow strike.
                else if(frame.Ball2 == null && frame.Ball1 != 10)
                {
                    int pins;
                    int.TryParse(s, out pins);
                    switch (s)
                    {
                        case "-":
                            frame.Ball2 = 0;
                            frame.Label.Content += " " + s;
                            frameGetsExtraBall = false;
                            doneBowling = true;
                            break;
                        case "X":
                            break;
                        case "/":
                            pins = (int)frame.Ball1;
                            frame.Ball2 = 10 - pins;
                            frame.Label.Content += " " + s;
                            frameGetsExtraBall = true;
                            frame.SecondBallSpare = true;
                            break;
                        default:
                            if (frame.Ball1 + pins > 9)
                            {
                                return;
                            }
                            frame.Ball2 = pins;
                            frame.Label.Content += " " + s;
                            frameGetsExtraBall = false;
                            doneBowling = true;
                            break;
                    }
                    //return;
                }
                //first ball was strike, do not allow spare.
                else if(frame.Ball2 == null && frame.Ball1 == 10)
                {
                    int pins;
                    int.TryParse(s, out pins);
                    frameGetsExtraBall = true;
                    switch (s)
                    {
                        case "-":
                            frame.Ball2 = 0;
                            frame.Label.Content += " " + s;
                            break;
                        case "X":
                            frame.Ball2 = 10;
                            frame.Label.Content += " " + s;
                            frame.SecondBallStrike = true;
                            break; ;
                        case "/":
                            break; ;
                        default:
                            frame.Ball2 = pins;
                            frame.Label.Content += " " + s;
                            break;
                    }
                    //return;
                }
                //if first ball was strike, or second ball was spare or strike, handle 3rd ball.
                else if (frameGetsExtraBall)
                {
                    //if second ball thrown was strike or spare, allow a strike. do not allow spare.
                    if (frame.SecondBallStrike || frame.SecondBallSpare)
                    {
                        int pins;
                        int.TryParse(s, out pins);
                        switch (s)
                        {
                            case "-":
                                frame.Ball3 = 0;
                                frame.Label.Content += " " + s;
                                doneBowling = true;
                                break;
                            case "X":
                                frame.Ball3 = 10;
                                frame.Label.Content += " " + s;
                                doneBowling = true;
                                return;
                            case "/":
                                return;
                            default:
                                frame.Ball3 = pins;
                                frame.Label.Content += " " + s;
                                doneBowling = true;
                                break;
                        }
                    }
                    //handle for first ball strike and second ball not strike.
                    else
                    {
                        int pins;
                        int.TryParse(s, out pins);
                        switch (s)
                        {
                            case "-":
                                frame.Ball3 = 0;
                                frame.Label.Content += " " + s;
                                doneBowling = true;
                                break;
                            case "X":
                                return;
                            case "/":
                                if (frame.Ball2 + pins > 9)
                                {
                                    return;
                                }
                                pins = 10 - (int)frame.Ball2;
                                frame.Ball3 = pins;
                                frame.Label.Content += " " + s;
                                frameGetsExtraBall = false;
                                doneBowling = true;
                                break;
                            default:
                                frame.Ball3 = pins;
                                frame.Label.Content += " " + s;
                                doneBowling = true;
                                break;
                        }
                    }
                }
            }
            //if( (frameGetsSpare == false) && (s == "/") )
            //{
            //    return;
            //}
            //if( currentFrame < 9 && s == "-" && frameGetsSpare == false)
            //{
            //    frames[currentFrame].Ball1 = 0;
            //    frames[currentFrame].Label.Content = "-";
            //    frameGetsSpare = true;
            //    return;
            //}
            //if( currentFrame < 9 && s == "-" && frameGetsSpare == true)
            //{
            //    frames[currentFrame].Ball2 = 0;
            //    frames[currentFrame].Label.Content += " -";
            //    currentFrame += 1;
            //    frameGetsSpare = false;
            //    return;
            //}
            //if (currentFrame < 9 && frameGetsSpare == false && s == "X")
            //{
            //    frames[currentFrame].Ball1 = 10;
            //    frames[currentFrame].Label.Content = "X";
            //    currentFrame += 1;
            //    return;
            //}
            //if(currentFrame < 9 && frameGetsSpare == true && s == "/")
            //{
            //    frames[currentFrame].Ball2 = 10;
            //    frames[currentFrame].Label.Content += " /";
            //    currentFrame += 1;
            //    frameGetsSpare = false;
            //    return;
            //}
            //if (currentFrame < 9 && frameGetsSpare == false && s != "X")
            //{
            //    frames[currentFrame].Ball1 = int.Parse(s);
            //    frames[currentFrame].Label.Content = s;
            //    frameGetsSpare = true;
            //    return;
            //}
            //if(currentFrame < 9 && frameGetsSpare == true && s != "/")
            //{
            //    int.TryParse(frames[currentFrame].Label.Content.ToString(), out int ball);
            //    if (s == "X" || ( ball + int.Parse(s) > 9 ) )
            //    {
            //        return;
            //    } 
            //    frames[currentFrame].Ball2 = int.Parse(s);
            //    frames[currentFrame].Label.Content += " " + s;
            //    currentFrame += 1;
            //    frameGetsSpare = false;
            //    return;
            //}

            //if(currentFrame == 9 && frameGetsSpare == false)
            //{

            //}

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for(int current = 0; current<frames.Count(); current++)
            {
                TestBox.Text += frames[current].Ball1.ToString();
                TestBox.Text += frames[current].Ball2.ToString();
                if(current == 9)
                {
                    TenthFrame tenth = (TenthFrame)frames[current];
                    TestBox.Text += tenth.Ball3.ToString();
                }

            }
            //foreach(Frame frame in frames)
            //{
            //    if(frame.Strike == true)
            //    {
            //        if(frame)
            //    }
            //    //TestBox.Text += ball.ToString() + " ";
            //}
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
        private int? ball1 = null;
        private int? ball2 = null;
        private int rawFrameScore;
        private bool strike;
        private bool spare;

        //private bool wasStrike = false;

        private Label label = new Label();

        public Frame()
        {
            //label.Content = "X";
            this.label.HorizontalAlignment = HorizontalAlignment.Right;
            this.label.VerticalAlignment = VerticalAlignment.Top;
        }

        public int? Ball1
        {
            get { return ball1; }
            set { ball1 = value; }
        }
        public int? Ball2
        {
            get { return ball2; }
            set { ball2 = value; }
        }
        public int RawFrameScore
        {
            get { return rawFrameScore; }
            set { rawFrameScore = value; }
        }
        public bool Strike
        {
            get { return strike; }
            set { strike = value; }
        }
        public bool Spare
        {
            get { return spare; }
            set { spare = value; }
        }

        public Label Label
        {
            get { return label; }
            set { label = value; }
        }
    }

    class TenthFrame : Frame
    {
        private int? ball3;
        private bool secondBallStrike;
        private bool secondBallSpare;

        public TenthFrame() : base()
        {
            
        }

        public int? Ball3
        {
            get { return ball3; }
            set { ball3 = value; }
        }
        public bool SecondBallStrike
        {
            get { return secondBallStrike; }
            set { secondBallStrike = value; }
        }
        public bool SecondBallSpare
        {
            get { return secondBallSpare; }
            set { secondBallSpare = value; }
        }
    }
}