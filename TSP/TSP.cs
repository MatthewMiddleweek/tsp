using System;
using System.IO;

/*
 * Usage:
 * TSP.exe datafile.txt
 * 
 * Output:
 * Length of shortest round trip tour, visiting
 * every city precisely once, followed by the number
 * of seconds elapsed.
 * 
 */

namespace TSP
{
    // A dynamic programming approach to the TSP problem where problem size
    // is defined by the number of cities (verticies) involved in a tour.
    // The algorithm uses an objective function which determines the shortest
    // path from an arbitrary, fixed source city for a given problem size and 
    // each destination city, based on the solutions for the immediately smaller 
    // problem size, culminating in the solution to the original problem.
    // Input is a text file headed by the number of cities, with subsequent
    // lines giving the 2D coordinates of those cities.

    class TSP
    {
        int V;              // the number of vertices
        int setSize;        // the square of the number of vertices
        float[,] edges;     // the square array of edges
        float[][,] A;       // the dynamic programming memoised results array
        int[] S;            // index of set position within sets array
        int[][] sets;       // jagged array of sets by cardinality
                            // (subproblems are ordered by increasing cardinality)
        int[] cardCount;    // a count of the cardinality of each set

        // constructor takes an adjacency matrix graph as its input
        // while the main method below takes a TSP problem
        // and translates it into an adjacency matrix graph
        public TSP(AdjMatrixEdgeWeightedGraph G)
        {
            this.V = G.V;
            this.edges = G.edgeArr;
            this.setSize = (int)Math.Pow(2, V - 1);
            S = new int[setSize];
            sets = new int[V - 1][];
            cardCount = new int[V - 1];
            A = new float[2][,];

            // create 2nd dimensions of sets jagged array
            int c;
            for (int i = 0; i < V - 1; i++)
            {
                c = Combinations(V - 1, i + 1);
                sets[i] = new int[c];
            }

            // populate S, sets and cardCount arrays
            // cardCount maintains a running count during this routine
            byte t;
            for (int i = 1; i < setSize; i++)
            {
                t = Cardinality(i);
                S[i] = cardCount[t - 1];
                sets[t - 1][cardCount[t - 1]++] = i;
            }
        }

        public float Dynamic()
        {

            // create A subarrays, one for each subproblem size / cardinality
            // A is memoised so only two sets of subproblems, past and present, are maintained
            // the subarrays are 2D, storing the number of possible combinations of subproblems
            // by the number of possible destinations
            int c;
            for (int i = 0; i < 2; i++)
            {
                c = Combinations(V - 1, i + 1);
                A[i] = new float[c, i + 1];
            }

            int n, x, countj, countk;
            float z = 0;

            // initialise A with distances from source city to each destination city
            for (int j = 0; j < cardCount[0]; j++)
                A[0][j, 0] = edges[0, j + 1];

            // main loop iterates through increasing subproblem sizes
            for (int m = 1; m < V - 1; m++)
            {
                // second loop iterates through each subproblem set
                for (int s = 0; s < cardCount[m]; s++)
                {
                    x = sets[m][s];
                    countj = 0;

                    // third loop iterates through each destination
                    for (int j = 0; j < V - 1; j++)
                        if (((x >> j) & 1) == 1) // only continue if j is a member of x
                        {
                            countk = 0;
                            z = float.PositiveInfinity;
                            for (int k = 0; k < V - 1; k++)
                                if ((((x >> k) & 1) == 1) && j != k) // only continue if k is
                                {                                    // a member of x and is not j
                                    // n is the set x excluding j
                                    n = S[x - (1 << j)];
                                    // z is the previous subproblem size's shortest path length to j via k
                                    z = Math.Min(z, A[(m - 1) % 2][n, countk++] + edges[k + 1, j + 1]);
                                }
                            A[m % 2][s, countj++] = z; // set the shortest path length for the current
                        }                              // subproblem size and destination
                }

                // after an iteration of subproblem sizes has completed
                // replace the previous 2D array of subproblem solutions
                // with a new array of appropriate size
                c = Combinations(V - 1, m + 2);
                A[(m + 1) % 2] = null;
                A[(m + 1) % 2] = new float[c, m + 2];
            }

            // having computed shortest paths to each destination, visiting every other city precisely once
            // determine the shortest round-trip tour by adding a trip back to the source city
            z = float.PositiveInfinity;
            for (int j = 0; j < V - 1; j++)
            {
                z = Math.Min(z, A[(V - 2) % 2][0, j] + edges[j + 1, 0]);
            }

            return z;

        }

        // determine the number of 1s in the binary representation of the input
        byte Cardinality(int i)
        {
            byte count = 0;
            while (i > 0)
            {
                count += (byte)(i & 1);
                i >>= 1;
            }
            return count;
        }

        // compute the number of possible combinations, n choose k
        // minimising the potential for arithmetical overflow
        public int Combinations(int n, int k)
        {
            long r = 1;
            for (int i = 0; i < k; i++)
            {
                r *= (n - i);
                r /= (i + 1);
            }
            return (int)r;
        }

        static void Main(string[] args)
        {
            DateTime start;
            TimeSpan time;
            start = DateTime.UtcNow;

            StreamReader sr = new StreamReader(args[0]);
            int V = int.Parse(sr.ReadLine());
            float[,] points = new float[V, 2];
            string[] line = new string[2];
            for (int i = 0; i < V; i++)
            {
                line = sr.ReadLine().Split(' ');
                points[i, 0] = float.Parse(line[0]);
                points[i, 1] = float.Parse(line[1]);
            }

            AdjMatrixEdgeWeightedGraph G = new AdjMatrixEdgeWeightedGraph(points);
            TSP t = new TSP(G);
            float ans = t.Dynamic();
            Console.WriteLine(ans);

            time = DateTime.UtcNow - start;
            Console.WriteLine(time.TotalMilliseconds);
        }

    }

}