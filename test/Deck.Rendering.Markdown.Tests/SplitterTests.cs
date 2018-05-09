using System;
using Deck.Rendering.Markdown;
using Xunit;

namespace Deck.Rendering.Markdown.Tests
{
    public class SplitterTests
    {
        [Fact]
        public void ReadsDeckFrontMatterAndFirstSlide()
        {
            
            const string text = "---\ntitle: Test\n---\n***\n---\nlayout: title-slide\n---\nOne";
            using (var target = new Splitter(text))
            {
                var fm = target.ReadFrontMatter();
                var block = target.ReadNextBlock();
                Assert.Equal("title: Test", fm.Trim());
                Assert.Equal("layout: title-slide", block.FrontMatter);
                Assert.Equal("One", block.Slide);
                Assert.Null(block.Notes);
            }
        }
        
        [Fact]
        public void SplitsAtHtmlComment()
        {
            const string text = "***\n\nOne\n***\nTwo";
            using (var target = new Splitter(text))
            {
                var block1  = target.ReadNextBlock();
                var block2 = target.ReadNextBlock();
                Assert.Equal("One", block1.Slide);
                Assert.Equal("Two", block2.Slide);
            }
        }
        
        [Fact]
        public void SplitsAtEmptyHtmlComment()
        {
            const string text = "One\n***\nTwo";
            using (var target = new Splitter(text))
            {
                var block1 = target.ReadNextBlock();
                var block2 = target.ReadNextBlock();
                Assert.Equal("One", block1.Slide);
                Assert.Equal("Two", block2.Slide);
            }
        }
    }
}
