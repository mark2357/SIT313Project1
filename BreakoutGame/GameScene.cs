using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;

namespace BreakoutGame
{
	public enum Collision_Bits : uint
	{
		Ball = 0,
		Paddle = 1,
		Block = 2
	}


	public class GameScene : SKScene, ISKPhysicsContactDelegate
	{
		// SKSceneDelegate
		public Paddle paddle;
		Ball ball;
		float score;
		SKLabelNode scoreLabel;
		float lives;
		SKLabelNode livesLabel;
		int level;
		int difficulty = (int)diff.normal;

		public GameViewController gameViewController;

		protected GameScene(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void DidMoveToView(SKView view)
		{

			difficulty = gameViewController.difficulty;

			// Setup your scene here
			scoreLabel = new SKLabelNode()
			{
				Text = "Score: 0",
				FontSize = 20,
				Position = new CGPoint(5, Frame.Height - 5),
				FontName = "Arial-BoldMT",
				HorizontalAlignmentMode = SKLabelHorizontalAlignmentMode.Left,
				VerticalAlignmentMode = SKLabelVerticalAlignmentMode.Top                                                        
			};
			AddChild(scoreLabel);

			lives = 4 - difficulty;

			livesLabel = new SKLabelNode()
			{
				Text = "Lives: " + lives,
				FontSize = 20,
				Position = new CGPoint(Frame.Width - 5, Frame.Height - 5),
				FontName = "Arial-BoldMT",
				HorizontalAlignmentMode = SKLabelHorizontalAlignmentMode.Right,
				VerticalAlignmentMode = SKLabelVerticalAlignmentMode.Top
			};
			AddChild(livesLabel);


			PhysicsWorld.ContactDelegate = this;
			paddle = new Paddle(Frame, difficulty);
			AddChild(paddle);
			ball = new Ball(Frame, 10, difficulty);
			AddChild(ball);

			GenerateTiles(0);

			UISwipeGestureRecognizer swipe = new UISwipeGestureRecognizer(OnSwipeDetected);
			swipe.Direction = UISwipeGestureRecognizerDirection.Up;
			view.AddGestureRecognizer(swipe);

			level = 0;
			score = 0;

		}
		public void GenerateTiles(int level)
		{
			int rowCount = level + 4;
			if (rowCount > 10)
				rowCount = 10;
			for (int i = 0; i < 11; i++)
			{
				for (int j = 0; j < rowCount; j++)
				{
					CGPoint pos = new CGPoint((i + 1) * Frame.Size.Width / 12, Frame.Size.Height - ((j + 2) * Frame.Size.Width / 12));
					AddChild(new Block(pos, (float)Frame.Size.Width / 14, (float)Frame.Size.Width / 14));
				}
			}
		}
		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			// Called when a touch begins
			foreach (var touch in touches)
			{
				/*
				var location = ((UITouch)touch).LocationInNode(this);

				var sprite = new SKSpriteNode("Spaceship")
				{
					Position = location,
					XScale = 0.5f,
					YScale = 0.5f
				};

				var action = SKAction.RotateByAngle(NMath.PI, 1.0);

				sprite.RunAction(SKAction.RepeatActionForever(action));

				AddChild(sprite);
				*/
				paddle.Position = new CGPoint(((UITouch)touch).LocationInNode(this).X, paddle.Position.Y);
				if (ball.onPaddle == true)
					ball.Position = new CGPoint(((UITouch)touch).LocationInNode(this).X, Frame.Height / 8 + 50);

			}
		}
		void OnSwipeDetected()
		{
			ball.onPaddle = false;
			ball.PhysicsBody.Velocity = ball.currentVelocity;
		}
		public override void Update(double currentTime)
		{
			// Called before each frame is rendered
			if (ball.reset == true)
			{
				ball.Reset();
				ball.reset = false;
			}
			else if (ball.col == true)
			{
				ball.col = false;
				ball.PhysicsBody.Velocity = ball.currentVelocity;
			}
			ball.ScreenColTest();
		}

		public void AddScore(int s)
		{
			score += s;
			scoreLabel.Text = "Score: " + score.ToString();
		}

		public void DecreaseLives()
		{
			lives--;
			//if (lives > 1)
				livesLabel.Text = "Lives: " + lives.ToString();
			//else
			//livesLabel.Text = "Lives: " + lives.ToString();
			if (lives <= 0)
				gameViewController.GameOver(score);
		}

		//void HandleDidBeginContact(object sender, EventArgs args)
		[Export ("didBeginContact:")]
		public void DidBeginContact(SKPhysicsContact col)
		{
			
			//SKPhysicsContact col = sender as SKPhysicsContact;
			SKPhysicsBody bodyA;
			SKPhysicsBody bodyB;

			if (col.BodyA.CategoryBitMask < col.BodyB.CategoryBitMask)
			{
				bodyA = col.BodyA;
				bodyB = col.BodyB;
			}
			else
			{
				//return;
				bodyA = col.BodyB;
				bodyB = col.BodyA;
			}
			if (bodyB.CategoryBitMask == (uint)Collision_Bits.Paddle)
			{
				ball.col = true;
				ball.collisionWithPaddle(paddle.Position);
			}
			else if (bodyB.CategoryBitMask == (uint)Collision_Bits.Block)
			{
				ball.col = true;

				ball.collisionWithBlock(col.ContactPoint);
				bodyB.Node.RemoveFromParent();
				Block.BlockDestroyed();
				LevelCheck();
				AddScore(100 + (50 * difficulty));
			}
		}
		void LevelCheck()
		{
			if (Block.blockCount == 0)
			{
				level++;
				AddScore(1000 + (difficulty * 500));
				ball.reset = true; 
				GenerateTiles(level);
			}
		}
	}
}

