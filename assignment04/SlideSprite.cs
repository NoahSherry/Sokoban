using System;
using System.Drawing;

namespace assignment04
{
	public class SlideSprite : Sprite
	{
		public int Velocity { get; internal set; }
		float k = .2f;
		public float K
		{
			get { return k; }
			set { k = value; }
		}

		float c = .2f;
		public float C
		{
			get { return c; }
			set { c = value; }
		}

		public SlideSprite(Bitmap image) : base(image)
		{

		}
		public SlideSprite(Bitmap image, int x, int y) : base(image)
		{
			X = x;
			Y = y;
			TargetX = x;
			TargetY = y;
			Velocity = 20;
			isSlideSprite = true;
		}

		public override void Paint(Graphics g)
		{
			g.DrawImage(Image, 0, 0);
		}

		public override void Act()
		{
			
			#region Check Horizontal Movement
			if (X + Velocity < TargetX)
			{
				X += Velocity;
			}
			else if (X - Velocity > TargetX)
			{
				X -= Velocity;
			}
			else if (Math.Abs(X - TargetX) <= Velocity)
			{
				X = TargetX;
			}
			#endregion

			#region Check Vertical Movement
			if (Y + Velocity < TargetY)
			{
				Y += Velocity;
			}
			else if (Y - Velocity > TargetY)
			{
				Y -= Velocity;
			}
			else if (Math.Abs(Y - TargetY) <= Velocity)
			{
				Y = TargetY;
			}
			#endregion
			

			//X = TargetX;
			//Y = TargetY;
		}

	}
}
