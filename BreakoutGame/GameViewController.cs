using System;

using SpriteKit;
using UIKit;
using Foundation;

namespace BreakoutGame
{
	public partial class GameViewController : UIViewController
	{
		//stores some infomation used as input and output of the scene
		public int difficulty = (int)diff.normal;
		float score = 0;

		GameScene scene; //the game scene
		protected GameViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Configure the view.
			var skView = (SKView)View;
			//disables several debuging stats
			skView.ShowsFPS = false;
			skView.ShowsNodeCount = false;
			/* Sprite Kit applies additional optimizations to improve rendering performance */
			skView.IgnoresSiblingOrder = true;

			// Create and configure the scene.
			scene = SKNode.FromFile<GameScene>("GameScene");
			scene.ScaleMode = SKSceneScaleMode.AspectFit;
			scene.Size = skView.Bounds.Size;
			scene.gameViewController = (GameViewController)Self;


			// Present the scene.
			skView.PresentScene(scene);
		}

		public override bool ShouldAutorotate()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() //makes the app only work in portrait
		{
			//return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? UIInterfaceOrientationMask.AllButUpsideDown : 
				           return UIInterfaceOrientationMask.Portrait;
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

		public void GameOver(float s) //called from the GameScene
		{
			score = s;
			PerformSegue("GameOver", null);
			//DismissViewController(false, null);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			//base.PrepareForSegue(segue, sender);
			if (segue.Identifier == "GameOver") //if it's moving to the game over scene
			{
				GameOverViewController viewController = (GameOverViewController)segue.DestinationViewController;
				viewController.score = score; //passes score to game over scene
			}
		}
	}
}

