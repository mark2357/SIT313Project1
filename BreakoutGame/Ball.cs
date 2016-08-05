using System;
using SpriteKit;
using Foundation;
using UIKit;
using CoreGraphics;

namespace BreakoutGame
{
	public class Ball : SKShapeNode
	{
		public CGRect screenSize;
		public CGVector currentVelocity;
		public bool col = false;
		private float radius;
		public bool onPaddle = true;
		public bool reset = false;

		public Ball(CGRect frame, float r, int diff)
		{
			var pathBall = new CGPath();
			radius = r;
			pathBall.AddArc(0f, 0f, radius, 0, 2f * (float)Math.PI, true);
			Path = pathBall;
			screenSize = frame;
			FillColor = new UIColor(1f, 0f, 0f, 1f);
			Position = new CGPoint(frame.Width / 2, frame.Height / 8 + 50);
			PhysicsBody = SKPhysicsBody.CreateBodyFromPath(pathBall);
			PhysicsBody.AffectedByGravity = false;
			currentVelocity = new CGVector(0, 150 + 50 * diff);
			PhysicsBody.Friction = 0f;
			PhysicsBody.LinearDamping = 0f;

			//ball.PhysicsBody.Dynamic = false;
			PhysicsBody.CategoryBitMask = (uint)Collision_Bits.Ball;
			PhysicsBody.CollisionBitMask = (uint)Collision_Bits.Paddle | (uint)Collision_Bits.Block;
			PhysicsBody.ContactTestBitMask = (uint)Collision_Bits.Paddle | (uint)Collision_Bits.Block;
		}
		public void Reset()
		{
			Position = new CGPoint(((GameScene)Parent).paddle.Position.X, screenSize.Height / 8 + 50);
			onPaddle = true;
			PhysicsBody.Velocity = new CGVector(0, 0);
			currentVelocity = new CGVector(0, GetLength(currentVelocity) * 1.1f);
			col = false;
		}
		public void collisionWithPaddle(CGPoint p)
		{
			CGVector dir = new CGVector(Position.X - p.X, Position.Y - p.Y);
			dir = Normalise(dir);
			//float dirLength = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
			//dir = new CGVector(dir.X / dirLength, dir.Y / dirLength);
			float speed = GetLength(currentVelocity);
			currentVelocity = new CGVector(dir.dx * speed, dir.dy * speed);
		}

		public void collisionWithBlock(CGPoint p)
		{
			CGVector dir = new CGVector(Position.X - p.X, Position.Y - p.Y);
			/*
			if (dir.dx > dir.dy)
			{
				currentVelocity = new CGVector(currentVelocity.dx, -currentVelocity.dy);
			}
			else
			{
				currentVelocity = new CGVector(-currentVelocity.dx, currentVelocity.dy);
			}
			*/
			dir = Normalise(dir);
			float speed = GetLength(currentVelocity);
			currentVelocity = new CGVector(dir.dx * speed, dir.dy * speed);
		}
		private CGVector Normalise(CGVector vect)
		{
			float vectLength = (float)Math.Sqrt((vect.dx * vect.dx) + (vect.dy * vect.dy));
			vect = new CGVector(vect.dx / vectLength, vect.dy / vectLength);
			return vect;
		}
		private float GetLength(CGVector vect)
		{
			float vectLength = (float)Math.Sqrt((vect.dx * vect.dx) + (vect.dy * vect.dy));
			return vectLength;
		}
		public void ScreenColTest()
		{
			
			if ((Position.X - radius < 0 && currentVelocity.dx < 0)
			    || (Position.X + radius > screenSize.Width && currentVelocity.dx > 0))
			{
				currentVelocity = new CGVector(-currentVelocity.dx, currentVelocity.dy);
				PhysicsBody.Velocity = currentVelocity;
			}


			if (Position.Y - radius < 0 && currentVelocity.dy < 0)
			{
				((GameScene)Parent).DecreaseLives();
				Position = new CGPoint(((GameScene)Parent).paddle.Position.X, screenSize.Height / 8 + 50);
				onPaddle = true;
				PhysicsBody.Velocity = new CGVector(0, 0);
				currentVelocity = new CGVector(0, GetLength(currentVelocity));
			}
			if(Position.Y + radius > screenSize.Height && currentVelocity.dy > 0)
			{
				currentVelocity = new CGVector(currentVelocity.dx, -currentVelocity.dy);
				PhysicsBody.Velocity = currentVelocity;
			}

		}
	}
}

