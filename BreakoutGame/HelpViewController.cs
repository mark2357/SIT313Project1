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

		partial void BackClick(UIButton sender)
		{
			DismissViewController(false, null);
			//throw new NotImplementedException();
		}
	}
}