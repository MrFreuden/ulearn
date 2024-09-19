using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		var s = visits
			//.Where(visit => visit.SlideType == slideType)
			.OrderBy(visit => Tuple.Create(visit.UserId, visit.DateTime))
			.GroupBy(x => x, x => x.DateTime)
			
			//.Select(x => x.Bigrams())
			
			//.SelectMany(x => x)
			//.Select(x => (x.Second - x.First).TotalMinutes)
			//.Where(x => x > 1 && x < 120)
			
            ;

		return 0.0;
        // return s.Any() ? s.Median() : 0;
	}
}