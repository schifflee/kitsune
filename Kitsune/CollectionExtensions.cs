﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kitsune
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> After<T>(this IEnumerable<T> collection, int skip)
        {
            foreach (T x in collection)
            {
                if (skip == 0)
                    yield return x;
                else
                    skip--;
            }
        }
        public static List<int> PartitionRuns<T>(this IEnumerable<T> collection, Func<T, T, bool> equality)
        {
            List<int> ret = new List<int>();
            IEnumerator<T> e = collection.GetEnumerator();
            if(!e.MoveNext())
                return ret;
            T soFar = e.Current;
            int nSofar = 1;
            while (e.MoveNext())
            {
                T v2 = e.Current;
                if (equality(soFar, v2))
                {
                    nSofar++;
                }
                else
                {
                    ret.Add(nSofar);
                    nSofar = 0;
                    soFar = v2;
                }
            }
            ret.Add(nSofar);
            return ret;
        }
        public static List<List<T>> Partition<T>(this IEnumerable<T> collection, List<int> partitions)
        {
            IEnumerator<T> enume = collection.GetEnumerator();
            List<List<T>> ret = new List<List<T>>();
            foreach (int i in partitions)
            {
                List<T> part = new List<T>();
                for (int j = 0; j < i; ++j)
                {
                    enume.MoveNext();
                    part.Add(enume.Current);
                }
                ret.Add(part);
            }
            return ret;
        }
        public static void InsertSorted<T>(this List<T> list, T value, Comparison<T> comparer)
        {
            int i=0;
            while (i < list.Count)
            {
                if (comparer(value, list[i]) < 0)
                {
                    list.Insert(i, value);
                    return;
                }
                i++;
            }

            list.Add(value);
        }
        /*
         e.g:
         "if % % else %" => "if ", "%", " ", "%", " else ", " %"
         */
        public static string[] SplitFuncArgs(this string declaration)
        {
            List<string> ret = new List<string>();
            string run = "";
            bool inSpecial = false;
            foreach (char c in declaration)
            {
                if (c == '%')
                {
                    if (!(run == ""))
                        ret.Add(run);
                    run = "";
                    ret.Add("%");
                }
                else if (c == '_')
                {
                    if (!inSpecial)
                    {
                        if (!(run == ""))
                            ret.Add(run);
                        run = "_";
                        inSpecial = true;
                    }
                    else
                    {
                        ret.Add(run);
                        run = "";
                        inSpecial = false;
                    }
                }
                else
                {
                    run += c.ToString();
                }
            }
            if (!(run == ""))
                ret.Add(run);
            return ret.ToArray();
        }
    }
}
