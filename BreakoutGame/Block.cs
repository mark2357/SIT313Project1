using System;
using SpriteKit;
using Foundation;
using UIKit;
using CoreGraphics;

namespace BreakoutGame
{
	public class Block : SKShapeNode
	{
		//is static so there is only one variable for all blocks
		public static int blockCount = 0; //used to keep track of the total tiles

		public Block(CGPoint pos, float w, float h)
		{
			blockCount++; //increments the total number of blocks
			Position = pos; //sets the position
			CGPath path = new CGPath();
			CGPoint[] points = new CGPoint[5]; //creates the 5 points used for the block
			//(5 are used as one mone is needed to go back to the start point)
			//sets the points positions based on the width and height of the block
			points[0] = new CGPoint(-w / 2, -h / 2);
			points[1] = new CGPoint(w / 2, -h / 2);
			points[2] = new CGPoint(w / 2, h / 2);
			points[3] = new CGPoint(-w / 2, h / 2);
			points[4] = new CGPoint(-w / 2, -h / 2);
			path.AddLines(points); //creates the path from the points
			Path = path;
			FillColor = new UIColor(0f, 0f, 1f, 1f); //sets the colour to blue


			PhysicsBody = SKPhysicsBody.CreateBodyFromPath(path); //sets the hitbox/collision box
			PhysicsBody.AffectedByGravity = false;

			//sets the collisions so blocks only hit the ball
			PhysicsBody.CategoryBitMask = (uint)Collision_Bits.Block;
			PhysicsBody.CollisionBitMask = (uint)Collision_Bits.Ball;
			PhysicsBody.ContactTestBitMask = (uint)Collision_Bits.Ball;
		}

		public static void BlockDestroyed() //called when a block is destroyed
		{
			blockCount--;
		}
	}
}

