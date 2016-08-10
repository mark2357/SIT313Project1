using Foundation;
using System;
using UIKit;

namespace BreakoutGame
{
	public enum diff
	{
		easy = 0,
		normal = 1,
		hard = 2
	}
    public partial class DifficultyViewController : UIViewController
    {
		int currentDiff = (int)diff.normal; //the current difficulty
        public DifficultyViewController (IntPtr handle) : base (handle)
        {
			
		}

		public override void ViewDidLoad()
		{
			
		}

		partial void EasyPressed(UIButton sender) //called when the Easy button is pressed
		{
			currentDiff = (int)diff.easy; //sets the difficulty
		}

		partial void NormalPressed(UIButton sender) //called when the Normal button is pressed
		{
			currentDiff = (int)diff.normal;
		}

		partial void HardPressed(UIButton sender) //called when the Hard button is pressed
		{
			currentDiff = (int)diff.hard;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			if (segue.Identifier != "Back") //this is true when the View Controller is segueing to the game view
			{
				//sets the view to portrat as the game doesn't support landscape
				NSNumber value = (int)UIInterfaceOrientation.Portrait;
				NSString s = (NSString)"orientation";
				UIDevice.CurrentDevice.SetValueForKey(value, s);

				//gets the destination view controller
				GameViewController game =  (GameViewController)segue.DestinationViewController;
				game.difficulty = currentDiff; //passes the selected difficulty to the gameViewContorller
			}
		}

		partial void Back(UIButton sender) //called when the back button is pressed
		{
			//goes back to the main menu
			DismissViewController(true, null);
		}
	}
}