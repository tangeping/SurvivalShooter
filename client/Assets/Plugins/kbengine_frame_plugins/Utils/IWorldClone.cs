using System;

namespace SyncFrame
{
	public interface IWorldClone
	{
		string checksum
		{
			get;
		}

		void Clone(IWorld iWorld);

		void Clone(IWorld iWorld, bool doChecksum);

		void Restore(IWorld iWorld);
	}
}
