using System;

namespace SyncFrame
{
	public interface IBody
	{
		bool TSDisabled
		{
			get;
			set;
		}

		string Checkum();
	}
}
