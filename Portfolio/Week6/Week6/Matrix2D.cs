using System;
namespace Week6;

public class Matrix2D
{
    private int[,] matrix;

    public Matrix2D()
    {
        matrix = new int[0, 0];
    }

    public Matrix2D(int rows, int cols)
    {
        matrix = new int[rows, cols];
    }

    public Matrix2D(int[,] input)
    {
        matrix = input;
    }

    public int[,] GetMatrix()
    {
        return matrix;
    }

    public int NumberOfRows()
    {
        return matrix.GetLength(0);
    }

    public int NumberOfColumns()
    {
        return matrix.GetLength(1);
    }

    public void SetMatrix(int[,] input)
    {
        matrix = input;
    }

    public string DisplayMatrix()
    {
        string output = "";

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                output += matrix[i, j] + "\t";
            }
            output += Environment.NewLine;
        }

        return output;
    }
}