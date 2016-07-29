using System;
using System.Linq;
using PosApp.Domain;
using PosApp.Dtos.Requests;
using Xunit;

namespace PosApp.Test.Unit
{
    public class TagRequestExtensionFacts
    {
        [Fact]
        public void should_fail_if_tags_are_not_provided()
        {
            string[] nullTags = null;

            Assert.Null(nullTags.ToBoughtProducts());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void should_fail_if_tags_contains_null_or_empty_tag(string emptyTag)
        {
            string[] containsEmptyCode = {"barcode", emptyTag};

            Assert.Null(containsEmptyCode.ToBoughtProducts());
        }

        [Theory]
        [InlineData("barcode")]
        [InlineData("barcode-notnumber")]
        [InlineData("barcode-00x")]
        [InlineData("barcode-01-notnumber")]
        [InlineData("-")]
        [InlineData("----")]
        [InlineData("-barcode")]
        [InlineData("barcode-")]
        public void should_parse_tag_without_amount(string tag)
        {
            string[] validTags = {tag};

            BoughtProduct boughtProduct = validTags.ToBoughtProducts().Single();

            Assert.Equal(tag, boughtProduct.Barcode);
            Assert.Equal(1, boughtProduct.Amount);
        }

        [Theory]
        [InlineData("barcode-001", "barcode", 1)]
        [InlineData("barcode-notnumber-2", "barcode-notnumber", 2)]
        [InlineData("barcode-01-not-number-3", "barcode-01-not-number", 3)]
        [InlineData("barcode----2", "barcode---", 2)]
        [InlineData("--2", "-", 2)]
        public void should_parse_tag_with_amount(
            string tagWithAmount,
            string expectedBarcode,
            int expectedAmount)
        {
            string[] validTags = {tagWithAmount};

            BoughtProduct boughtProduct = validTags.ToBoughtProducts().Single();

            Assert.Equal(expectedBarcode, boughtProduct.Barcode);
            Assert.Equal(expectedAmount, boughtProduct.Amount);
        }

        [Theory]
        [InlineData("-2")]
        public void should_fail_if_tag_part_is_missing(string invalidTag)
        {
            string[] inValidTags = { invalidTag };

            Assert.Null(inValidTags.ToBoughtProducts());
        }
    }
}