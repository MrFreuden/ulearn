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
			
			//.ToList()
            ;

		return 0.0;
	}
}