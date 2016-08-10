using Foundation;
using System;
using UIKit;

namespace BreakoutGame
{
    public partial class HelpViewController : UIViewController
    {
        public HelpViewController (IntPtr handle) : base (handle)
        {
        }

		partial void BackClick(UIButton sender) //called when back button is pressed
		{
			DismissViewController(false, null); //returns to previous scene
			//throw new NotImplementedException();
		}
	}
}