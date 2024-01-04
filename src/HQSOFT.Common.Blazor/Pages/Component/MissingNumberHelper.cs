using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HQSOFT.Common.Blazor.Pages.Component
{
	public class MissingNumberHelper
	{
		public static int FindMissingNumber<T>(IEnumerable<T> items, Func<T, int> getIndexFunc, int maxIdx)
		{
			HashSet<int> idxSet = new HashSet<int>(items.Select(getIndexFunc));

			for (int i = 1; i <= maxIdx + 1; i++)
			{
				if (!idxSet.Contains(i))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
