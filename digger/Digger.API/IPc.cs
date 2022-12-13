namespace Digger.API
{
    public interface IPc
    {
        int GetWidth();

        int GetHeight();

        int[] GetPixels();

        IRefresher GetCurrentSource();

        (int x, int y, bool vis) GetPlayer();
    }
}