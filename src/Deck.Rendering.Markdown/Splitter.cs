using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Markdig.Syntax.Inlines;

namespace Deck.Rendering.Markdown
{
    public sealed class Splitter : IDisposable
    {
        private readonly StringReader _reader;
        private readonly StringBuilder _builder = new StringBuilder();

        public Splitter(string markdown)
        {
            _reader = new StringReader(markdown);
        }

        public string ReadFrontMatter()
        {
            CheckDisposed();
            if (_reader.Peek() == -1) return null;
            _builder.Clear();

            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                Debug.Assert(line != null);
                if (line.StartsWith("***"))
                {
                    break;
                }

                if (!line.StartsWith("---"))
                {
                    _builder.AppendLine(line);
                }
            }

            return _builder.ToString();
        }

        public Block ReadNextBlock()
        {
            CheckDisposed();
            if (_reader.Peek() == -1) return default;

            var (frontMatter, foundFrontMatter) = ReadSlideFrontMatter();
            var (markdown, hasNotes) = ReadSlide(foundFrontMatter ? null : frontMatter);
            var notes = hasNotes ? ReadNotes() : null;
            
            return new Block(foundFrontMatter ? frontMatter : null, markdown, notes);
        }

        private (string, bool) ReadSlide(string pre)
        {
            _builder.Clear();
            if (pre != null)
            {
                _builder.AppendLine(pre);
            }
            bool previousLineWasEmpty = false;
            bool inSyntax = false;
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                Debug.Assert(line != null);

                if (!inSyntax)
                {
                    line = line.TrimEnd();
                    if (line == "---" && previousLineWasEmpty)
                    {
                        return (_builder.ToString(), true);
                    }
                    
                    if (line == "***")
                    {
                        return (_builder.ToString(), false);
                    }

                    if (line == "")
                    {
                        previousLineWasEmpty = true;
                    }
                }

                if (line.StartsWith("```") || line.StartsWith("~~~"))
                {
                    inSyntax = !inSyntax;
                }
                
                _builder.AppendLine(line);
            }
            return (_builder.ToString(), false);
        }

        private string ReadNotes()
        {
            _builder.Clear();
            bool inSyntax = false;
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                Debug.Assert(line != null);

                if (!inSyntax)
                {
                    line = line.TrimEnd();
                    
                    if (line == "***")
                    {
                        return _builder.ToString();
                    }
                }

                if (line.StartsWith("```") || line.StartsWith("~~~"))
                {
                    inSyntax = !inSyntax;
                }
                
                _builder.AppendLine(line);
            }
            return _builder.ToString();
        }

        private (string, bool) ReadSlideFrontMatter()
        {
            _builder.Clear();
            bool foundFrontMatter = false;
            while (_reader.Peek() >= 0)
            {
                var line = _reader.ReadLine();
                Debug.Assert(line != null);

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                
                if (line.StartsWith("---"))
                {
                    if (foundFrontMatter)
                    {
                        return (_builder.ToString().Trim(), true);
                    }

                    foundFrontMatter = true;
                    continue;
                }
                else
                {
                    if (!foundFrontMatter)
                    {
                        return (line, false);
                    }
                }
                _builder.AppendLine(line);
            }
            return (_builder.ToString().Trim(), true);
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