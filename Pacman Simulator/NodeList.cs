using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class NodeList<T>
    {
        List<T> tmp;

        public NodeList()
        {
            tmp = new List<T>();
        }

        public void Push(T value)
        {
            tmp.Add(value);
        }
        public void Push(T[] value)
        {
            tmp.AddRange(value);
        }

        public T Pop()
        {
            T lst = tmp[0];
            tmp.RemoveAt(0);
            return lst;
        }

        public int Length()
        {
            return tmp.Count;
        }

        public void Clear()
        {
            tmp.Clear();
        }

    }
}
