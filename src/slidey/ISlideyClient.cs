using System.IO;
using System.Threading.Tasks;

namespace slidey
{
    public interface ISlideyClient
    {
        Task<LiveShow> StartShow(StartShow start);
        Task<bool> SetShown(string presenter, string slug, int index, Stream slide, string contentType);
    }
}