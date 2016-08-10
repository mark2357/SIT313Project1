using Foundation;
using System;
using UIKit;

namespace BreakoutGame
{
    public partial class GameOverViewController : UIViewController
    {
		public float score = 0;

        public GameOverViewController (IntPtr handle) : base (handle)
		{
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ScoreLabel.Text += Math.Round(score); //sets the score text to the score value, note the score value is passed through during segue 
		}
	}
}