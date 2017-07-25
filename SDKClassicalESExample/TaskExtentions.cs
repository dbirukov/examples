using System.Threading.Tasks;

namespace SDKClassicalESExample
{
    public static class TaskExtentions
    {
        public static async Task<T> Retry<T>(this Task<T> task, int retryCount, int delayBetweenRetries)
        {
            while (true)
            {
                try
                {
                    var result = await task;
                    return result;
                }
                catch when (retryCount-- > 0){}
                if (delayBetweenRetries > 0)
                {
                    await Task.Delay(delayBetweenRetries);
                }
            }
        }
    }
}