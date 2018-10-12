using System;
using System.Collections.Generic;

namespace SyncFrame
{
	internal class ResourcePoolListSyncedData : ResourcePool<List<SyncedData>>
	{
		protected override List<SyncedData> NewInstance()
		{
			return new List<SyncedData>();
		}
	}
}
