// Coalesce.cs
//
// Simple example to show how nested loops can be 
// colesced.
//
// (C) Datasim Education BV 2012.
//

using System;

public class Coalesce
{
    public Coalesce()
    {
        int N = 3; int M = 4;

        int[,] mat = new int[N, M];

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                mat[i, j] = i + j;
            }
        }

        // Now coalesce
        int NM = N * M;

        int ii,jj;
        
        // Put parallel loop here (how?)
        for (int ij = 0; ij < NM; ij++)
        {
            ii = ij / M;
            jj = ij % M;
            Console.WriteLine("maj {0}, row= {1}, col= {2}", ij, ii, jj);

            mat[ii, jj] = ii + jj;
        }

    }
    static void Main()
        {

            Coalesce c = new Coalesce();
        }

}
