using System;
using System.Linq;

namespace GaussAlgorithm;

public class Solver
{
    private const double Epsilon = 1e-3;

    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        int n = matrix.Length;
        int m = matrix[0].Length;

        if (n != freeMembers.Length)
        {
            throw new ArgumentException("���������� ����� ������� ������ ��������� � ������ ������� ��������� ������.");
        }

        // ������ ��� ������ ������
        for (int i = 0; i < Math.Min(n, m); i++)
        {
            int pivotRow = FindPivotRow(matrix, i, n);

            if (pivotRow == -1)
            {
                continue; // ���������� �������, ���� ��� �������� ������ � ����
            }

            // ������ ������� ������
            if (pivotRow != i)
            {
                SwapRows(matrix, freeMembers, i, pivotRow);
            }

            // ������������ ������
            NormalizeRow(matrix[i], freeMembers, i, m);

            // ��������� ��������� � �������
            EliminateColumn(matrix, freeMembers, i, n, m);
        }

        // �������� ������������� ������� � ��������� ������ �������
        for (int i = 0; i < n; i++)
        {
            bool allZero = true;
            for (int j = 0; j < m; j++)
            {
                if (Math.Abs(matrix[i][j]) > Epsilon)
                {
                    allZero = false;
                    break;
                }
            }
            if (allZero && Math.Abs(freeMembers[i]) > Epsilon)
            {
                throw new NoSolutionException("������� ��������� �����������.");
            }
        }

        // �������� ���
        double[] solution = new double[m];
        for (int i = Math.Min(n, m) - 1; i >= 0; i--)
        {
            if (Math.Abs(matrix[i][i]) < Epsilon)
            {
                solution[i] = 0; // ���� ������� ������� ����� ����, ������� ��� ���� ���������� ����� ����
            }
            else
            {
                solution[i] = freeMembers[i] / matrix[i][i];
            }

            for (int j = i - 1; j >= 0; j--)
            {
                freeMembers[j] -= matrix[j][i] * solution[i];
            }
        }

        return solution;
    }

    private int FindPivotRow(double[][] matrix, int col, int n)
    {
        if (col >= matrix.Length || col < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(col), "Column index is out of range.");
        }

        int pivotRow = -1;
        double maxValue = Epsilon;

        for (int i = col; i < n; i++)
        {
            if (Math.Abs(matrix[i][col]) > maxValue)
            {
                maxValue = Math.Abs(matrix[i][col]);
                pivotRow = i;
            }
        }

        return pivotRow;
    }

    private void SwapRows(double[][] matrix, double[] freeMembers, int row1, int row2)
    {
        var tempRow = matrix[row1];
        matrix[row1] = matrix[row2];
        matrix[row2] = tempRow;

        var tempValue = freeMembers[row1];
        freeMembers[row1] = freeMembers[row2];
        freeMembers[row2] = tempValue;
    }

    private void NormalizeRow(double[] row, double[] freeMembers, int rowIndex, int m)
    {
        double divisor = row[rowIndex];
        if (Math.Abs(divisor) > Epsilon)
        {
            for (int j = rowIndex; j < m; j++)
            {
                row[j] /= divisor;
            }
            freeMembers[rowIndex] /= divisor;
        }
    }

    private void EliminateColumn(double[][] matrix, double[] freeMembers, int col, int n, int m)
    {
        for (int i = 0; i < n; i++)
        {
            if (i == col) continue;
            double factor = matrix[i][col];
            for (int j = col; j < m; j++)
            {
                matrix[i][j] -= factor * matrix[col][j];
                if (Math.Abs(matrix[i][j]) < Epsilon) matrix[i][j] = 0;
            }
            freeMembers[i] -= factor * freeMembers[col];
            if (Math.Abs(freeMembers[i]) < Epsilon) freeMembers[i] = 0;
        }
    }
}
