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
		int currentDiff = (int)diff.normal;
        public DifficultyViewController (IntPtr handle) : base (handle)
        {
			
		}

		public override void ViewDidLoad()
		{
			
		}

		partial void EasyPressed(UIButton sender)
		{
			currentDiff = (int)diff.easy;
			//throw new NotImplementedException();
		}

		partial void NormalPressed(UIButton sender)
		{
			currentDiff = (int)diff.normal;

			//throw new NotImplementedException();
		}

		partial void HardPressed(UIButton sender)
		{
			currentDiff = (int)diff.hard;

			//throw new NotImplementedException();
		}
		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			if (segue.Identifier != "Back")
			{
				NSNumber value = (int)UIInterfaceOrientation.Portrait;
				NSString s = (NSString)"orientation";
				UIDevice.CurrentDevice.SetValueForKey(value, s);

				GameViewController game =  (GameViewController)segue.DestinationViewController;
				game.difficulty = currentDiff;
			}
		}

		partial void Back(UIButton sender)
		{
			DismissViewController(true, null);
		}
	}
}