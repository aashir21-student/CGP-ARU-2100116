using System;
namespace Week6;

public class Testing
{
    public static void Main()
    {
        int[,] aValues = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };

        int[,] bValues = new int[,]
        {
            { 7, 8 },
            { 9, 10 },
            { 11, 12 }
        };

        Matrix2D matrixA = new Matrix2D(aValues);
        Matrix2D matrixB = new Matrix2D(bValues);

        Console.WriteLine("Matrix A:");
        Console.WriteLine(matrixA.DisplayMatrix());

        Console.WriteLine("Matrix B:");
        Console.WriteLine(matrixB.DisplayMatrix());

        Matrix2D result = MatrixMultiplier.multiplies(matrixA, matrixB);

        Console.WriteLine("A x B:");
        Console.WriteLine(result.DisplayMatrix());

        Console.ReadLine();
    }
}