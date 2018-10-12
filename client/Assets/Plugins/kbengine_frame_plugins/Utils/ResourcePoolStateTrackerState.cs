using System;

namespace SyncFrame
{
	internal class ResourcePoolStateTrackerState : ResourcePool<StateTracker.State>
	{
		protected override StateTracker.State NewInstance()
		{
			return new StateTracker.State();
		}
	}
}
