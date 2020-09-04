using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Helpers
{
    public class PageHelper
    {
        public static IEnumerable<int> PageNumbers(int pageNumber, int pageCount)
        {
            if (pageCount <= 5)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    yield return i;
                }
            }
            else
            {
                //range of 3 ie +1 from left border or -1 from right border
                int midPoint = pageNumber < 2 ? 2
                    : pageNumber > pageCount - 1 ? pageCount - 1
                    : pageNumber;

                int lowerBound = midPoint - 1;
                int upperBound = midPoint + 1;

                if (lowerBound != 1)
                {
                    yield return 1;
                    if (lowerBound - 1 > 1)
                    {
                        yield return -1;
                    }
                }

                for (int i = midPoint - 1; i <= upperBound; i++)
                {
                    yield return i;
                }

                if (upperBound != pageCount)

                    if (pageCount - upperBound > 1)
                    {
                        yield return -1;
                    }
                yield return pageCount;
            }
        }
    }
}
