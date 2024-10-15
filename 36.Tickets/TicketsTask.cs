using System.Numerics;
namespace Tickets;

public class TicketsTask
{
    public static BigInteger Solve(int halfLen, int totalSum)
    {
        if (totalSum % 2 != 0) return 0;

        var halfSum = totalSum / 2;
        var dp = new BigInteger[halfLen + 1, halfSum + 1];
        dp[0, 0] = 1;

        for (var i = 1; i <= halfLen; i++)
            for (var sum = 0; sum <= halfSum; sum++)
                for (int digit = 0; digit <= 9; digit++)
                    if (sum >= digit)
                    {
                        dp[i, sum] += dp[i - 1, sum - digit];
                    }

        return dp[halfLen, halfSum] * dp[halfLen, halfSum];
    }
}