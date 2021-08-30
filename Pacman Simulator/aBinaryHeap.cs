using System;
using System.Collections.Generic;
using System.Text;

namespace System
{

    public class BinaryHeap
    {

        List<int[]> m_nodes;

        int sortid;
        public BinaryHeap(int  Sortid)
        {
            m_nodes = new List<int[]>();
            sortid = Sortid;
        }

        public void Push(int[] item)
        {
            m_nodes.Add((int[])item.Clone());
            sinkDown(m_nodes.Count - 1);
        }

        public void Push(int[][] items)
        {

            for (int i = 0; i < items.Length; i++)
            {
                Push(items[i]);
            }
        }

        public int[] Pop()
        {
            int[] n = m_nodes[0];

            m_nodes.RemoveAt(0);
            //int[] l = m_nodes[m_nodes.Count - 1];

            // m_nodes.RemoveAt(m_nodes.Count - 1);

            /*    if (this.m_nodes.Count > 0)
                {
                    m_nodes[0]=l;
                    BubbleUp(0);
                }*/

            return n;
        }

        public int Size()
        {
            return m_nodes.Count;
        }

        private void sinkDown(int index)
        {
            int id = index;
            int[] n = m_nodes[index];

            while (id > 0)
            {
                int pID = (int)(Math.Floor((double)(((double)id + (double)1) / (double)2) - 1));
                int[] pn = m_nodes[pID];

                if (n[sortid] > pn[sortid])
                {
                    m_nodes[pID] = n;
                    m_nodes[id] = pn;
                    id = pID;
                }
                else
                {
                    break;
                }
            }
        }

        private void BubbleUp(int index)
        {
            int i = index;
            int len = m_nodes.Count;
            int[] n = m_nodes[i];

            while (true)
            {
                int c1, c2;
                int swapIdx = -1;
                int s1 = 0, s2 = 0;

                c2 = (i + 1) * 2;
                c1 = c2 - 1;

                if (c1 < len)
                {
                    s1 = m_nodes[c1][3];
                    if (s1 < n[3]) swapIdx = c1;
                }


                if (c2 < len)
                {
                    s2 = m_nodes[c2][3];
                    int low = (swapIdx == -1 ? n[3] : s1);
                    if (s2 < low) swapIdx = c2;
                }


                if (swapIdx != -1)
                {
                    m_nodes[i] = m_nodes[swapIdx];
                    m_nodes[swapIdx] = n;
                    i = swapIdx;
                }
                else break;



            }
        }

        public int[][] GetArray()
        {
            return m_nodes.ToArray();
        }

    }

}
