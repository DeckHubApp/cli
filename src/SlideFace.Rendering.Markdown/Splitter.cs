using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SlideFace.Rendering.Markdown
{
    public sealed class Splitter : IDisposable
    {
        private readonly StringReader _reader;
        private readonly StringBuilder _frontMatterBuilder = new StringBuilder();
        private readonly StringBuilder _markdownBuilder = new StringBuilder();
        private bool _inFrontMatter;
        private bool _emptyFrontMatter;

        public Splitter(string markdown)
        {
            _reader = new StringReader(markdown);
        }

        public string ReadFrontMatter()
        {
            CheckDisposed();
            if (_reader.Peek() == -1) return null;
            _frontMatterBuilder.Clear();

            if (!ReadPastNextOpenComment())
            {
                return default;
            }
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (line.StartsWith("-->"))
                {
                    break;
                }
                _frontMatterBuilder.AppendLine(line);
            }

            return _frontMatterBuilder.ToString();
        }

        public (string, string) ReadNextBlock()
        {
            CheckDisposed();
            if (_reader.Peek() == -1) return default;

            string frontMatter = ReadSlideFrontMatter();

            _markdownBuilder.Clear();
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (line.StartsWith("<!--"))
                {
                    _inFrontMatter = true;
                    _emptyFrontMatter = line.EndsWith("-->");
                    return (frontMatter, _markdownBuilder.ToString().Trim());
                }
                _markdownBuilder.AppendLine(line);
            }
            return (frontMatter, _markdownBuilder.ToString().Trim());
        }

        private string ReadSlideFrontMatter()
        {
            if (!ReadPastNextOpenComment()) return default;
            
            if (_emptyFrontMatter)
            {
                _emptyFrontMatter = false;
                return string.Empty;
            }
            
            _frontMatterBuilder.Clear();
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (line.StartsWith("-->"))
                {
                    return _frontMatterBuilder.ToString().Trim();
                }
                _frontMatterBuilder.AppendLine(line);
            }
            return _frontMatterBuilder.ToString().Trim();
        }

        private bool ReadPastNextOpenComment()
        {
            if (_inFrontMatter || _emptyFrontMatter)
            {
                _inFrontMatter = false;
                return true;
            }
            
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (line.StartsWith("<!--"))
                {
                    _emptyFrontMatter = line.EndsWith("-->");
                    return true;
                }
            }

            return false;
        }

        private void CheckDisposed()
        {
            if (_disposed) throw new ObjectDisposedException("Splitter");
        }

        #region IDisposable Support
        private bool _disposed; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _reader.Dispose();
            }

            _disposed = true;
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