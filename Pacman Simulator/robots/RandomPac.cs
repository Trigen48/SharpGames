using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman_Simulator.robots
{
    class RandomPac: IAI
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

        const char wall = '#', pill = '!', bonus = '*', dot = '.', empty = ' ';
        string stor = "";

        char[][] map;

        int[] s1, s2;

        void Main(string[] args)
        {

            GetMap(args);
            SetupVar();
            BuildScoreMap();
            MakeMove();
            SaveMap();
        }

        void SetupVar()
        {
            s1 = new int[2];
            s2 = new int[2];

            dx = new int[4];
            dy = new int[4];


            dx[0] = 0;    dx[1] = 0;    dx[2] = -1;   dx[3] = 1;
           
             dy[0] = -1;     dy[1] = 1;    dy[2] = 0;    dy[3] = 0;

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
            for (int y = 0; y < height; y++)
            {
 
                for (int x = 0; x < width; x++)
                {
                    switch (map[y][x])
                    {

                        case 'A':
                            s1[X] = x;
                            s1[Y] = y;
                            break;
                        case 'B':
                            s2[X] = x;
                            s2[Y] = y;
                            break;

                    }
                }
            }
        }
        void SaveMap()
        {
            int range = height-1 ;
            System.IO.StreamWriter rw = new System.IO.StreamWriter(stor + "game.state");

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
            return i!='#';
        }

        bool IsPoint(int[] val)
        {
            char i = map[val[Y]][val[X]];
            return i == '.' || i == '*';
        }

        void MakeMove()
        {
            List<int[]> moves= new List<int[]>();
            int[] t;

            int i;
            for (i = 0; i < 4; i++)
            {
                t = new int[2];

                t[0] = (int)(s1[0]+dx[i]);
                t[1] = (int)(s1[1] + dy[i]);

                if (InBoundry(t) && CanNav(t))moves.Add(t);

            }
                Random r = new Random();
                i = (int)r.Next(0, moves.Count);

                s1[0] = moves[i][0];
                s1[1] = moves[i][1];

        }
    }
}
