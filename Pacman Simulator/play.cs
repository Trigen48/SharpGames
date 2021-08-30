using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Pacman_Simulator.robots;

namespace Pacman_Simulator
{
    public partial class play : Form
    {
        int width=19,height=22;
        int blocksize = 15;
        Graphics g;
        private Image background;
        bool Setup = false;
        
        char[][] map;
        int p1x=3, p1y=10, p2x=15, p2y=10;

        string frmt = "";
        bool gameover = false;
        bool isp1 = true;
        IAI pl;
        int[] mv;
        short dots;// = 200;

        short p1Score=0, p2Score=0;
        public play()
        {
            InitializeComponent();
            CreateStage();
            frmt = "Player 1   (A)pts      Player 2   (B)pts      Remaining (C)";
            CreateScore();

        }

        private void CreateScore()
        {
            label1.Text = frmt.Replace("(A)", p1Score.ToString()).Replace("(B)", p2Score.ToString()).Replace("(C)", dots.ToString());

        }
        //private

        private void CreateStage()
        {
  
            string [] mp=System.IO.Directory.GetFiles("maps\\FinalMaps\\","*.pmap");

            if (mp.Length != 0)
            {

                ShuffleList<string> sf = new ShuffleList<string>();
                sf.AddRange(mp);


                if (sf.Count > 1)
                {
                    sf.ShuffleInplace();
                    sf.ShuffleShift();
                }


                string[] ssl = System.IO.File.ReadAllLines(sf.PopRandom());

                sf.Clear();
                sf.AddRange(ssl);

                if (sf[sf.Count-1].Trim() == "")
                {
                    sf.RemoveAt(sf.Count - 1);
                }

                map = new char[sf.Count][];

                for (int y = 0; y < map.Length; y++)
                {

                    int le = sf[0].Length;

                        string m = sf[y];

                    if (le != sf[y].Length)
                    {

                        int l = le - sf[y].Length;


                        for (int xx = 0; xx < le;xx++ )
                        {
                            m += " ";
                        }
                    }
                    map[y] = m.ToCharArray();


                }
                ssl= null;
                sf.Clear();
                sf = null;
            }
            else
            {

                map = new char[height][];

                map[0] = "###################".ToCharArray();
                map[1] = "#........#........#".ToCharArray();
                map[2] = "#*##.###.#.###.##*#".ToCharArray();
                map[3] = "#.##.###.#.###.##.#".ToCharArray();
                map[4] = "#.................#".ToCharArray();
                map[5] = "#.##.#.#####.#.##.#".ToCharArray();
                map[6] = "#....#...#...#....#".ToCharArray();
                map[7] = "####.###.#.###.####".ToCharArray();
                map[8] = "####.#.......#.####".ToCharArray();
                map[9] = "####.#.## ##.#.####".ToCharArray();
                map[10] = "   A...#   #...B   ".ToCharArray();
                map[11] = "####.#.## ##.#.####".ToCharArray();
                map[12] = "####.#.......#.####".ToCharArray();
                map[13] = "####.#.#####.#.####".ToCharArray();
                map[14] = "#........#........#".ToCharArray();
                map[15] = "#.##.###.#.###.##.#".ToCharArray();
                map[16] = "#*.#...........#.*#".ToCharArray();
                map[17] = "##.#.#.#####.#.#.##".ToCharArray();
                map[18] = "#....#...#...#....#".ToCharArray();
                map[19] = "#.######.#.######.#".ToCharArray();
                map[20] = "#.................#".ToCharArray();
                map[21] = "###################".ToCharArray();
            }

            width = (int)map[0].Length;
            height = (int)map.Length;

            background = new Bitmap(width * blocksize, height * blocksize);

            g = Graphics.FromImage(background);

            g.Clear(Color.Black);
            SolidBrush s= new SolidBrush(Color.White), ss =new SolidBrush( Color.Maroon),sss= new SolidBrush(Color.Blue),ssc= new SolidBrush(Color.Brown);

            Rectangle pointr;
            int de = 0;
            dots = 0;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {

                    switch (map[j][i])
                    {

                        case '#':
                            pointr = new Rectangle((int)(i * blocksize)+1, (int)(j * blocksize)+1, (int)(blocksize)-2, (int)(blocksize)-2);
                            g.FillRectangle(ss, pointr);

                            break;

                        case '.':
                            dots++;
                            de = (int)(blocksize * 0.60f);
                            pointr = new Rectangle((int)(i * blocksize) + de, (int)(j * blocksize) + de, (int)(blocksize) - (de*2), (int)(blocksize) - (de*2));
                            g.FillEllipse(s,pointr);
                            break;

                        case '*':
                            dots++;
                            de = (int)(blocksize * 0.75f);
                            pointr = new Rectangle((int)(i * blocksize) + de, (int)(j * blocksize) + de, (int)(blocksize) - (de*2), (int)(blocksize) - (de*2));
                            g.FillEllipse(sss,pointr);
                            break;

                        case '!':
                            
                            de = (int)(blocksize * 0.75f);
                            pointr = new Rectangle((int)(i * blocksize) + de, (int)(j * blocksize) + de, (int)(blocksize) - (de*2), (int)(blocksize) - (de*2));
                            g.FillEllipse(ssc,pointr);
                            break;

                        case 'A':
                            p1x = i;
                            p1y = j;
                            break;

                        case 'B':
                            p2x = i;
                            p2y = j;
                            break;
                    }

                }
            }
            //background.Save("out.png");
            Setup = true;
            isp1 = true;
        }

