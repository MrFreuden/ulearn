using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		var times = visits
			.OrderBy(visit => Tuple.Create(visit.UserId, visit.DateTime))
			.GroupBy(visit => visit.UserId)
			.SelectMany(group => group.Bigrams())
			.Where(bigram => bigram.First.SlideType.Equals(slideType))
			.Select(bigram => (bigram.Second.DateTime - bigram.First.DateTime).TotalMinutes)
			.Where(time => time >= 1 && time <= 120);

         return times.Any() ? times.Median() : 0;
	}
}