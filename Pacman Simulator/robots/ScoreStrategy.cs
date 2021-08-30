using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman_Simulator.robots
{
    class ScoreStrategy:IAI
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

        const int dotScore = 1, bonusScore = 10;

        const char wall = '#', pill = '!', bonus = '*', dot = '.', empty = ' ';
        string stor = "";

        char[][] map;
      //  int[][] scoremap;

        int[][][] path;
        // player cords
        int[] s1, s2;

        void Main(string[] args)
        {
            GetMap(args);
            SetupVar();
            FindPlayers();
            
            path = EndSeek(StartSeek(s1), StartSeek(s2));
            //path = MapDeduct(StartSeek(s1), StartDepthSeek(s2, 1));
            CalculateBestNode();
            Plan(0);
            Move();
            SaveMap();
        }

        void SetupVar()
        {
            s1 = new int[2];
            s2 = new int[2];

            dx = new int[4];
            dy = new int[4];


            dx[0] = 1;    dx[1] = 0;    dx[2] = -1;   dx[3] = 0;
            dy[0] = 0;    dy[1] = -1;    dy[2] = 0;    dy[3] = 1;
            

        }

        void GetMap(string[] file)
        {
            string[] tmp = file;
            height = tmp.Length;
            width = tmp[0].Length;

            map = new char[tmp.Length][];
            for (int i = 0; i < height; i++) map[i] = tmp[i].ToCharArray();

        }

        void FindPlayers()
        {
            int pos = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (map[y][x])
                    {
                        case 'A':
                            s1[X] = x;
                            s1[Y] = y;
                            pos++;
                            break;
                        case 'B':
                            s2[X] = x;
                            s2[Y] = y;
                            pos++;
                            break;
                    }
                    if (pos == 2) return;
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


        // logical start
        List<int[]> Food;

        void GetNearestFood()
        {
            int i;
            i = 0;

            int[] cord;
            int[] parent = new int[] { s1[0], s1[1], 0, 0, -1, -1 };
            Food = new List<int[]>();

            while (i < 4)
            {
                cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]), 1, 0, parent[0], parent[1] };

                if (CanNavigate(cord) && IsDot(cord))
                {
                    Food.Add(cord);
                }

                i++;
            }
        }

        // logical seekers

        int[][][] StartSeek(int[] pos)
        {
            int i=0;
            int[][][] SeekMap;
            SeekMap = new int[width][][];
            for (i = 0; i < width; i++) SeekMap[i] = new int[height][];

            NodeList<int[]> stack = new NodeList<int[]>();
            List<int[]> list = new List<int[]>();

            int[] cord;
            int[] parent = new int[] { pos[0], pos[1], 0,0,-1,-1 };
            int step = 1;

            stack.Push(parent);


            SeekMap[parent[0]][parent[1]] = parent;
            while (true)
            {

                while (stack.Length() != 0)
                {
                    parent = stack.Pop();
                    
                    i = 0;
                    while (i < 4)
                    {
                        cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]),step,0,parent[0],parent[1] };

                        if(CanNavigate(cord) && SeekMap[cord[0]][cord[1]]==null)
                        {


                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0]=9;
                                cord[1] = 10;
                                
                            }
                            cord[3] = parent[3] + GetSpotScore(map[cord[Y]][cord[X]]);
                                list.Add(cord);
                                SeekMap[cord[0]][cord[1]] = cord;
                        }

                        i++;
                    }

                    step++;
                }

                if (list.Count == 0) break;

                stack.Push(list.ToArray());
                list.Clear();
            }

            return SeekMap;
        }

        int[][][] EndSeek(int[][][] main, int[][][] secondary)
        {
            int[][][] SeekMap;
            int i;
            SeekMap = new int[width][][];
            for (i = 0; i < width; i++) SeekMap[i] = new int[height][];
            Food = new List<int[]>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    if (main[x][y] != null)
                    {

                        if (secondary[x][y][2]-3 > main[x][y][2])
                        {
                            SeekMap[x][y] = main[x][y];

                            if (IsDot(main[x][y]))
                            {
                                Food.Add(main[x][y]);
                            }
                        }

                    }
                }
            }

            return SeekMap;
        }

        int[][][] StartDepthSeek(int[] pos, int depth)
        {

            int i = 0;
            int[][][] SeekMap;
            SeekMap = new int[width][][];
            for (i = 0; i < width; i++) SeekMap[i] = new int[height][];

            NodeList<int[]> stack = new NodeList<int[]>();
            List<int[]> list = new List<int[]>();

            int[] cord;
            int[] parent = new int[] { pos[0], pos[1], 0, 0, -1, -1 };
            int step = 1;

            stack.Push(parent);


            SeekMap[parent[0]][parent[1]] = parent;
            int count = 0;

            while (count<depth)
            {

                while (stack.Length() != 0)
                {
                    parent = stack.Pop();

                    i = 0;
                    while (i < 4)
                    {
                        cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]), step, 0, parent[0], parent[1] };

                        if (CanNavigate(cord) && SeekMap[cord[0]][cord[1]] == null)
                        {


                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;

                            }
                            cord[3] = parent[3] + GetSpotScore(map[cord[Y]][cord[X]]);
                            list.Add(cord);
                            SeekMap[cord[0]][cord[1]] = cord;
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

            return SeekMap;
         //   return null;
        }

        int[][][] MapDeduct(int[][][] main, int[][][] secondary)
        {
            int[][][] SeekMap;
            int i;
            SeekMap = new int[width][][];
            for (i = 0; i < width; i++) SeekMap[i] = new int[height][];
            Food = new List<int[]>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    if (main[x][y] != null)
                    {

                        if (secondary[x][y]==null)
                        {
                            SeekMap[x][y] = main[x][y]; 
                            if (IsDot(main[x][y]))
                            {
                                Food.Add(main[x][y]);
                            }
                        }

                    }
                }
            }

            return SeekMap;
        }

        bool CanNavigate(int[] pos)
        {

            if (pos[X] > -1 && pos[X] < width && pos[Y] > -1
                && pos[Y] < height
                && map[pos[Y]][pos[X]] != wall)
                return true;

            return false;
        }

        public int GetSpotScore(char r)
        {
            if (r == '.') return dotScore;
            if (r == '*') return bonusScore;
            return 0;
        }

        public bool IsDot(int[] pos)
        {
            char r = map[pos[Y]][pos[X]];
            return (r == '.' || r == '*');
        }

        int[] k;

        public void CalculateBestNode()
        {
            k=new int[]{0,0,0,0,0,0};

            int i = 0;
            int[][][] SeekMap;
            SeekMap = new int[width][][];
            for (i = 0; i < width; i++) SeekMap[i] = new int[height][];

            NodeList<int[]> stack = new NodeList<int[]>();
            List<int[]> list = new List<int[]>();

            int[] cord;
            int[] parent = new int[] { s1[0], s1[1], 0, 0, -1, -1 };
            int step = 1;

            stack.Push(parent);
            SeekMap[parent[0]][parent[1]] = parent;
            while (true)
            {

                while (stack.Length() != 0)
                {
                    parent = stack.Pop();

                    i = 0;
                    while (i < 4)
                    {
                        cord = new int[] { (parent[0] + dx[i]), (parent[1] + dy[i]), step, 0, parent[0], parent[1] };

                        if (CanNavigate(cord)&&  PathOpen(cord) && SeekMap[cord[0]][cord[1]] == null)
                        {


                            if (map[cord[Y]][cord[X]] == '!')
                            {
                                cord[0] = 9;
                                cord[1] = 10;

                            }
                            cord[3] = parent[3] + GetSpotScore(map[cord[Y]][cord[X]]);
                            list.Add(cord);
                            SeekMap[cord[0]][cord[1]] = cord;
                            k = cord;
                        }

                        i++;
                    }

                    
                }

                if (list.Count == 0) break;
                EvaluateNodes(list);

                if (BestNodeFound(list))
                {
                    list.Clear();
                    return;
                }
                stack.Push(list.ToArray());
                list.Clear();
                step++;
            }


           // ChooseFurtherNode(); 
        }
        
        public void ChooseFurtherNode()
        {

            if (k[2] == 0)
            {
                SeekDot(); // just blindly find any dot
            }
            else
            {
              //  List<int[]> bfood = new List<int[]>();

               // int dis=0;

                int[] d2;

               // d1= new int[]{s1[0],s1[1]};
                d2=new int[]{s2[0],s2[1]};

                BinaryHeap h= new BinaryHeap(3);
                for (int x = 0; x < Food.Count; x++)
                {
                    if (Food[x][2] == k[2])
                    {
                        int[] n = (int[])Food[x].Clone();

                        n[3] = distance(d2, Food[x]);
                        h.Push(n);

                    }
                }
                k = h.Pop();
                h = null;
            }
        }

        public int distance(int[] p1, int[] p2)
        {

            int x = p1[0] - p2[0];
            int y = p1[1] - p2[1];


            if (x < 0)
            {
                x *= -1;
            }

            if (y < 0)
            {
                y *= -1;
            }

            return x + y;
        }

        bool PathOpen(int[] node)
        {
            return path[node[X]][node[Y]] != null;
        }

        bool BestNodeFound(List<int[]> nodes)
        {

            if (nodes.Count == 1) return false;
            BinaryHeap h = new BinaryHeap(3);

            h.Push(nodes.ToArray());

            int[] p1, p2;

            p1 = h.Pop();
            p2 = h.Pop();
            h = null;

            //while(h.Size()!=0 &&

            if (p1[3] > p2[3])
            {
                k = p1;
            }
            else return false;

            return true;
        }

        void EvaluateNodes(List<int[]> nodes)
        {
            /*
            if (nodes.Count == 1) return;
            BinaryHeap h = new BinaryHeap();
            h.Push(nodes.ToArray());
            */


        }

        void Plan(int mode)
        {
            int []last=null;

            try
            {
                while (k[4] != -1 && k[5] != -1)
                {
                    last = k;
                    k = path[k[4]][k[5]];
                }
            }
            catch
            {

                if (mode == 0)
                {
                    try
                    {
                        path = EndSeek(StartSeek(s1), StartSeek(s2));
                        CalculateBestNode();
                        Plan(mode + 1);
                    }
                    catch
                    {

                        if (k == null)
                        {
                            throw new Exception("Null Pointer");
                        }
                    }
                }
                else
                {
                    return;
                }

                if (k == null)
                {
                    throw new Exception("Null Pointer");
                }
                path = null;
                return;
            }
            path = null;
            if (last == null) throw new Exception("Null Pointer");
            if (k == null) throw new Exception("Null Pointer");
            k = last;

        }

        void Move()
        {
            map[s1[1]][s1[0]] = ' ';
            map[k[1]][k[0]] = 'A';
            //s1 = k;

            s1[0] = (int)k[0];
            s1[1] = (int)k[1];
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
                        //tmpStack.
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
    }
}
