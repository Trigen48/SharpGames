using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman_Simulator.robots
{
    class ScoreRunner: IAI
    {

        int[] IAI.getmove(string[] map, string store)
        {
            stor = store;
            Main(map);
            return s1;
        }

        int[] dx;
        int[] dy;

        // node index 
        const int X = 0, Y = 1, value = 2;

        int height = 22, width = 19;

        const short dotScore = 2, bonusScore = 11;

        const char wall = '#', pill = '!', bonus = '*', dot = '.', empty = ' ';
        string stor = "";

        char[][] map;
        short[][] scoremap;


        // player cords
        int[] s1, s2;

        void Main(string[] args)
        {

            GetMap(args);
            SetupVar();
            BuildScoreMap();
            SeekDot();
            SaveMap();
        }

        void SetupVar()
        {
            s1 = new int[5];
            s2 = new int[5];

            dx = new int[4];
            dy = new int[4];


            dx[0] = 1;    /*dx[1] = 0;*/    dx[2] = -1;   /*dx[3] = 0;*/
            /*dy[0] = 0;*/
            dy[1] = 1;    /*dy[2] = 0;*/    dy[3] = -1;

        }

        void GetMap(string[] file)
        {

            string[] tmp = file;
            height = tmp.Length;
            width = tmp[0].Length;

            map = new char[tmp.Length][];
            for (int i = 0; i < height; i++) map[i] = tmp[i].ToCharArray();

          
        }

        void BuildScoreMap()
        {
            scoremap = new short[height][];
            for (int y = 0; y < height; y++)
            {
                scoremap[y] = new short[width];
                for (int x = 0; x < width; x++)
                {
                    switch (map[y][x])
                    {
                        case ' ':
                            scoremap[y][x] = 1;
                            break;

                        case '.':
                            scoremap[y][x] = dotScore;
                            break;
                        case '*':
                            scoremap[y][x] = bonusScore;
                            break;

                        case 'A':
                            s1[X] = x;
                            s1[Y] = y;
                            break;
                        case 'B':
                            s2[X] = x;
                            s2[Y] = y;
                            break;

                        case '!':
                            scoremap[y][x] = -1;
                            break;
                    }
                }
            }
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

        }



        bool InBoundry(int[] val)
        {
            return (val[X] > -1 && val[X] < width) && (val[Y] > -1 && val[Y] < height);
        }

        bool CanNav(int[] val)
        {
            char i = map[val[Y]][val[X]];
            return i == ' ' || i == '.' || i == '*';
        }

        bool IsPoint(int[] val)
        {
            char i = map[val[Y]][val[X]];
            return i == '.' || i == '*';
        }

        void SeekDot()
        {
            List<int[]> tmpList = new List<int[]>();
            List<int[]> tmpStack = new List<int[]>();

            // int [][] Aiseek;

            int[][][] pathmap;

            pathmap = new int[width][][];

            //  Aiseek = new int[width][];

            int i; //int j;
            for (i = 0; i < width; i++)
            {
                //   Aiseek[i] = new int[height];
                pathmap[i] = new int[height][];
            }

            int[] cord = null, last = null;

            tmpStack.Add(s1);
            bool found = false;

            while (true)
            {

                for (int x = 0; x < tmpStack.Count; x++)
                {

                    for (i = 0; i < 4; i++)
                    {

                        cord = (int[])tmpStack[x].Clone();

                        cord[X] += dx[i];
                        cord[Y] += dy[i];

                        if (InBoundry(cord) && CanNav(cord) && pathmap[cord[X]][cord[Y]] == null)
                        {
                            tmpList.Add(cord);
                            //Aiseek[cord[X]][cord[Y]] = 1;
                            pathmap[cord[X]][cord[Y]] = (int[])tmpStack[x].Clone();
                        }
                        else
                        {
                            continue;
                        }

                        found = IsPoint(cord);
                        if (found) break;


                    }
                    if (found) break;

                }


                if (found || tmpStack.Count == 0) break;
                tmpStack.Clear();
                tmpStack.AddRange(tmpList.ToArray());
                tmpList.Clear();



            }

            pathmap[s1[0]][s1[1]] = null;
            if (found)
            {


                while (true)
                {

                    if (cord != null && pathmap[cord[X]][cord[Y]] != null)
                    {
                        last = cord;
                        cord = pathmap[cord[X]][cord[Y]];
                    }
                    else
                    {
                        break;
                    }

                }

            }
            else
            {

            }

            map[s1[1]][s1[0]] = ' ';
            map[last[1]][last[0]] = 'A';

            s1[0] = last[0];
            s1[1] = last[1];
            last = null;
            // Aiseek = null;
            pathmap = null;
        }

    }
}
