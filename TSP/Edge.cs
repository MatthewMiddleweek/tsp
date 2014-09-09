using System;

namespace TSP
{
    // Test comment
    public class Edge : IComparable<Edge>
    {
        int v;
        int w;
        float weight;

        public Edge(int v, int w, float weight)
        {
            if (v < 0) throw new IndexOutOfRangeException();
            if (w < 0) throw new IndexOutOfRangeException();
            if (Double.IsNaN(weight)) throw new ArgumentException();
            this.v = v;
            this.w = w;
            this.weight = weight;
        }

        public float Weight
        {
            get { return weight; }
        }

        public int Either
        {
            get { return v; }
        }

        public int Other(int vertex)
        {
            if (vertex == v) return w;
            else if (vertex == w) return v;
            else throw new ArgumentException("Illegal Endpoint");
        }

        public int CompareTo(Edge edge)
        {
            if (edge == null) throw new ArgumentException("null Edge comparison");
            return this.weight.CompareTo(edge.Weight);
        }

        public override string ToString()
        {
            return String.Format("{0}-{1} {2}", v, w, weight);
        }
    }
}
