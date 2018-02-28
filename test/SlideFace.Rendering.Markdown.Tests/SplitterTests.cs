using System;
using Xunit;

namespace SlideFace.Rendering.Markdown.Tests
{
    public class SplitterTests
    {
        [Fact]
        public void ReadsDeckFrontMatterAndFirstSlide()
        {
            
            const string text = "<!--\ntitle: Test\n-->\n<!--\nlayout: title-slide\n-->\nOne";
            using (var target = new Splitter(text))
            {
                var fm = target.ReadFrontMatter();
                var (_, md1) = target.ReadNextBlock();
                Assert.Equal("title: Test", fm.Trim());
                Assert.Equal("One", md1);
            }
        }
        
        [Fact]
        public void SplitsAtHtmlComment()
        {
            const string text = "<!--\n-->\nOne\n<!--\n-->\nTwo";
            using (var target = new Splitter(text))
            {
                var (_, md1) = target.ReadNextBlock();
                var (_, md2) = target.ReadNextBlock();
                Assert.Equal("One", md1);
                Assert.Equal("Two", md2);
            }
        }
        
        [Fact]
        public void SplitsAtEmptyHtmlComment()
        {
            const string text = "<!-- -->\nOne\n<!-- -->\nTwo";
            using (var target = new Splitter(text))
            {
                var (_, md1) = target.ReadNextBlock();
                var (_, md2) = target.ReadNextBlock();
                Assert.Equal("One", md1);
                Assert.Equal("Two", md2);
            }
        }
    }
}
