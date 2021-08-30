using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace Pacman_Simulator.robots
{
    class SmartPacman : IAI
    {        
        int[] xs1, xs2;
        string stor;
        int[] IAI.getmove(string[] map, string store)
        {
            stor = store;
            xs1 = new int[2];
            xs2 = new int[2];
            Main(map);
            return xs1;
        }



        // direction nodes
        int[] dx; int[] dy;
        // node index 
        const int X = 0, Y = 1, value = 2;

        int height = 22, width = 19;
        const short dotScore = 1, bonusScore = 10;
        const int Directions = 4;
        const char wall = '#', pill = '!', bonus = '*', dot = '.', empty = ' ', playA = 'A', playB = 'B';
        const short TotalDots = 183;
        NodeList<int[]> stack;
        List<int[]> list;
        const string GameStateFile = "state.st";

        char[][] map;
        bool IsFirst;

        // player cords
        int[] s1;
        int[] s2;
        int[][][] path;
        // Scoring and total dots
        short OverallDots, OverallScore;
        int[] k;
        bool IsNewGame;


        //bool isp1 = false;
        void Main(string[] args)
        {
            height = args.Length;
            width = args[0].Length;
            SetupVar();
            LoadMap(args);
            DoLogic();
            SaveMap();
        }

        void SetupVar()
        {

            const int LocationStateSize = 3;

            s1 = new int[LocationStateSize];
            s2 = new int[LocationStateSize];

            dx = new int[Directions];
            dy = new int[Directions];
            OverallScore = 0;
            OverallDots = 0;

            path = new int[width][][];

            for (int i = 0; i < width; i++) path[i] = new int[height][];

            stack = new NodeList<int[]>();
            list = new List<int[]>();
        }


        #region State File Work
        ////////////////////////////  State File/////////////////////////////////////
        void SetGameState()
        {
            IsFirst = (bool)(s1[0] == 3 ? true : false);
            s1[2] = 1; // Set Poison Pill
            s2[2] = 1; // Set Poison Pill
        }

        public bool IsPortal(int[] cord)
        {
            // 0,10 will result you being in 18,10?

            if(cord[0]==-1 && cord[1]==10)
            {
                cord[0]=18;
                return true;
            }

            if (cord[0] == 19 && cord[1] == 10)
            {
                cord[0] = 0;
                return true;
            }

            return false;
        }

        void LoadGameState()
        {
            if (IsNewGame)
            {
                SetGameState();
                return;
            }


            if (System.IO.File.Exists(stor + GameStateFile))
            {
                byte[] tmp=System.IO.File.ReadAllBytes(stor+GameStateFile);

                IsFirst = (bool)(tmp[0] == 1 ? true : false);
                s1[2] = tmp[1];
                s2[2] = tmp[2];
            }
            else
            {
                SetGameState();
            }
        }

        void SaveGameState()
        {
            byte[] fl = new byte[3];
            if (IsFirst) fl[0] = 1; else fl[0] = 0;
            fl[1]=(byte)s1[2];
            fl[2]=(byte)s2[2];

            System.IO.File.WriteAllBytes(stor + GameStateFile, fl);
        }
        ////////////////////////////////////////////////////////////
        #endregion




        #region Stats Work
        // Stats help
        void AddScore(short value)
        {
            OverallScore += value;
        }

        void IncrementDots()
        {
            OverallDots++;
        }

        void LoadDirectSet()
        {

            if (IsFirst)
            {
                dx[0] = 1; dx[1] = -1; dx[2] = 0; dx[3] = 0;

                dy[0] = 0; dy[1] = 0; dy[2] = 1; dy[3] = -1;
            }
            else
            {
                dx[0] = -1; dx[1] = 1; dx[2] = 0; dx[3] = 0;
                dy[0] = 0; dy[1] = 0; dy[2] = 1; dy[3] = -1;
            }


        }
        #endregion


        #region Map File Work
        // Map Section
        void LoadMap(string[] file)
        {

          //  System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open);



            map = new char[height][];

            for (int y = 0; y < height; y++)
            {
                map[y] = new char[width];
                for (int x = 0; x < width; x++)
                {
                    char c = file[y][x]; //(char)fs.Readint();

                    switch (c)
                    {
                        case empty:

                            break;

                        /*case '#':
                            break;*/

                        case dot: // normal dots
                            IncrementDots();
                            AddScore(dotScore);
                            // Path Scoring
                            break;

                        case bonus: // bonus dots
                            IncrementDots();
                            AddScore(bonusScore);
                            // Path Scoring
                            break;

                        /* case '!': // poison pill
                             break;*/

                        case playA:
                            s1[X] = x;
                            s1[Y] = y;
                            break;

                        case playB:
                            s2[X] = x;
                            s2[Y] = y;
                            break;
                    }

                    // add to map
                    map[y][x] = c;
                }



                //if (y != height - 1) fs.Position++;
            }
            //fs.Close();
           // fs = null;
        }
        void SaveMap()
        {
            int range = height-1 ;
            System.IO.StreamWriter rw = new System.IO.StreamWriter(stor+"game.state");

            for (int i = 0; i < range; i++)
            {
                rw.Write(map[i]);
                rw.Write('\n');
            }

            rw.Write(map[range]);
            rw.Close();
            rw = null;

        }

        #endregion

        // Logic Stuff here

        void GameStatus()
        {
            if (OverallDots == TotalDots || OverallDots == TotalDots - 1)
            {
                if ((s1[0] == 3 && s1[1] == 10))
                {
                    IsNewGame = true;
                    IsFirst = true;
                }
                else if ((s1[0] == 15 && s1[1] == 10))
                {
                    IsNewGame = true;
                    IsFirst = false;
                }
           
                  //  IsNewGame = true;
          
            }
        }

        #region Logic Stuff

        void DoLogic()
        {
            GameStatus();
            LoadGameState();
            LoadDirectSet();

            //Block();
            ScanMap();

            if (IsFirst) Danger(1);
           // CreateGarphics();

            if (IsFirst)
            {
                BestChoice();
                FindMainNode();
            }
            else
            {

                if (!CanAttack())
                {
                    BestChoice();
                    FindMainNode();
                }
                
            }



            Move();
            SaveGameState();

        }
       /* void Block()
        {

            for (int x = 0; x < 3; x++)
            {
                if (s1[0] == 9 && s1[1] == 9 + x)
                {
                    return;
                }
            }

            map[9][9] = '#';
            map[10][9] = '#';
            map[11][9] = '#';

        }

         void Unblock()
        {
            map[9][9] = ' ';
            map[10][9] = ' ';
            map[11][9] = ' ';
        }*/

        void CreateGarphics()
        {

            SolidBrush s = new SolidBrush(Color.Lime);

            Image im = new Bitmap(width * 50, height * 50);
            Graphics g = Graphics.FromImage(im);

            for (int x = 0; x < width; x++)
            {

                for (int y = 0; y < height; y++)
                {

                    if (path[x][y] != null)
                    {
                        g.FillRectangle(s, new Rectangle((x * 50) +4, (y * 50) +4 , 50 - 8, 50 - 8));
                        g.DrawString(path[x][y][3].ToString(),new Font("arial",12),new SolidBrush(Color.Black), new PointF((x*50)+4,(y*50)+6));
                    }
                }

            }

            g.FillEllipse(new SolidBrush(Color.Black),new Rectangle(s1[0]*50,s1[1]*50,50,50));
            g.FillEllipse(new SolidBrush(Color.Maroon), new Rectangle(s2[0] * 50, s2[1] * 50, 50, 50));

            im.Save(stor+ DateTime.Now.Ticks.ToString()+  " Outix.jpeg");

        }

        void ScanMap()
        {
            int i = 0;
            int step = 1;

            Clear();

            int[] cord;
            int[] parent = new int[] { s1[0], s1[1], 0, 0, -1, -1,0 };

            stack.Push(parent);
            path[parent[0]][parent[1]] = parent;
            while (true)
            {

                while (stack.Length() != 0)
                {
                    parent = stack.Pop();

                    i = 0;
                    while (i < 4)
                    {
                        cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]), step, 0, parent[0], parent[1],0 };

                       /* if (IsPortal(cord) && path[cord[0]][cord[1]] == null)
                        {
                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;
                            }

                            cord[3] = parent[3] + GetSpotScore(cord);
                            list.Add(cord);
                            path[cord[0]][cord[1]] = cord;
                            continue;
                        }*/

                        if (CanNavigate(cord) && path[cord[0]][cord[1]] == null)
                        {
                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;
                            }

                            cord[3] = parent[3] + GetSpotScore(cord);
                            list.Add(cord);
                            path[cord[0]][cord[1]] = cord;
                        }

                        i++;
                    }

                    
                }

                if (list.Count == 0) break;

                stack.Push(list.ToArray());
                list.Clear();
                step++;
            }

            Clear();

        }

        public int GetSpotScore(int[] pos)
        {
            char r = map[pos[1]][pos[0]];
            if (r == '.') return dotScore;
            if (r == '*') return bonusScore;
            return 0;
        }

        const int DangerValue= -10;

        void Danger(int depth)
        {

            int i = 0;


            Clear();

            int[] cord;
            int[] parent = new int[] { s2[0], s2[1], 0, 0, -1, -1 };
            int step = 1;

            stack.Push(parent);

            path[parent[0]][parent[1]][6] = 1;
    //        SeekMap[parent[0]][parent[1]] = parent;
            int count = 0;

            while (count < depth)
            {

                while (stack.Length() != 0)
                {
                    parent = stack.Pop();

                    i = 0;
                    while (i < 4)
                    {
                        cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]), 0, 0, 0, 0 };

                        if (CanNavigate(cord) && path[cord[0]][cord[1]][6]!=1)
                        {


                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;

                            }
                            path[cord[0]][cord[1]][3] += DangerValue;
                           // cord[3] = parent[3] + GetSpotScore(map[cord[Y]][cord[X]]);
                            list.Add(cord);
                            path[cord[0]][cord[1]][6] = 1;
                        }

                        i++;
                    }

                    step++;
                }

                if (list.Count == 0) break;

                stack.Push(list.ToArray());
                list.Clear();
                count++;
            }

            Clear();
            //   return null;
        }

        void BestChoice()
        {

            if (s1[0] == 1 && s1[1] == 4)
            {
                k = new int[5];
            }

            k = new int[5];
            List<int[]> bb = new List<int[]>();
            int i = 0;
            int[] cord;
            int[] parent = new int[] { s1[0], s1[1], 0, 0, -1, -1 };

            int[][][] SeekMap= new int[width][][];

            for (i = 0; i < width; i++) SeekMap[i] = new int[height][];
    
            stack.Push(parent);
            while (true)
            {
                while (stack.Length() != 0)
                {
                    parent = stack.Pop();
                    i = 0;
                    while (i < 4)
                    {
                        cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]), 0, 0, parent[0], parent[1] };


                     /*   if (IsPortal(cord) && SeekMap[cord[0]][cord[1]] == null)
                        {
                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;
                            }

                            //cord[3] = parent[3] + GetSpotScore(cord);
                            list.Add(cord);
                            SeekMap[cord[0]][cord[1]] = new int[1];
                            continue;
                        }*/

                        if (CanNavigate(cord) &&  SeekMap[cord[0]][cord[1]] == null)
                        {
                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;
                            }
                            list.Add(path[cord[0]][cord[1]]);
                            SeekMap[cord[0]][cord[1]] = new int[1];
                        }

                        i++;
                    }


                }

                if (list.Count == 0)
                {
                    break;
                }


                if (BestNodeFound(list))
                {
                    Clear();
                    bb.Clear();
                    return;
                }
                stack.Push(list.ToArray());
                bb.Clear();
                bb.AddRange(list);

                list.Clear();
   
            }

            k = bb[0];
            bb = null;
           // k = null;
        }

        void FindMainNode()
        {
            int[] last=null;
            while (k[4] != -1 && k[5] != -1)
            {
                last = k;
                k = path[k[4]][k[5]];
            }

            k = last; 
        }

        bool CanAttack()
        {

            int[] cord;

            for (int i = 0; i < Directions; i++)
            {
                cord = new int[] { s1[0] + dx[i], s1[1] + dy[i] };

                if (CanNavigate(cord) && map[cord[1]][cord[0]] == 'B')
                {
                    k = cord;
                    return true;
                }
            }

            return false;
        }

        bool BestNodeFound(List<int[]> nodes)
        {

            if (nodes.Count == 1) return false;

            List<int[]> b = new List<int[]>();

            b.AddRange(nodes.ToArray());

            b.Sort((x, y) => x[3].CompareTo(y[3]));
            b.Reverse();

            if (b[0][3] == 1)
            {
                b.Contains(b[0]);
            }

            if (b[0][3] > b[1][3])
            {
                k = b[0];
                b.Clear();
                return true;
            }

            list.Clear();

            for (int x = 0; x < b.Count; x++)
            {
                if (b[x][3] == b[0][3]) list.Add(b[x]);
            }
            /*
            int max = 0;

            int i = 0;
            int[] t = null;
            int index = 0;

            for (i = 0; i < b.Count; i++)
            {
                if (max < b[i][3])
                {
                    max = b[i][3];
                    t = b[i];
                    index = i;
                }
            }

            b.RemoveAt(index);

            for (i = 0; i < b.Count; i++)
            {
                if (max == b[i][3])
                {
                    //k = nodes[i];
                    t = null;
                    return false;
                }
            }*/
            /*
            cost.Push(nodes.ToArray());

            int[] p1, p2;

            p1 = cost.Pop();
            p2 = cost.Pop();
            //cost.Clear();


            if (p1[3] > p2[3])
            {
                k = p1;
            }
            else
            {
                return false;
            }
            */

           
            return false;
        }

        void Move()
        {
            map[s1[1]][s1[0]] = ' ';
            map[k[1]][k[0]] = 'A';

            xs1[0] = (int)k[0];
            xs1[1] = (int)k[1];

            if (k[1] == s1[1] && k[0] == s1[0])
            {
                throw new Exception();
            }
        }

        // Move Validate
        bool CanNavigate(int[] pos)
        {
            if (pos[X] > -1 && pos[X] < width && pos[Y] > -1
                && pos[Y] < height
                && map[pos[Y]][pos[X]] != wall)
                return true;

            return false;
        }

        void Clear()
        {
            list.Clear();
            stack.Clear();
        }
        #endregion



    }
}
