using Newtonsoft.Json.Linq;
using System;

namespace Incapsulation.Failures;

public class ReportMaker
{
	/// <summary>
	/// </summary>
	/// <param name="day"></param>
	/// <param name="failureTypes">
	/// 0 for unexpected shutdown, 
	/// 1 for short non-responding, 
	/// 2 for hardware failures, 
	/// 3 for connection problems
	/// </param>
	/// <param name="deviceId"></param>
	/// <param name="times"></param>
	/// <param name="devices"></param>
	/// <returns></returns>
	public static List<string> FindDevicesFailedBeforeDateObsolete(
		int day,
		int month,
		int year,
		int[] failureTypes, 
		int[] deviceId, 
		object[][] times,
		List<Dictionary<string, object>> devices)
	{
		if (!IsArraysLengthSame(failureTypes.Length, deviceId.Length, times.Length, devices.Count)) 
			throw new ArgumentException("Массивы имеют разные длины.");

        var requestTime = new DateTime(year, month, day);
        var timesAsObject = MakeTimes(times);

        var devicesList = new List<Device>();
        for (int i = 0; i < deviceId.Length; i++)
		{
			var dic = devices[i];
            var id = dic["DeviceId"];
            var name = dic["Name"];

            if (id == null || name == null) throw new ArgumentException();

			var fail = new Failure((FailureType)failureTypes[i], timesAsObject[i]);
			devicesList.Add(new Device((string)name, (int)id, fail));
        }

		return FindDevicesFailedBeforeDate(requestTime, devicesList);
	}

	private static bool IsArraysLengthSame(params int[] lengths)
	{
        var baseLength = lengths[0];

		foreach (var length in lengths)
		{
			if (length != baseLength)
			{
				return false;
            }
		}

		return true;
    }

    private static List<DateTime> MakeTimes(object[][] times)
    {
        return times
            .Select(x =>
			{
				DateTime time;
				try
				{
					time = new DateTime((int)x[2], (int)x[1], (int)x[0]);

                }
				catch (Exception)
				{
					throw new ArgumentException("Cannot cast to int");
				}
				return time; 
			} )
            .ToList();
    }

    public static List<string> FindDevicesFailedBeforeDate(DateTime requestTime, List<Device> devices)
	{
        var problematicDevices = devices
			.Where(device => device.Failure.IsFailureSerious && device.Failure.Time <= requestTime)
			.ToList();

        return problematicDevices
			.Select(device => device.Name)
			.ToList();
	}
}

public class Failure
{
    public FailureType FailureType { get; }
	public DateTime Time { get; }
	public bool IsFailureSerious => (int)FailureType % 2 == 0;


    public Failure(FailureType failureType, DateTime time)
	{
        FailureType = failureType;
		Time = time;
	}
}

public class Device
{
	public string Name { get; }
	public int Id { get; }
	public Failure Failure { get; }

	public Device(string name, int id, Failure failure)
	{
		Name = name;
		Id = id;
		Failure = failure;
	}
}

public enum FailureType
{
    UnexpectedShutdown,
	ShortNonResponding,
    HardwareFailures,
    ConnectionProblems
}