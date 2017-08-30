using System.Threading.Tasks;

namespace shtik
{
    public interface IShtikClient
    {
        Task<LiveShow> StartShow(StartShow start);
        Task<bool> SetShown(LiveShow show, int index);
    }
}