        private void EatDot(int x,int y)
        {
            g.FillRectangle(new SolidBrush(Color.Black) ,x * blocksize, y * blocksize, blocksize, blocksize);
        }

        private void DrawPlayers()
        {
            DrawPac(p1x, p1y, Color.Yellow);
            DrawPac(p2x, p2y, Color.Lime);
        }

        private void DrawPac(int x, int y, Color cc)
        {
            Rectangle ptr = new Rectangle(1, 1, blocksize-2, blocksize-2);
            Image pac = new Bitmap(blocksize, blocksize);
            Graphics b = Graphics.FromImage(pac);
            b.FillPie(new SolidBrush(cc), ptr, 0,360);
            c.DrawImage(pac, new Point(x * blocksize, y * blocksize));
        }

        private void tmr_Tick(object sender, EventArgs e)
        {

            tmr.Stop();
            GameLogic();
            Invalidate();

            if (dots > 0|| gameover) tmr.Start();
            else
            {
                if (!gameover)
                {
                    string c = "";

                    if (p1Score > p2Score)
                    {
                        c = "Player 1 wins with " + p1Score.ToString() + " TO " + p2Score.ToString();
                    }
                    else if (p1Score < p2Score)
                    {
                        c = "Player 2 wins with   " + p2Score.ToString() + "pts   -   " + p1Score.ToString() + "pts";
                    }


                    label1.Text = c;
                }

            }
 
        }

        void Player1Move(int[] mv)
        {

            map[p1y][p1x] = ' ';
            p1x = (int)mv[0];
            p1y = (int)mv[1];

            if (isEatable(mv, 0))
            {
                EatDot((int)mv[0], (int)mv[1]);

                char i = map[mv[1]][mv[0]];

                if (i == '.')
                {
                    p1Score += 1; dots -= 1;
                }
                else if (i == '*')
                {
                    p1Score += 10; dots -= 1;
                }
                else if (i == 'B')
                {
                    p2x = (int)width / 2;
                    p2y = (int)height / 2;
                    map[p2y][p2x] = 'B';
                }
                else if (i == '!')
                {
                    p1x = (int)width / 2;
                    p1y = (int)height / 2;
                    map[p1y][p1x] = 'A';
                }

            }
   
                map[p1y][p1x] = 'A';
            
            CreateScore();

        }

        void Player2Move(int[] mv)
        {

            map[p2y][p2x] = ' ';
            p2x = (int)mv[0];
            p2y = (int)mv[1];

            if (isEatable(mv, 1))
            {
                EatDot((int)mv[0], (int)mv[1]);

                char i = map[mv[1]][mv[0]];

                if (i == '.')
                {
                    p2Score += 1; dots -= 1;
                }
                else if (i == '*')
                {
                    p2Score += 10; dots -= 1;
                }
                else if (i == 'B')
                {
                    map[p1y][p1x] = ' ';
                    p1x = (int)width / 2;
                    p1y = (int)height / 2;
                    map[p1y][p1x] = 'B';
                }
                else if (i == '!')
                {
                    p2x = (int)width / 2;
                    p2y = (int)height / 2;
                    map[p2y][p2x] = 'A';
                }
            }

                map[p2y][p2x] = 'B';
       
            CreateScore();

        }

        bool isEatable(int[] mv,int id)
        {
            char i = map[mv[1]][mv[0]];

            return i=='.'|| i=='!'|| i=='*' || i==('A'+id);
        }

      //  int it = 0;
        private string[] GetMap(int pp)
        {

            char[][] bc = new char[height][];
                
            map.CopyTo(bc, 0) ;


            if (pp == 1)
            {
                bc[p1y][p1x] = 'B';
                bc[p2y][p2x] = 'A';
            }

            string[] c = new string[height]; ;

            for (int x = 0; x < height; x++)
            {
                c[x] = new string(bc[x]);
            }

           // System.IO.File.WriteAllLines("replay\\"+it.ToString() + " - Player " + (pp + 1).ToString()+".txt", c);
          //  it++;
            return c;
        }
        
        private void GameLogic()
        {

            if (isp1)
            {
                pl = new ScoreRunner();//new SmartPacman();
               //pl = new ScoreRunner();
                mv=pl.getmove(GetMap(0), "store1\\");
                if (mv[0] == p1x && mv[1] == p1y)
                {
                    label1.Text = "Player 1 did not make a move. Player 2 wins";
                    gameover = true;
                    return;
                }
                Player1Move(mv);
                

            }
            else
            {
                pl = new ScoreRunner();// new SmartPacman();
                mv = pl.getmove(GetMap(1), "store2\\");

                if (mv[0] == p2x && mv[1] == p2y)
                {
                    label1.Text = "PLayer 2 did not make a move. Player 1 wins";
                    gameover = true;
                    return;
                }
                Player2Move(mv);
                map[p1y][p1x] = 'A';
            }


            isp1 ^= true;
        }

        private void play_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Location.X, 0);
            tmr.Start();
        }
        Graphics c;

        private void play_Paint(object sender, PaintEventArgs e)
        {

            if (Setup == false) return;
            c = e.Graphics;
            e.Graphics.DrawImage(background,new Point());
            //BackgroundImage = ;
            DrawPlayers();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tmr.Enabled) tmr.Stop();

            CreateStage();
            p1Score = 0;
            p2Score = 0;

            p1x = 3; p1y = 10; p2x = 15; p2y = 10;

            tmr.Start();
        }

        private void play_MaximizedBoundsChanged(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Normal;
        }

        public int GetSpotScore(char r)
        {
            if (r == '.') return 1;
            if (r == '*') return 10;
            return 0;
        }

    }
}
