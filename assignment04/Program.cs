using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace assignment04
{
	public class Program : ChaosEngine
	{

		public static SlideSprite player;
		public static SlideSprite[,] goals;
		public static SlideSprite[,] walls;
		public static SlideSprite[,] blocks;
		public static Sprite[,] floors;
		public static SoundPlayer juke = new SoundPlayer(Properties.Resources.Shop);
		public static string CurrentLevel = Properties.Resources.LevelStart;
		public static Bitmap CurrentPlayer = Properties.Resources.player;
		public static int x;
		public static int y;
		public static int width;
		public static int height;
		public static bool WinGame = false;
		public static int LevelCount = 2;
		public static int PlayerCount = 1;
		public static int MoveCount = 0;

		protected override void OnKeyDown(KeyEventArgs e)
		{
			#region Blah Blah Main Key Events
			if (StartFlag || !(CurrentLevel == Properties.Resources.WinGame))
			{
				if (e.KeyCode == Keys.N)
				{
					if (WinGame)
					{
						LevelCount++;
						switch (LevelCount)
						{
							case 1:
								CurrentLevel = Properties.Resources.LevelStart;
								juke = new SoundPlayer(Properties.Resources.Shop);
								juke.PlayLooping();
								break;
							case 2:
								CurrentLevel = Properties.Resources.Level1;
								juke = new SoundPlayer(Properties.Resources.Sonic);
								juke.PlayLooping();
								break;
							case 3:
								CurrentLevel = Properties.Resources.Level2;
								juke = new SoundPlayer(Properties.Resources.SpiderDance);
								juke.PlayLooping();
								break;
							case 4:
								CurrentLevel = Properties.Resources.Level3;
								juke = new SoundPlayer(Properties.Resources.Megalovania);
								juke.PlayLooping();
								break;
							case 5:
								CurrentLevel = Properties.Resources.WinGame;
								juke = new SoundPlayer(Properties.Resources.RadioB);
								juke.PlayLooping();
								LevelCount = 0;
								break;
						}
						WinGame = false;
						StartGame();
					}

				}
				if(e.KeyCode == Keys.Q)
				{
					CurrentLevel = Properties.Resources.LevelStart;
					juke = new SoundPlayer(Properties.Resources.Shop);
					juke.PlayLooping();
					StartFlag = false;
					StartGame();
				}
				if (e.KeyCode == Keys.Right)
				{
					if (canMoveTo(x + 1, y, 1, 0)) x++;
					if (blocks[x, y] != null) moveBlock(x, y, 1, 0);
				}
				if (e.KeyCode == Keys.Left)
				{
					if (canMoveTo(x - 1, y, -1, 0)) x--;
					if (blocks[x, y] != null) moveBlock(x, y, -1, 0);
				}
				if (e.KeyCode == Keys.Up)
				{
					if (canMoveTo(x, y - 1, 0, -1)) y--;
					if (blocks[x, y] != null) moveBlock(x, y, 0, -1);
				}
				if (e.KeyCode == Keys.Down)
				{
					if (canMoveTo(x, y + 1, 0, 1)) y++;
					if (blocks[x, y] != null) moveBlock(x, y, 0, 1);
				}
			}
			if (!StartFlag)
			{
				if (e.KeyCode == Keys.C)
				{
					PlayerCount++;
					switch (PlayerCount)
					{
						case 1:
							CurrentPlayer = Properties.Resources.player;
							player.Image = Properties.Resources.player;
							break;
						case 2:
							CurrentPlayer = Properties.Resources.player1;
							player.Image = Properties.Resources.player1;
							break;
						case 3:
							CurrentPlayer = Properties.Resources.player2;
							player.Image = Properties.Resources.player2;
							break;
						case 4:
							CurrentPlayer = Properties.Resources.player3;
							player.Image = Properties.Resources.player3;
							PlayerCount = 0;
							break;
					}
				}
				if (e.KeyCode == Keys.Space)
				{
					CurrentLevel = Properties.Resources.Level1;
					juke = new SoundPlayer(Properties.Resources.Sonic);
					StartFlag = true;
					StartGame();
				}
			}
			if(e.KeyCode == Keys.R)
			{
				StartGame();
			}
			#endregion
			player.TargetX = x * 100;
			player.TargetY = y * 100;
		}

		public void moveBlock(int i, int j, int dx, int dy)
		{
			blocks[i + dx, j + dy] = blocks[i, j];
			blocks[i, j] = null;

			blocks[i + dx, j + dy].TargetX = (i + dx) * 100;
			blocks[i + dx, j + dy].TargetY = (j + dy) * 100;
			if (goals[i + dx, j + dy] != null)
			{
				blocks[i + dx, j + dy].Image = Properties.Resources.final;
				blocks[i + dx, j + dy].correct = true;
				if (CheckWin())
				{
					WinGame = true;
					Console.WriteLine("Win");
				}
				else WinGame = false;
			}
			else
			{
				blocks[i + dx, j + dy].Image = Properties.Resources.box;
				blocks[i + dx, j + dy].correct = false;
			}

		}

		public static void StartGame()
		{
			parent.children.Clear();
			juke.PlayLooping();
			String map = CurrentLevel;
			String[] lines = map.Split('\n');
			width = 10;
			height = 10;
			goals = new SlideSprite[width, height];
			walls = new SlideSprite[width, height];
			blocks = new SlideSprite[width, height];
			floors = new SlideSprite[width, height];
			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					floors[i, j] = new SlideSprite(Properties.Resources.floor, i * 100, j * 100);
					floors[i, j].correct = true;
					parent.Add(floors[i, j]);
					if (lines[j][i] == 'g' || lines[j][i] == 'B')
					{
						goals[i, j] = new SlideSprite(Properties.Resources.goal, i * 100, j * 100);
						goals[i, j].correct = true;
						Program.parent.Add(goals[i, j]);
					}
					if (lines[j][i] == 'p')
					{
						walls[i, j] = new SlideSprite(Properties.Resources.party, i * 100, j * 100);
						walls[i, j].correct = true;
						Program.parent.Add(walls[i, j]);
					}
					if (lines[j][i] == 'w')
					{
						walls[i, j] = new SlideSprite(Properties.Resources.wall, i * 100, j * 100);
						walls[i, j].correct = true;
						Program.parent.Add(walls[i, j]);
					}
					if (lines[j][i] == 'b' || lines[j][i] == 'B')
					{
						blocks[i, j] = new SlideSprite(Properties.Resources.box, i * 100, j * 100);
						if (lines[j][i] == 'B')
						{
							blocks[i, j].Image = Properties.Resources.final;
							blocks[i, j].correct = true;
						}

					}
					if (lines[j][i] == 'c')
					{
						player = new SlideSprite(CurrentPlayer, i * 100, j * 100);

						x = i;
						y = j;
					}
				}
			}
			for (int j = 0; j < height; j++)
				for (int i = 0; i < width; i++)
					if (blocks[i, j] != null) Program.parent.Add(blocks[i, j]);
			player.correct = true;
			parent.Add(player);

		}

		public Boolean canMoveTo(int i, int j, int dx, int dy)
		{

			if (walls[i, j] == null && blocks[i, j] == null) return true;
			if (walls[i, j] != null) return false;
			if (blocks[i, j] != null && blocks[i + dx, j + dy] == null && walls[i + dx, j + dy] == null) return true;
			return false;

		}


		public Boolean CheckWin()
		{
			Boolean win = true;
			foreach(Sprite i in parent.children)
			{
				if(i.isSlideSprite)
				{
					if (!i.correct)
					{
						win = false;
					}
				}
			}
			return win;
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			fixScale();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			fixScale();
		}

		private void fixScale()
		{
			parent.Scale = Math.Min(ClientSize.Width, ClientSize.Height) / (Math.Max(height,width) * 100.0f);
			parent.X = (ClientSize.Width - (100 * Width * parent.Scale)) / 2;
			parent.Y = (ClientSize.Height - (100 * Height * parent.Scale)) / 2;
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			StartGame();
			Application.Run(new Program());
		}
	}
}