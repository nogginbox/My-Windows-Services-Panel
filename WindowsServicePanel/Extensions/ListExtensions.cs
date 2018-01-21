using System.Collections.Generic;

namespace WindowsServicePanel.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Add all items to a list
        /// </summary>
        public static IList<T> Add<T>(this IList<T> targetList, IEnumerable<T> newItems)
        {
            foreach(var item in newItems)
            {
                targetList.Add(item);
            }
            return targetList;
        }
    }
}
