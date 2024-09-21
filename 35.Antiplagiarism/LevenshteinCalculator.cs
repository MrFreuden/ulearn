using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
	public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
	{
		return new List<ComparisonResult> {
			new(
				documents[0], 
				documents[1], 
				TokenDistanceCalculator.GetTokenDistance(documents[0][0], documents[1][0]))};
	}
}