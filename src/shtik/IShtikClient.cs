using System.Threading.Tasks;

namespace shtik
{
    public interface IShtikClient
    {
        Task<LiveShow> StartShow(StartShow start);
    }
}