using System.Globalization;

namespace Delegates.Reports;

public class MeanAndStd
{
	public double Mean { get; set; }
	public double Std { get; set; }
	public override string ToString()
	{
		return Mean.ToString(CultureInfo.InvariantCulture) + "±" + Std.ToString(CultureInfo.InvariantCulture);
	}
}