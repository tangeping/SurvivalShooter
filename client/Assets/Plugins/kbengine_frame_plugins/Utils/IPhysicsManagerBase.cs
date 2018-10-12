using System;
using SyncFrame;

public interface IPhysicsManagerBase
{
	void Init();

	void UpdateStep();

	IWorld GetWorld();

	IWorldClone GetWorldClone();

	void RemoveBody(IBody iBody);
}
