using System.IO;
using System.Threading.Tasks;

namespace Deck
{
    public interface IDeckHubClient
    {
        Task<LiveShow> StartShow(StartShow start);
        Task SetShown(string place, string presenter, string slug, int index, Stream slide, string contentType);
    }
}