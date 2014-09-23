using System;

class Program
{
    static int[] X = new int[9];
    static int[] Y = new int[9];
    static int[,] BLK = new int[3, 3];
    static int[,] map = new int[9, 9];

    static int n_bits(int mask)
    {
        int c = 0;
        while (mask != 0)
        {
            c++;
            mask &= (mask - 1);
        }
        return c;
    }

    static bool Solve()
    {
        int min_ways = int.MaxValue;
        int y = -1, x = -1, m = 0;

        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
            {
                if (map[i, j] == -1)
                {
                    int mask = Y[i] & X[j] & BLK[i / 3, j / 3];
                    int ways = n_bits(mask);

                    if (ways < min_ways)
                    {
                        min_ways = ways;
                        y = i;
                        x = j;
                        m = mask;
                    }
                }
            }

        if (min_ways == int.MaxValue)
            return true;

        if (min_ways == 0)
            return false;

        for (int d = 1; d <= 9; d++)
        {
            int mask = 1 << (d - 1);
            if ((mask & m) != 0)
            {

                map[y, x] = d;
                Y[y] &= ~mask;
                X[x] &= ~mask;
                BLK[y / 3, x / 3] &= ~mask;

                if (Solve())
                    return true;

                map[y, x] = -1;
                Y[y] |= mask;
                X[x] |= mask;
                BLK[y / 3, x / 3] |= mask;
            }
        }

        return false;
    }

    static void Main(string[] args)
    {
        string[] sudoku =
        {
            "..7.5....",
            "..8..25.1",
            ".2...8.4.",
            "3..2.....",
            "67.....59",
            ".....7..2",
            ".6.3...9.",
            "9.16..7..",
            "....9.1.."
        };

        // set lowest 9 bits
        for (int i = 0; i < 9; i++)
            X[i] = Y[i] = BLK[i / 3, i % 3] = 0x1ff;

        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
            {
                if (sudoku[i][j] == '.')
                    map[i, j] = -1;
                else
                {
                    int d = (int)sudoku[i][j] - (int)'0';
                    map[i, j] = d;

                    int mask = 1 << (d - 1);

                    // clear corresponding bit
                    Y[i] &= ~mask;
                    X[j] &= ~mask;
                    BLK[i / 3, j / 3] &= ~mask;
                }
            }

        if (Solve())
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(map[i, j]);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("not solvable");
        }
    }
}
