namespace FibonacciTest
{
    public interface ICache
    {
        int Get(int index);
        void Set(int index, int value);
    }
}
