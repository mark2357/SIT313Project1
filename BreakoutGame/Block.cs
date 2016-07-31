using System;
using SpriteKit;
using Foundation;
using UIKit;
using CoreGraphics;

namespace BreakoutGame
{
	public class Block : SKShapeNode
	{
		public static int blockCount = 0;

		public Block(CGPoint pos, float w, float h)
		{
			blockCount++;
			Position = pos;
			CGPath path = new CGPath();
			CGPoint[] points = new CGPoint[5];
			points[0] = new CGPoint(-w / 2, -h / 2);
			points[1] = new CGPoint(w / 2, -h / 2);
			points[2] = new CGPoint(w / 2, h / 2);
			points[3] = new CGPoint(-w / 2, h / 2);
			points[4] = new CGPoint(-w / 2, -h / 2);
			path.AddLines(points);
			Path = path;
			FillColor = new UIColor(0f, 0f, 1f, 1f);

			PhysicsBody = SKPhysicsBody.CreateBodyFromPath(path);
			PhysicsBody.AffectedByGravity = false;

			PhysicsBody.CategoryBitMask = (uint)Collision_Bits.Block;
			PhysicsBody.CollisionBitMask = (uint)Collision_Bits.Ball;
			PhysicsBody.ContactTestBitMask = (uint)Collision_Bits.Ball;
		}

		public static void BlockDestroyed()
		{
			blockCount--;
		}
	}
}

