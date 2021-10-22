namespace WebAppExample.Services
{
    public class RandomizerMock : IRandomizer
    {
        public int Next(int min, int max)
        {
            return min;
        }
    }
}
