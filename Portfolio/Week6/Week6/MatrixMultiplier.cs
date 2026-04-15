using System;

namespace Week6;

public class MatrixMultiplier
{
    public static Matrix2D multiplies(Matrix2D a, Matrix2D b)
    {
        int[,] matrixA = a.GetMatrix();
        int[,] matrixB = b.GetMatrix();

        int rowsA = a.NumberOfRows();
        int colsA = a.NumberOfColumns();
        int rowsB = b.NumberOfRows();
        int colsB = b.NumberOfColumns();

        if (colsA != rowsB)
        {
            throw new ArgumentException("Matrix dimensions are not valid for multiplication.");
        }

        int[,] result = new int[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                result[i, j] = 0;

                for (int k = 0; k < colsA; k++)
                {
                    result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        }

        return new Matrix2D(result);
    }
}