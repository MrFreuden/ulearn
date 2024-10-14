using System;
using System.Collections.Generic;
using System.Linq;

using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        if (documents.Count <= 1) return new List<ComparisonResult>();
        
        var documentsPairs = documents.SelectMany(
            (fst, i) => documents.Skip(i + 1).Select(snd => (fst, snd)));
        var comparisonResults = documentsPairs.Select(
            x => new ComparisonResult(x.fst, x.snd, LevenshteinDistance(x.fst, x.snd))).ToList();

        return comparisonResults;
    }

    public static double LevenshteinDistance(DocumentTokens first, DocumentTokens second)
    {
        var opt = new double[first.Count + 1, second.Count + 1];
        for (var i = 0; i <= first.Count; ++i) opt[i, 0] = i;
        for (var i = 0; i <= second.Count; ++i) opt[0, i] = i;

        for (var i = 1; i <= first.Count; ++i)
            for (var j = 1; j <= second.Count; ++j)
            {
                opt[i, j] = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                opt[i, j] = Math.Min(opt[i - 1, j] + 1, Math.Min(opt[i, j - 1] + 1, opt[i - 1, j - 1] + opt[i, j]));
            }
        return opt[first.Count, second.Count];
    }
}