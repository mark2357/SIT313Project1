using System;
using SpriteKit;
using Foundation;
using UIKit;
using CoreGraphics;

namespace BreakoutGame
{
	public class Paddle : SKShapeNode
	{
		public Paddle(CGRect frame)
		{

			var pathPaddle = new CGPath();
			pathPaddle.AddArc(-50f, 0f, 15f, 3f * (float)Math.PI / 2f, (float)Math.PI / 2f, true);
			pathPaddle.AddLineToPoint(50f, 15f);
			pathPaddle.AddArc(50f, 0f, 15f, (float)Math.PI / 2f, 3f * (float)Math.PI / 2f, true);
			pathPaddle.AddLineToPoint(-50f, -15f);

			Path = pathPaddle;
			Position = new CGPoint(frame.Width / 2, frame.Height / 8);
			FillColor = new UIColor(0f, 1f, 0f, 1f);
			PhysicsBody = SKPhysicsBody.CreateBodyFromPath(pathPaddle);
			PhysicsBody.AffectedByGravity = false;
			//PhysicsBody.Dynamic = false;
			PhysicsBody.CategoryBitMask = (uint)Collision_Bits.Paddle;
			PhysicsBody.CollisionBitMask = (uint)Collision_Bits.Ball;
			PhysicsBody.ContactTestBitMask = (uint)Collision_Bits.Ball;
		}
	}
}

