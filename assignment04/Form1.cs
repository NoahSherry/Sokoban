using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace assignment04
{
	public partial class ChaosEngine : Form
	{
		public static Form form;
		public static Thread thread;
		public static Thread thread2;
		public static int fps = 30;
		public static double running_fps = 30.0;
		public static Boolean StartFlag = false;
		public static Sprite parent = new Sprite();

		public ChaosEngine()
		{
			InitializeComponent();
			DoubleBuffered = true;
			form = this;
			thread = new Thread(new ThreadStart(Update));
			thread.Start();
			thread2 = new Thread(new ThreadStart(Render));
			thread2.Start();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			thread.Abort();
			thread2.Abort();
		}

		public static void Update()
		{
			DateTime last = DateTime.Now;
			DateTime now = last;
			TimeSpan frameTime = new TimeSpan(10000000 / fps);
			while (true)
			{
				DateTime temp = DateTime.Now;
				running_fps = .9 * running_fps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
				now = temp;
				TimeSpan diff = now - last;
				if (diff.TotalMilliseconds < frameTime.TotalMilliseconds)
					Thread.Sleep((frameTime - diff).Milliseconds);
				last = DateTime.Now;
				parent.Act();
			}
		}

		public static void Render()
		{
			DateTime last = DateTime.Now;
			DateTime now = last;
			TimeSpan frameTime = new TimeSpan(10000000 / fps);
			while (true)
			{
				DateTime temp = DateTime.Now;
				running_fps = .9 * running_fps + .1 * 1000.0 / (temp - now).TotalMilliseconds;
				now = temp;
				TimeSpan diff = now - last;
				if (diff.TotalMilliseconds < frameTime.TotalMilliseconds)
					Thread.Sleep((frameTime - diff).Milliseconds);
				last = DateTime.Now;
				form.Invoke(new MethodInvoker(form.Refresh));
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{

			int opacity = 200;
			parent.Render(e.Graphics);
			Color winRectCol = Color.FromArgb(opacity, Color.LightBlue);
			Brush brush = new SolidBrush(winRectCol);
			Font text = new Font("Ubuntu", Math.Min(ClientSize.Width, ClientSize.Height) / 30);

			if (Program.CurrentLevel == Properties.Resources.LevelStart)
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				int x = ClientSize.Width / 2;
				int y = ClientSize.Height / 2;
				e.Graphics.DrawString("Welcome to Sokoban!", text, Brushes.Blue, x, y - 100, stringFormat);
				e.Graphics.DrawString("Press Space to Play", text, Brushes.Blue, x, y - 50, stringFormat);
				e.Graphics.DrawString("Press C to Change your Character", text, Brushes.Blue, x, y, stringFormat);
			}
			if(Program.CurrentLevel == Properties.Resources.WinGame)
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				int x = ClientSize.Width / 2;
				int y = ClientSize.Height / 2;
				e.Graphics.DrawString("Congratz! You win!", text, Brushes.Blue, x, y - 100, stringFormat);
				e.Graphics.DrawString("Press Q to Restart.", text, Brushes.Blue, x, y - 50, stringFormat);
			}
			if (Program.WinGame)
			{
				e.Graphics.FillRectangle(brush, 0, 0, ClientSize.Width, ClientSize.Height);
				e.Graphics.DrawString("You Win!", text, Brushes.Black, ClientSize.Height / 4, ClientSize.Width / 2 - 50);
				e.Graphics.DrawString("Press N to Go on to the Next Level", text, Brushes.Black, ClientSize.Height / 4, ClientSize.Width / 2 - 25);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}
	}
}
