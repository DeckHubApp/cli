using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Slidable.Embedded
{
    public static class ArraySegmentByteExtensions
    {
        public static string Utf8ToString(this ArraySegment<byte> segment)
        {
            return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }

    }

    public static class StreamExtensions
    {
        public static Task WriteAsync(this Stream stream, ArraySegment<byte> segment)
        {
            return stream.WriteAsync(segment.Array, segment.Offset, segment.Count);
        }
    }
}