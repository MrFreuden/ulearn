using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		var s = visits
			.Where(visit => visit.SlideType == slideType)
			.OrderBy(visit => Tuple.Create(visit.UserId, visit.DateTime))
			.GroupBy(x => x.UserId, x => x.DateTime)
			.Select(x => x.Bigrams())
			.SelectMany(x => x)
			
            ;
		var q = s.Select(x => x.Second - x.First);

        return 0.0;
	}
}