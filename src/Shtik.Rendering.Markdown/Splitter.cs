using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shtik.Rendering.Markdown
{
    public class Splitter : IDisposable
    {
        private readonly StreamReader _reader;
        private readonly StringBuilder _builder = new StringBuilder();
        public Splitter(Stream stream)
        {
            _reader = new StreamReader(stream);
        }

        public async Task<string> ReadNextBlockAsync()
        {
            if (_reader.EndOfStream) return null;
            bool first = true;
            _builder.Clear();
            while (!_reader.EndOfStream)
            {
                var line = await _reader.ReadLineAsync();
                if (line.StartsWith("---"))
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    return _builder.ToString().Trim();
                }
                _builder.AppendLine(line);
                first = false;
            }
            return _builder.ToString().Trim();
        }

        private void CheckDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("Splitter");
        }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }

                _disposed = true;
            }
        }

        ~Splitter()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}