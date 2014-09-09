using System;
using System.Collections;
using System.IO;
using System.Text;

namespace TSP
{
    class AdjMatrixEdgeWeightedGraph
    {
        public int V;
        public int E;
        public Edge[,] adj;
        public float[,] edgeArr;

        public AdjMatrixEdgeWeightedGraph(int V)
        {
            if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
            this.V = V;
            this.E = 0;
            this.adj = new Edge[V, V];
        }

        public AdjMatrixEdgeWeightedGraph(string arg)
        {
            StreamReader sr = new StreamReader(arg);
            string[] initial = sr.ReadLine().Split(' ');
            this.V = int.Parse(initial[0]);
            this.E = 0;
            if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
            this.adj = new Edge[V, V];
            int E = int.Parse(initial[1]);
            Edge e;
            string[] line;
            for (int i = 0; i < V; i++)
            {
                line = sr.ReadLine().Split(' ');
                e = new Edge(int.Parse(line[0]) - 1, int.Parse(line[1]) - 1, float.Parse(line[2]));
                addEdge(e);
            }
        }

        public AdjMatrixEdgeWeightedGraph(int[,] points)
        {
            this.V = points.GetLength(0);
            this.E = 0;
            if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
            this.adj = new Edge[V, V];
            this.edgeArr = new float[V, V];
            Edge e;
            float w;
            for (int i = 0; i < V; i++)
                for (int j = 0; j < V; j++)
                {
                    w = (float)Math.Sqrt(Math.Pow(points[i, 0] - points[j, 0], 2) + Math.Pow(points[i, 1] - points[j, 1], 2));
                    e = new Edge(i, j, (float)Math.Round(w, 0));
                    addEdge(e);
                    edgeArr[i, j] = w;
                    edgeArr[j, i] = w;
                }
        }

        public AdjMatrixEdgeWeightedGraph(float[,] points)
        {
            this.V = points.GetLength(0);
            this.E = 0;
            if (V < 0) throw new ArgumentException("Number of vertices must be nonnegative");
            this.adj = new Edge[V, V];
            this.edgeArr = new float[V, V];
            Edge e;
            float w;
            for (int i = 0; i < V; i++)
                for (int j = 0; j < V; j++)
                {
                    w = (float)Math.Sqrt(Math.Pow(points[i, 0] - points[j, 0], 2) + Math.Pow(points[i, 1] - points[j, 1], 2));
                    e = new Edge(i, j, (float)Math.Round(w, 0));
                    addEdge(e);
                    edgeArr[i, j] = w;
                    edgeArr[j, i] = w;
                }
        }

        public void addEdge(Edge e)
        {
            int v = e.Either;
            int w = e.Other(v);
            if (v < 0 || v >= V) throw new IndexOutOfRangeException("vertex " + v + " is not between 0 " + (V - 1));
            if (w < 0 || w >= V) throw new IndexOutOfRangeException("vertex " + w + " is not between 0 " + (V - 1));
            if (adj[v, w] == null)
            {
                adj[v, w] = e;
                E++;
            }
        }

        public IEnumerable edges(int v)
        {
            if (v < 0 || v > +V) throw new IndexOutOfRangeException("vertex " + v + " is not between 0 " + (V - 1));
            for (int i = 0; i < V; i++)
                yield return adj[v, i];
        }

        public IEnumerable allEdges()
        {
            foreach (Edge e in adj)
                yield return e;
        }

        public override string ToString()
        {
            string NEWLINE = System.Environment.NewLine;
            StringBuilder s = new StringBuilder();
            s.Append(V + " " + E + NEWLINE);
            for (int v = 0; v < V; v++)
            {
                s.Append(v + ": ");
                foreach (Edge e in edges(v))
                    s.Append(e + " " + NEWLINE);
                s.Append(NEWLINE);
            }
            return s.ToString();
        }
    }
}
