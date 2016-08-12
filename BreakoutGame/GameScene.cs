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
		float score; //the player score
		SKLabelNode scoreLabel;
		float lives; //the number of lives
		SKLabelNode livesLabel;
		int level; //the level the player is on, starts on 0

		int difficulty = (int)diff.normal;//gets set by the ViewContoller
		bool gameOver = false; //set to true when the game is over

		public GameViewController gameViewController; //the viewcontroller

		protected GameScene(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void DidMoveToView(SKView view)
		{
			difficulty = gameViewController.difficulty;// sets the difficulty
			gameOver = false;

			//creates and adds the labels
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

			lives = 4 - difficulty; //sets lives as they are based on difficulty

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



			PhysicsWorld.ContactDelegate = this; //sets the scene to deal wih collisions
			//creates the paddle and ball
			paddle = new Paddle(Frame, difficulty);
			AddChild(paddle);
			ball = new Ball(Frame, 10, difficulty);
			AddChild(ball);

			//creates the tiles for level 0(that is what the 0 is for)
			GenerateTiles(0);

			//creates the Swipe recogniser to recognise up swipes (used for ball release)
			UISwipeGestureRecognizer swipe = new UISwipeGestureRecognizer(OnSwipeDetected);
			swipe.Direction = UISwipeGestureRecognizerDirection.Up;
			view.AddGestureRecognizer(swipe);

			//sets level and score values
			level = 0;
			score = 0;

		}
		public void GenerateTiles(int level) //creates the tiles
		{
			int rowCount = level + 4; //adds an extra row each level
			if (rowCount > 10) //max 10 rows
				rowCount = 10;
			
			for (int i = 0; i < 11; i++) //11 tiles for each row
			{
				for (int j = 0; j < rowCount; j++)
				{
					//positions and sizes are relitive to the soze of the screen
					CGPoint pos = new CGPoint((i + 1) * Frame.Size.Width / 12, Frame.Size.Height - ((j + 2) * Frame.Size.Width / 12)); //creates the position
					AddChild(new Block(pos, (float)Frame.Size.Width / 14, (float)Frame.Size.Width / 14));//AddScore the bloc at the position
				}
			}
		}
		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			// Called when a touch begins
			foreach (var touch in touches)
			{
				
				paddle.Position = new CGPoint(((UITouch)touch).LocationInNode(this).X, paddle.Position.Y); //moves the paddle to where the screen is pressed
				if (ball.onPaddle == true) //if the ball is on the paddle the ball is moved as well
					ball.Position = new CGPoint(((UITouch)touch).LocationInNode(this).X, Frame.Height / 8 + (50 * (float)Frame.Width / 375));
			}
		}
		void OnSwipeDetected() //called when the user swipes up on the screen
		{
			//releases the ball
			ball.onPaddle = false;
			ball.PhysicsBody.Velocity = ball.currentVelocity;
		}
		public override void Update(double currentTime)
		{
			if (gameOver == true) //if the game is over
			{
				//removes the objects from the scene
				ball.RemoveAllActions();
				this.RemoveAllChildren();

				//calls the game view controller to move to the game over view
				gameViewController.GameOver(score);

				//sets several values to null to prevent errors
				paddle = null;
				ball = null;
				gameOver = false;
			}

			//several actions are done here instead of during collisions
			//as there was a issue with getting some code to work during collision calculations
			if (ball != null) 
			{
				if (ball.reset == true)
				{
					ball.Reset(); //puts the ball back on the paddle
					ball.reset = false;
				}
				else if (ball.col == true)
				{
					ball.col = false;
					ball.PhysicsBody.Velocity = ball.currentVelocity; //sets the balls position
				}
				ball.ScreenColTest(); //does a colision check with screen edges
			}
		}

		public void AddScore(int s) //adds score and updates the score label
		{
			score += s;
			scoreLabel.Text = "Score: " + score.ToString();
		}

		public bool DecreaseLives() //returns true when game over
		{
			//decreases the number of lives and updates the score label
			lives--;
				livesLabel.Text = "Lives: " + lives.ToString();
			//sets the score label
			if (lives <= 0)
			{
 				gameOver = true;
				return true;
			}
			return false;
		}

		//called when collision happens
		[Export ("didBeginContact:")]
		public void DidBeginContact(SKPhysicsContact col)
		{
			
			//SKPhysicsContact col = sender as SKPhysicsContact;
			SKPhysicsBody bodyA; //bodyA is allways the ball
			SKPhysicsBody bodyB;

			if (col.BodyA.CategoryBitMask < col.BodyB.CategoryBitMask) //makes the bodyA the ball
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
			if (bodyB.CategoryBitMask == (uint)Collision_Bits.Paddle) //collision with paddle
			{
				ball.col = true;
				ball.collisionWithPaddle(paddle.Position);
			}
			else if (bodyB.CategoryBitMask == (uint)Collision_Bits.Block) //colision with block
			{
				
				ball.collisionWithBlock(col.ContactPoint);
				ball.col = true;

				//destroys the block
				bodyB.Node.RemoveFromParent();
				Block.BlockDestroyed();

				LevelCheck(); //checks to see if the level is over
				AddScore(100 + (50 * difficulty)); //adds score, higher difficulty more score
			}
		}
		void LevelCheck() //checks if the level is over
		{
			if (Block.blockCount == 0) //if there is no more blocks on screen
			{
				level++; //increment level amount

				AddScore(1000 + (difficulty * 500));//adds bonus score
				ball.reset = true;
				GenerateTiles(level); //creates tiles for new level
			}
		}
	}
}

