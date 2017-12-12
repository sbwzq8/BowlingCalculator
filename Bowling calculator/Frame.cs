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
    class Frame
    {
        private int? ball1 = null;
        private int? ball2 = null;
        private int rawFrameScore;
        private int finalFrameScore;
        private bool strike;
        private bool spare;

        private Label label = new Label();

        public Frame()
        {
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
        public int FinalFrameScore
        {
            get { return finalFrameScore; }
            set { finalFrameScore = value; }
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

        public virtual void resetFrame()
        {
            this.Ball1 = 0;
            this.Ball2 = 0;
            this.RawFrameScore = 0;
            this.FinalFrameScore = 0;
            this.Strike = false;
            this.Spare = false;
            this.Label.Content = "";
        }
    }

    class TenthFrame : Frame
    {
        private int? ball3 = 0;
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

        public override void resetFrame()
        {
            this.Ball1 = 0;
            this.Ball2 = 0;
            this.Ball3 = 0;
            this.RawFrameScore = 0;
            this.FinalFrameScore = 0;
            this.Strike = false;
            this.Spare = false;
            this.Label.Content = "";
        }
    }
}
