using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Slidey.Rendering.Markdown
{
    public sealed class Splitter : IDisposable
    {
        private readonly StringReader _reader;
        private readonly StringBuilder _builder = new StringBuilder();
        private bool _first = true;
        private string _firstSlideFrontMatter;
        private string _firstSlideMarkdown;

        public Splitter(string markdown)
        {
            _reader = new StringReader(markdown);
        }

        public string ReadFrontMatter()
        {
            CheckDisposed();
            if (_reader.Peek() == -1) return null;
            _builder.Clear();
            var list = new List<string>();
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (line.StartsWith("..."))
                {
                    list.Add(_builder.ToString());
                    _builder.Clear();
                    continue;
                }
                if (line.StartsWith("---"))
                {
                    list.Add(_builder.ToString());
                    break;
                }
                _builder.AppendLine(line);
            }

            switch (list.Count)
            {
                case 0:
                    return string.Empty;
                case 1:
                    _firstSlideMarkdown = list[0];
                    return string.Empty;
                case 2:
                    _firstSlideFrontMatter = list[0];
                    _firstSlideMarkdown = list[1];
                    return string.Empty;
                default:
                    _firstSlideFrontMatter = list[1];
                    _firstSlideMarkdown = list[2];
                    return list[0];
            }
        }

        public (string, string) ReadNextBlock()
        {
            CheckDisposed();
            if (_first)
            {
                _first = false;
                return (_firstSlideFrontMatter, _firstSlideMarkdown);
            }
            if (_reader.Peek() == -1) return (null, null);
            _builder.Clear();
            string frontMatter = string.Empty;
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                if (line.StartsWith("..."))
                {
                    frontMatter = _builder.ToString();
                    _builder.Clear();
                    continue;
                }
                else if (line.StartsWith("---"))
                {
                    return (frontMatter, _builder.ToString().Trim());
                }
                _builder.AppendLine(line);
            }
            return (frontMatter, _builder.ToString().Trim());
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