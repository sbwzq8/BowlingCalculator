using System;
using System.IO;
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

        //List<Label> frameLabels = new List<Label>();
        //List<Border> borders = new List<Border>();
        //List<int?> balls = new List<int?>();

        //initialize lists for storing frame scores
        List<Frame> frames = new List<Frame>();
        List<Label> scoreFrames = new List<Label>();

        int currentFrame = 0;
        int score = 0;

        bool frameGetsExtraBall = false;
        bool doneBowling = false;
        


        public MainWindow()
        {
            InitializeComponent();
            populateGrid();

            bowlingImageCanvas.Children.Clear();
            Image myImage = new Image();
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "bowling.jpg");

            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);
            //myBitmapImage.DecodePixelWidth = 128;
            myBitmapImage.EndInit();
            myImage.Source = myBitmapImage;
            myImage.Width = 215;
            bowlingImageCanvas.Children.Add(myImage);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(doneBowling == true)
            {
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
                                if (frame.Ball2 + pins > 9)
                                {
                                    return;
                                }
                                frame.Ball3 = pins;
                                frame.Label.Content += " " + s;
                                doneBowling = true;
                                break;
                        }
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (calculateFrameScores() == -1)
            {
                return;
            }
            score = 0;
            
            //find game final score
            foreach(Frame frame in frames)
            {
                score += frame.RawFrameScore;
            }
            //fill score frames with final values
            for(int i = 0; i <= currentFrame; i++)
            {
                scoreFrames[i].Content = frames[i].FinalFrameScore;
            }
            MessageBox.Show("Total score is " + score);
        }

        //reset button
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            currentFrame = 0;
            score = 0;

            frameGetsExtraBall = false;
            doneBowling = false;

            foreach (Frame frame in frames)
            {
                frame.Ball1 = null;
                frame.Ball2 = null;
                frame.RawFrameScore = 0;
                frame.FinalFrameScore = 0;
                frame.Strike = false;
                frame.Spare = false;
                frame.Label.Content = "";
            }
            TenthFrame tenth = (TenthFrame)frames[9];
            tenth.Ball3 = 0;
            foreach(Label label in scoreFrames)
            {
                label.Content = "";
            }
        }

        private int calculateFrameScores()
        {
            try
            {
                for (int current = 0; current < frames.Count(); current++)
                {
                    //handle all frames before 9th frame
                    if (current < 8)
                    {
                        //handle frame was strike, next 2 balls added to frame score
                        if (frames[current].Strike)
                        {
                            //next frame was spare, set current frame score to 20
                            if (frames[current + 1].Spare)
                            {
                                frames[current].RawFrameScore = 20;
                            }
                            //next 2 frames are strikes, set current frame score to 30
                            else if (frames[current + 1].Strike && frames[current + 2].Strike)
                            {
                                frames[current].RawFrameScore = 30;
                            }
                            //next frame strike, frame afterwards was not. Add 10 for strike and number of pins knocked down by first ball 2 frames ahead
                            else if (frames[current + 1].Strike && !frames[current + 2].Strike)
                            {
                                frames[current].RawFrameScore = 10 + 10 + (int)frames[current + 2].Ball1;
                            }
                            //Next frame not strike or spare, add both balls to frame score
                            else
                            {
                                frames[current].RawFrameScore = 10 + (int)frames[current + 1].Ball1 + (int)frames[current + 1].Ball2;
                            }
                        }
                        //handle frame was spare, next ball added to frame score
                        else if (frames[current].Spare)
                        {
                            frames[current].RawFrameScore = 10 + (int)frames[current + 1].Ball1;
                        }
                        //handle frame was neither strike nor spare, frame score is just both balls added together.
                        else
                        {
                            frames[current].RawFrameScore = (int)frames[current].Ball1 + (int)frames[current].Ball2;
                        }
                    }
                    //handle 9th frame
                    if (current == 8)
                    {
                        //9th frame strike. Add first 2 10th frame balls to score
                        if (frames[current].Strike)
                        {
                            frames[current].RawFrameScore = 10 + (int)frames[current + 1].Ball1 + (int)frames[current + 1].Ball2;
                        }
                        //9th frame spare, add first ball of 10th frame to score
                        else if (frames[current].Spare)
                        {
                            frames[current].RawFrameScore = 10 + (int)frames[current + 1].Ball1;
                        }
                        //9th frame neither strike nor spare, add both balls to final score
                        else
                        {
                            frames[current].RawFrameScore = (int)frames[current].Ball1 + (int)frames[current].Ball2;
                        }
                    }
                    //grab tenth frame casted as TenthFrame object
                    TenthFrame tenth = (TenthFrame)frames[9];
                    //10th frame, just add all 3 balls to score
                    if (current == 9)
                    {
                        frames[current].RawFrameScore = (int)frames[current].Ball1 + (int)frames[current].Ball2 + (int)tenth.Ball3;
                    }
                }
                //calculate final scores for each frame
                for(int i = 0; i <= currentFrame; i++)
                {
                    if(i == 0)
                    {
                        frames[i].FinalFrameScore = frames[i].RawFrameScore;
                    }
                    else
                    {
                        frames[i].FinalFrameScore = frames[i - 1].FinalFrameScore + frames[i].RawFrameScore;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error, must finish before scoring");
                return -1;
            }
            return 0;
        }

        private void populateGrid()
        {
            //create frame objects and add them to frames list
            for(int i=0; i < 9; i++)
            {
                Frame frame = new Frame();
                frames.Add(frame);
            }
            Frame tenth = new TenthFrame();
            frames.Add(tenth);

            for(int i = 0; i < 10; i++)
            {
                Label label = new Label();
                scoreFrames.Add(label);
            }

            //programatically create UI elements for frames
            for(int i = 0, hSpacing = 12; i<10; i++)
            {
                //add frames to borders
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                //border.Padding = new Thickness(0);
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(hSpacing, 0, 0, 287);
                border.Width = 50;
                border.Height = 50;
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.VerticalAlignment = VerticalAlignment.Bottom;

                border.Child = frames[i].Label;

                //add final frame score elements to borders
                Border scoringBorder = new Border();
                scoringBorder.BorderBrush = Brushes.Black;
                scoringBorder.BorderThickness = new Thickness(1);
                scoringBorder.Margin = new Thickness(hSpacing, 0, 0, 232);
                scoringBorder.Width = 50;
                scoringBorder.Height = 50;
                scoringBorder.HorizontalAlignment = HorizontalAlignment.Left;
                scoringBorder.VerticalAlignment = VerticalAlignment.Bottom;

                scoringBorder.Child = scoreFrames[i];

                //add borders to grid
                mainGrid.Children.Add(border);
                mainGrid.Children.Add(scoringBorder);
                hSpacing += 55;
            }
        }
    }
}