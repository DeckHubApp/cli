using System.IO;
using System.Threading.Tasks;

namespace Slidable
{
    public interface ISlidableClient
    {
        Task<LiveShow> StartShow(StartShow start);
        Task SetShown(string presenter, string slug, int index, Stream slide, string contentType);
    }
}