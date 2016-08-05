using System;

using SpriteKit;
using UIKit;

namespace BreakoutGame
{
	public partial class GameViewController : UIViewController
	{
		public int difficulty = (int)diff.normal;
		float score = 0;
		protected GameViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Configure the view.
			var skView = (SKView)View;
			skView.ShowsFPS = true;
			skView.ShowsNodeCount = true;
			/* Sprite Kit applies additional optimizations to improve rendering performance */
			skView.IgnoresSiblingOrder = true;

			// Create and configure the scene.
			var scene = SKNode.FromFile<GameScene>("GameScene");
			scene.ScaleMode = SKSceneScaleMode.AspectFill;
			scene.Size = skView.Bounds.Size;
			scene.gameViewController = (GameViewController)Self;


			// Present the scene.
			skView.PresentScene(scene);
		}

		public override bool ShouldAutorotate()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
			return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? UIInterfaceOrientationMask.AllButUpsideDown : UIInterfaceOrientationMask.All;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override bool PrefersStatusBarHidden()
		{
			return true;
		}

		public void GameOver(float s)
		{
			score = s;
			PerformSegue("GameOver", null);
			//DismissViewController(false, null);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			//base.PrepareForSegue(segue, sender);
			if (segue.Identifier == "GameOver")
			{
				GameOverViewController viewController = (GameOverViewController)segue.DestinationViewController;
				viewController.score = score;
			}
		}
	}
}

