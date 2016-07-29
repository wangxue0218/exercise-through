using System.Collections.Generic;
using System.Linq;
using PosApp.Domain;

namespace PosApp.Dtos.Requests
{
    static class TagsRequestExtension
    {
        public static BoughtProduct[] ToBoughtProducts(this IEnumerable<string> tags)
        {
            if (tags == null)
            {
                return null;
            }

            string[] tagArray = tags.ToArray();
            if (tagArray.Length == 0)
            {
                return null;
            }

            if (tagArray.Any(string.IsNullOrWhiteSpace))
            {
                return null;
            }

            BoughtProduct[] boughtProducts = tagArray.Select(ToBoughtProduct)
                .TakeWhile(bp => bp != null)
                .ToArray();
            return boughtProducts.Length == tagArray.Length ? boughtProducts : null;
        }

        static BoughtProduct ToBoughtProduct(string tag)
        {
            int possibleAmountDividerIndex = tag.LastIndexOf('-');
            if (possibleAmountDividerIndex == -1 || possibleAmountDividerIndex == tag.Length - 1)
            {
                return new BoughtProduct(tag, 1);
            }

            if (possibleAmountDividerIndex == 0)
            {
                int shouldNotBeAmount = TryGetAmount(tag, 0);
                if (shouldNotBeAmount != -1)
                {
                    return null;
                }

                return new BoughtProduct(tag, 1);
            }

            int amount = TryGetAmount(tag, possibleAmountDividerIndex);
            return amount == -1
                ? new BoughtProduct(tag, 1)
                : new BoughtProduct(tag.Substring(0, possibleAmountDividerIndex), amount);
        }

        static int TryGetAmount(string tag, int dividerIndex)
        {
            var numberPart = tag.Substring(dividerIndex + 1, tag.Length - dividerIndex - 1);
            int amount;
            if (int.TryParse(numberPart, out amount))
            {
                return amount;
            }

            return -1;
        }
    }
}