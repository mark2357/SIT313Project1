using System;
using SpriteKit;
using Foundation;
using UIKit;
using CoreGraphics;

namespace BreakoutGame
{
	public class Ball : SKShapeNode
	{
		public CGRect screenSize; //used to store the screensize
		public CGVector currentVelocity; //used to store the current velocity of the ball between collisions
		public bool col = false; //set to true when a collision occurs
		private float radius; //the radius of the ball
		public bool onPaddle = true; //when the ball is hovering above the paddle
		public bool reset = false; //when set to true the ball needs to be reset to it's inital pos

		public Ball(CGRect frame, float r, int diff) //constructor
		{
			var pathBall = new CGPath(); //creates the path, used for the balls image
			radius = r * (float)frame.Width / 375;//determines the size of the ball from the size of the screen
			pathBall.AddArc(0f, 0f, radius, 0, 2f * (float)Math.PI, true); //creates the balls path
			Path = pathBall;
			screenSize = frame; //sets the screen size
			FillColor = new UIColor(1f, 0f, 0f, 1f); //makes the ball red
			Position = new CGPoint(frame.Width / 2, frame.Height / 8 + (50 * (float)frame.Width / 375)); //sets the balls position
			PhysicsBody = SKPhysicsBody.CreateBodyFromPath(pathBall); //creates the balls hitbox (for collisions)
			PhysicsBody.AffectedByGravity = false;
			currentVelocity = new CGVector(0, (150 + 50 * diff) * (float)frame.Width / 375); //sets it's current velocity reletive to the screen size

			//sets several inital variables
			PhysicsBody.Friction = 0f;
			PhysicsBody.LinearDamping = 0f;
			PhysicsBody.Velocity = new CGVector(0, 0);

			//sets the balls bitmasks, basicly makes it hit the paddle and blocks
			PhysicsBody.CategoryBitMask = (uint)Collision_Bits.Ball;
			PhysicsBody.CollisionBitMask = (uint)Collision_Bits.Paddle | (uint)Collision_Bits.Block;
			PhysicsBody.ContactTestBitMask = (uint)Collision_Bits.Paddle | (uint)Collision_Bits.Block;
		}
		public void Reset() //resets the ball back onto the paddle
		{
			//resets the position to above the paddle
			Position = new CGPoint(((GameScene)Parent).paddle.Position.X, screenSize.Height / 8 + (50 * (float)screenSize.Width / 375));
			onPaddle = true;
			PhysicsBody.Velocity = new CGVector(0, 0); //resets the velocity to 0
			currentVelocity = new CGVector(0, GetLength(currentVelocity) * 1.1f); //increases the velocity by 10%
			col = false; //the ball hasn't hit anything
		}
		public void collisionWithPaddle(CGPoint p) //called by the game scene when the ball hits the paddle
		{
			//esentualy makes the ball move directly way from the centre of the paddle

			CGVector dir = new CGVector(Position.X - p.X, Position.Y - p.Y); //creates a vector between the ball and the paddle
			dir = Normalise(dir); //makes the length of the vector 1

			float speed = GetLength(currentVelocity); //gets the speed of the ball
			currentVelocity = new CGVector(dir.dx * speed, dir.dy * speed); //sets the velocity of the ball to the balls speed in the direction of "dir"
		}

		public void collisionWithBlock(CGPoint p) //called when it hits the block
		{
			if (col == false)
			{
				CGVector dir = new CGVector(Position.X - p.X, Position.Y - p.Y); //creates a vector between the ball and the collision point

				//tries to work out if the ball hits a vertical edge or horizontal edge, or corner of the block
				if (Math.Abs(dir.dx) * 3 < Math.Abs(dir.dy))
				{
					currentVelocity = new CGVector(currentVelocity.dx, -currentVelocity.dy); //reverses the vertical vel
				}
				else if (Math.Abs(dir.dy) * 3 < Math.Abs(dir.dx))
				{
					currentVelocity = new CGVector(-currentVelocity.dx, currentVelocity.dy); //reverses the horizontal vel
				}
				else
				{
					dir = Normalise(dir); //makes dir have a length of 1
					float speed = GetLength(currentVelocity); //gets the speed of the ball
															  //sets the current velocity so that ball is moving away from the collision point
					currentVelocity = new CGVector(dir.dx * speed, dir.dy * speed); 
				}
			}
		}
		private CGVector Normalise(CGVector vect) //makes the length of the inputed vector 1
		{
			float vectLength = (float)Math.Sqrt((vect.dx * vect.dx) + (vect.dy * vect.dy)); //gets length
			vect = new CGVector(vect.dx / vectLength, vect.dy / vectLength); //devides vector by it's length
			return vect;
		}
		private float GetLength(CGVector vect) //returns length of vector
		{
			float vectLength = (float)Math.Sqrt((vect.dx * vect.dx) + (vect.dy * vect.dy)); //gets length
			return vectLength;
		}
		public void ScreenColTest() //checks collisions with the edge of the screen
		{
			
			if ((Position.X - radius < 0 && currentVelocity.dx < 0) //left of screen or right of screen
			    || (Position.X + radius > screenSize.Width && currentVelocity.dx > 0))
			{
				//switchs the velocity
				currentVelocity = new CGVector(-currentVelocity.dx, currentVelocity.dy);
				PhysicsBody.Velocity = currentVelocity;
			}


			if (Position.Y - radius < 0 && currentVelocity.dy < 0) //hits the bottom of the screen
			{
				if (((GameScene)Parent).DecreaseLives() == true) //decreases the players lives
					return;
				//puts the ball back onto the paddle like the reset method but deson't increase the ball speed
				Position = new CGPoint(((GameScene)Parent).paddle.Position.X, screenSize.Height / 8 + (50 * (float)screenSize.Width / 375));
				onPaddle = true;
				PhysicsBody.Velocity = new CGVector(0, 0);
				currentVelocity = new CGVector(0, GetLength(currentVelocity));
			}
			if(Position.Y + radius > screenSize.Height && currentVelocity.dy > 0) //if it hits the top of the screen
			{
				//switches the vertical velocity
				currentVelocity = new CGVector(currentVelocity.dx, -currentVelocity.dy);
				PhysicsBody.Velocity = currentVelocity;
			}

		}
	}
}

