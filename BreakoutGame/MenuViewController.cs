using Foundation;
using System;
using UIKit;

namespace BreakoutGame
{
	public partial class MenuViewController : UIViewController
	{
		public MenuViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			//StartButton.BackgroundImageForState(UIControlState.Normal);
			//StartButton.ImageForState(UIControlState.Normal);
			StartButton.SetImage(StartButton.ImageView.Image, UIControlState.Normal);
		}

	}
}