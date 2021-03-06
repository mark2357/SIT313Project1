﻿using System;
using SpriteKit;
using Foundation;
using UIKit;
using CoreGraphics;

namespace BreakoutGame
{
	public class Paddle : SKShapeNode
	{
		public Paddle(CGRect frame, int diff)
		{

			var pathPaddle = new CGPath();
			float paddleSize = (60 - (10 * diff)) * (float)frame.Width / 375; //creates a size multiplier that scales with screen size 
			//adds the circles and lines to the paddle
			pathPaddle.AddArc(-paddleSize, 0f, 15f * (float)frame.Width / 375, 3f * (float)Math.PI / 2f, (float)Math.PI / 2f, true);
			pathPaddle.AddLineToPoint(paddleSize, 15f * (float)frame.Width / 375);
			pathPaddle.AddArc(paddleSize, 0f, 15f * (float)frame.Width / 375, (float)Math.PI / 2f, 3f * (float)Math.PI / 2f, true);
			pathPaddle.AddLineToPoint(-paddleSize, -15f * (float)frame.Width / 375);


			Path = pathPaddle; //adds path to paddle
			Position = new CGPoint(frame.Width / 2, frame.Height / 8);//sets pos
			FillColor = new UIColor(0f, 1f, 0f, 1f); //set colour to green
			PhysicsBody = SKPhysicsBody.CreateBodyFromPath(pathPaddle); //creates colision hitbox
			PhysicsBody.AffectedByGravity = false;

			//sets so only collides with the ball
			PhysicsBody.CategoryBitMask = (uint)Collision_Bits.Paddle;
			PhysicsBody.CollisionBitMask = (uint)Collision_Bits.Ball;
			PhysicsBody.ContactTestBitMask = (uint)Collision_Bits.Ball;
		}
	}
}

