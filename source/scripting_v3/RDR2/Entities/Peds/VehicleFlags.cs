using System;

namespace RDR2
{
	[Flags]
	public enum LeaveVehicleFlags
	{
		None = 0,
		WarpOut = 1 << 4,
		LeaveDoorOpen = 1 << 8,
		BailOut = 1 << 12,
		MoveFromPassenger = 1 << 18
	}

	[Flags]
	public enum VehicleDrivingFlags
	{
		Default = AvoidVehicles | AvoidEmptyVehicles | StopBeforePeds | StopBeforeVehicles | AvoidPeds | AvoidObjects,
		StopBeforeVehicles = 1 << 0,
		StopBeforePeds = 1 << 1,
		AvoidVehicles = 1 << 2,
		AvoidEmptyVehicles = 1 << 3,
		AvoidPeds = 1 << 4,
		AvoidObjects = 1 << 5,
		Unk6 = 1 << 6,
		ObeyTrafficStops = 1 << 7,
		UseBlinkers = 1 << 8,
		AllowCuttingTraffic = 1 << 9,
		Reverse = 1 << 10,
		Unk11 = 1 << 11,
		Unk12 = 1 << 12,
		Unk13 = 1 << 13,
		Unk14 = 1 << 14,
		Unk15 = 1 << 15,
		Unk16 = 1 << 16,
		Unk17 = 1 << 17,
		TakeShortestPath = 1 << 18,
		AvoidOffroad = 1 << 19,
		Unk20 = 1 << 20,
		Unk21 = 1 << 21,
		IgnoreRoads = 1 << 22,
		Unk23 = 1 << 23,
		IgnoreAllPathing = 1 << 24,
		Unk25 = 1 << 25,
		Unk26 = 1 << 26,
		Unk27 = 1 << 27,
		Unk28 = 1 << 28,
		AvoidHighways = 1 << 29,
		Unk30 = 1 << 30
	}
}
