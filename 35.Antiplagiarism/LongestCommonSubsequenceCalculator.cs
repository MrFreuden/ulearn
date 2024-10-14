using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{ 
    public static List<string> Calculate(List<string> first, List<string> second)
	{
		var opt = CreateOptimizationTable(first, second);
		return RestoreAnswer(opt, first, second);
	}

	private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
	{
        var opt = new int[first.Count + 1, second.Count + 1];
        for (var i = 1; i <= first.Count; ++i)
            for (var j = 1; j <= second.Count; ++j)
            {
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = 1 + opt[i - 1, j - 1] ;
                else
                    opt[i, j] = Math.Max(opt[i - 1, j], opt[i, j - 1]);
            }

        return opt;
	}

	private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
	{
        var lcs = new Stack<string>();
        var l1 = first.Count;
        var l2 = second.Count;

        while (l1 > 0 && l2 > 0)
        {
            if (first[l1 - 1] == second[l2 - 1])
            {
                lcs.Push(first[l1 - 1]);
                l1--;
                l2--;
            }
            else
            {
                if (opt[l1 - 1, l2] > opt[l1, l2 - 1])
                    l1--;
                else
                    l2--;
            }
        }

        return lcs.ToList();
    }
}