//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public class Tasks
	{
		Ped _ped;

		internal Tasks(Ped ped)
		{
			_ped = ped;
		}

		public void AchieveHeading(float heading)
		{
			AchieveHeading(heading, 0);
		}
		public void AchieveHeading(float heading, int timeout)
		{
			Function.Call(Hash.TASK_ACHIEVE_HEADING, _ped.Handle, heading, timeout);
		}
		public void AimAt(Entity target, int duration)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_ENTITY, _ped.Handle, target.Handle, duration, 0, 0);
		}
		public void AimAt(Vector3 target, int duration)
		{
			Function.Call(Hash.TASK_AIM_GUN_AT_COORD, _ped.Handle, target.X, target.Y, target.Z, duration, 0, 0);
		}
		public void Arrest(Ped ped)
		{
			Function.Call(Hash.TASK_ARREST_PED, _ped.Handle, ped.Handle);
		}

		public void Climb()
		{
			Function.Call(Hash.TASK_CLIMB, _ped.Handle, true);
		}
		public void Cower(int duration)
		{
			Function.Call(Hash.TASK_COWER, _ped.Handle, duration, 0, 0);
		}
		public void CruiseWithVehicle(Vehicle vehicle, float speed)
		{
			CruiseWithVehicle(vehicle, speed, 0);
		}
		public void CruiseWithVehicle(Vehicle vehicle, float speed, int drivingstyle)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_WANDER, _ped.Handle, vehicle.Handle, speed, drivingstyle);
		}
		public void DriveTo(Vehicle vehicle, Vector3 position, float radius, float speed)
		{
			DriveTo(vehicle, position, radius, speed, 0);
		}
		public void DriveTo(Vehicle vehicle, Vector3 position, float radius, float speed, int drivingstyle)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, _ped.Handle, vehicle.Handle, position.X, position.Y, position.Z, speed, Function.Call<int>(Hash.GET_HASH_KEY, vehicle), drivingstyle, radius, false);
		}
		public void EnterVehicle()
		{
			EnterVehicle(new Vehicle(0), VehicleSeat.Any, -1, 0.0f, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			EnterVehicle(vehicle, seat, -1, 0.0f, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout)
		{
			EnterVehicle(vehicle, seat, timeout, 0.0f, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout, float speed)
		{
			EnterVehicle(vehicle, seat, timeout, speed, 0);
		}
		public void EnterVehicle(Vehicle vehicle, VehicleSeat seat, int timeout, float speed, int flag)
		{
			Function.Call(Hash.TASK_ENTER_VEHICLE, _ped.Handle, vehicle.Handle, timeout, (int)(seat), speed, flag, 0);
		}
		public static void EveryoneLeaveVehicle(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_EVERYONE_LEAVE_VEHICLE, vehicle.Handle, 0);
		}
		public void FightAgainst(Ped target)
		{
			Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, 0, 16);
		}
		public void FightAgainst(Ped target, int duration)
		{
			Function.Call(Hash.TASK_COMBAT_PED_TIMED, _ped.Handle, target.Handle, duration, 0);
		}
		public void FightAgainstHatedTargets(float radius)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED, _ped.Handle, radius, 0);
		}
		public void FightAgainstHatedTargets(float radius, int duration)
		{
			Function.Call(Hash.TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, _ped.Handle, radius, duration, 0);
		}
		public void FleeFrom(Ped ped)
		{
			FleeFrom(ped, -1);
		}
		public void FleeFrom(Ped ped, int duration)
		{
			Function.Call(Hash.TASK_SMART_FLEE_PED, _ped.Handle, ped.Handle, 100.0f, duration, 0, 0, 0);
		}
		public void FleeFrom(Vector3 position)
		{
			FleeFrom(position, -1);
		}
		public void FleeFrom(Vector3 position, int duration)
		{
			Function.Call(Hash.TASK_SMART_FLEE_COORD, _ped.Handle, position.X, position.Y, position.Z, 100.0f, duration, 0, 0);
		}
		public void FollowPointRoute(params Vector3[] points)
		{
			Function.Call(Hash.TASK_FLUSH_ROUTE);

			foreach (Vector3 point in points)
			{
				Function.Call(Hash.TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
			}

			Function.Call(Hash.TASK_FOLLOW_POINT_ROUTE, _ped.Handle, 1.0f, 0, 0, 0, 0);
		}
		public void GoTo(Entity target)
		{
			GoTo(target, Vector3.Zero, -1);
		}
		public void GoTo(Entity target, Vector3 offset)
		{
			GoTo(target, offset, -1);
		}
		public void GoTo(Entity target, Vector3 offset, int timeout)
		{
			Function.Call(Hash.TASK_GOTO_ENTITY_OFFSET_XY, _ped.Handle, target.Handle, timeout, offset.X, offset.Y, offset.Z, 1.0f, true);
		}
		public void GoTo(Vector3 position)
		{
			GoTo(position, false, -1);
		}
		public void GoTo(Vector3 position, bool ignorePaths)
		{
			GoTo(position, ignorePaths, -1);
		}
		public void GoTo(Vector3 position, bool ignorePaths, int timeout)
		{
			if (ignorePaths)
			{
				Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f, 0.0f, 0);
			}
			else
			{
				Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f, 0, 0.0f);
			}
		}
		public void FollowToOffsetFromEntity(Entity target, Vector3 offset, int timeout, float stoppingRange)
		{
			FollowToOffsetFromEntity(target, offset, 1.0f, timeout, stoppingRange, true);
		}
		public void FollowToOffsetFromEntity(Entity target, Vector3 offset, float movementSpeed, int timeout, float stoppingRange, bool persistFollowing)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, target.Handle, offset.X, offset.Y, offset.Z, movementSpeed, timeout, stoppingRange, persistFollowing);
		}
		public void GuardCurrentPosition()
		{
			Function.Call(Hash.TASK_GUARD_CURRENT_POSITION, _ped.Handle, 15.0f, 10.0f, true);
		}
		public void HandsUp(int duration)
		{
			Function.Call(Hash.TASK_HANDS_UP, _ped.Handle, duration, 0, -1, false);
		}
		public void Jump()
		{
			Function.Call(Hash.TASK_JUMP, _ped.Handle, true);
		}
		public void LeaveVehicle()
		{
			Function.Call(Hash.TASK_LEAVE_ANY_VEHICLE, _ped.Handle, 0, 0 /* flags */);
		}
		public void LeaveVehicle(Vehicle vehicle, bool closeDoor)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, closeDoor ? 0 : 1 << 8);
		}
		public void LookAt(Entity target)
		{
			LookAt(target, -1);
		}
		public void LookAt(Entity target, int duration)
		{
			Function.Call(Hash.TASK_LOOK_AT_ENTITY, _ped.Handle, target.Handle, duration, 0 /* flags */, 2, 0);
		}
		public void LookAt(Vector3 position)
		{
			LookAt(position, -1);
		}
		public void LookAt(Vector3 position, int duration)
		{
			Function.Call(Hash.TASK_LOOK_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, 0 /* flags */, 2, 0);
		}
		public void PerformSequence(TaskSequence sequence)
		{
			if (!sequence.IsClosed)
			{
				sequence.Close();
			}

			ClearAll();

			_ped.BlockPermanentEvents = true;

			Function.Call(Hash.TASK_PERFORM_SEQUENCE, _ped.Handle, sequence.Handle);
		}

		public void PlayAnimation(string animDict, string animName)
		{
			PlayAnimation(animDict, animName, 8f, -8f, -1, AnimationFlags.None, 0f);
		}
		public void PlayAnimation(string animDict, string animName, float speed, int duration, float playbackRate)
		{
			PlayAnimation(animDict, animName, speed, -speed, duration, AnimationFlags.None, playbackRate);
		}
		public void PlayAnimation(string animDict, string animName, float blendInSpeed, int duration, AnimationFlags flags)
		{
			PlayAnimation(animDict, animName, blendInSpeed, -8f, duration, flags, 0f);
		}

		public void PlayAnimation(string animDict, string animName, float blendInSpeed, float blendOutSpeed, int duration,
			AnimationFlags flags, float playbackRate, float timeout = 1000f)
		{
			if (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, animDict);
			}

			var end = DateTime.UtcNow.AddMilliseconds(timeout);
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, animDict))
			{
				if (DateTime.UtcNow >= end)
				{
					return;
				}
			}

			Function.Call(Hash.TASK_PLAY_ANIM, _ped.Handle, animDict, animName, blendInSpeed, blendOutSpeed,
				duration, (int)flags, playbackRate, false, false, false);
		}

		public void ClearAnimation(string animDict, string animName, float blendOutSpeed = -8f)
		{
			Function.Call(Hash.STOP_ANIM_TASK, _ped.Handle, animDict, animName, blendOutSpeed);
		}

		public void ReactToEvent()
		{
			Function.Call(Hash.TASK_REACT, _ped.Handle);
		}
		public void ReloadWeapon()
		{
			Function.Call(Hash.TASK_RELOAD_WEAPON, _ped.Handle, true);
		}
		public void RunTo(Vector3 position)
		{
			RunTo(position, false, -1);
		}
		public void RunTo(Vector3 position, bool ignorePaths)
		{
			RunTo(position, ignorePaths, -1);
		}
		public void RunTo(Vector3 position, bool ignorePaths, int timeout)
		{
			if (ignorePaths)
			{
				Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f, 0.0f, 0);
			}
			else
			{
				Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f, 0, 0.0f);
			}
		}
		public void ShootAt(Ped target)
		{
			ShootAt(target, -1, 0);
		}
		public void ShootAt(Ped target, int duration)
		{
			ShootAt(target, duration, 0);
		}
		public void ShootAt(Ped target, int duration, int pattern)
		{
			Function.Call(Hash.TASK_SHOOT_AT_ENTITY, _ped.Handle, target.Handle, duration, (int)(pattern), 0);
		}
		public void ShootAt(Vector3 position)
		{
			ShootAt(position, -1, 0);
		}
		public void ShootAt(Vector3 position, int duration)
		{
			ShootAt(position, duration, 0);
		}
		public void ShootAt(Vector3 position, int duration, int pattern)
		{
			Function.Call(Hash.TASK_SHOOT_AT_COORD, _ped.Handle, position.X, position.Y, position.Z, duration, (int)(pattern), 0);
		}
		public void ShuffleToNextVehicleSeat(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, _ped.Handle, vehicle.Handle);
		}
		public void SlideTo(Vector3 position, float heading)
		{
			Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, _ped.Handle, position.X, position.Y, position.Z, heading, 0.7f);
		}
		public void StandStill(int duration)
		{
			Function.Call(Hash.TASK_STAND_STILL, _ped.Handle, duration);
		}
		public void StartScenario(string name)
		{
			Function.Call(Hash._TASK_START_SCENARIO_IN_PLACE, _ped.Handle, name, 0, 1);
		}
		public void StartScenarioInPlace(string scenario, bool playEnterAnim = true, int p4 = -1082130432)
		{
			Function.Call(Hash._TASK_START_SCENARIO_IN_PLACE, _ped.Handle, Game.GenerateHash(scenario), -1, playEnterAnim, false, p4, false);
		}

		public void TurnTo(Entity target)
		{
			TurnTo(target, -1);
		}
		public void TurnTo(Entity target, int duration)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, _ped.Handle, target.Handle, duration, 0, 0, 0);
		}
		public void TurnTo(Vector3 position)
		{
			TurnTo(position, -1);
		}
		public void TurnTo(Vector3 position, int duration)
		{
			Function.Call(Hash.TASK_TURN_PED_TO_FACE_COORD, _ped.Handle, position.X, position.Y, position.Z, duration);
		}
		/*public void VehicleChase(Ped target)
		{
			Function.Call(Hash.TASK_VEHICLE_CHASE, _ped.Handle, target.Handle);
		}*/ // unknown native
		public void Wait(int duration)
		{
			Function.Call(Hash.TASK_PAUSE, _ped.Handle, duration);
		}
		public void WanderAround()
		{
			Function.Call(Hash.TASK_WANDER_STANDARD, _ped.Handle, 0, 0);
		}
		public void WanderAround(Vector3 position, float radius)
		{
			Function.Call(Hash.TASK_WANDER_IN_AREA, _ped.Handle, position.X, position.Y, position.Z, radius, 0.0f, 0.0f, 0);
		}
		public void WarpIntoVehicle(Vehicle vehicle, int seat)
		{
			Function.Call(Hash.TASK_WARP_PED_INTO_VEHICLE, _ped.Handle, vehicle.Handle, (int)(seat));
		}
		public void WarpOutOfVehicle(Vehicle vehicle)
		{
			Function.Call(Hash.TASK_LEAVE_VEHICLE, _ped.Handle, vehicle.Handle, 0, 0);
		}

		public void ClearAll()
		{
			Function.Call(Hash.CLEAR_PED_TASKS, _ped.Handle);
		}
		public void ClearAllImmediately()
		{
			Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, _ped.Handle, true, true);
		}
		public void ClearLookAt()
		{
			Function.Call(Hash.TASK_CLEAR_LOOK_AT, _ped.Handle);
		}
		public void ClearSecondary()
		{
			Function.Call(Hash.CLEAR_PED_SECONDARY_TASK, _ped.Handle);
		}

		public void Whistle()
		{
			Function.Call((Hash)0xD6401A1B2F63BED6, _ped.Handle, 869278708, 1971704925);
		}

		public void HandsUp(int duration, Ped facingPed)
		{
			Function.Call(Hash.TASK_HANDS_UP, _ped.Handle, duration, facingPed == null ? 0 : facingPed.Handle, -1, false);
		}

		public void KnockOut(float angle, bool immediately)
		{
			Function.Call((Hash)0xF90427F00A495A28, _ped.Handle, angle, immediately);
		}

		public void KnockOutAndHogtied(float angle, bool immediately)
		{
			Function.Call((Hash)0x42AC6401ABB8C7E5, _ped.Handle, angle, immediately);
		}

		public void Combat(Ped target)
		{
			Function.Call(Hash.TASK_COMBAT_PED, _ped.Handle, target.Handle, 0, 0);
		}

		public void ReviveTarget(Ped target)
		{
			Function.Call((Hash)0x356088527D9EBAAD, _ped.Handle, target.Handle, -1516555556);
		}

		public void SeekCoverFrom(Ped target, int duration)
		{
			Function.Call(Hash.TASK_SEEK_COVER_FROM_PED, _ped.Handle, target.Handle, duration,
				false, false, false);
		}

		public void SeekCoverFrom(Vector3 pos, int duration)
		{
			Function.Call(Hash.TASK_SEEK_COVER_FROM_POS, _ped.Handle, pos.X, pos.Y, pos.Z, duration,
				false, false, false);
		}

		public void StandGuard(Vector3 pos = default)
		{
			pos = pos == default ? _ped.Position : pos;
			Function.Call(Hash.TASK_STAND_GUARD, _ped.Handle, pos.X, pos.Y, pos.Z, _ped.Heading, "DEFEND");
		}

		public void Rob(Ped target, int duration, int flag = 18)
		{
			Function.Call((Hash)0x7BB967F85D8CCBDB, _ped.Handle, target.Handle, flag, duration);
		}

		public void Flock()
		{
			Function.Call((Hash)0xE0961AED72642B80, _ped.Handle);
		}

		public void Duck(int duration)
		{
			Function.Call((Hash)0xA14B5FBF986BAC23, _ped.Handle, duration);
		}

		public void EnterCover()
		{
			Function.Call((Hash)0x4972A022AE6DAFA1, _ped.Handle);
		}

		public void ExitCover()
		{
			Function.Call((Hash)0x2BC4A6D92D140112, _ped.Handle);
		}

		public void EnterTransport()
		{
			Function.Call((Hash)0xAEE3ADD08829CB6F, _ped.Handle);
		}

		public void ExitVehicle(Vehicle vehicle, LeaveVehicleFlags flag = LeaveVehicleFlags.None)
		{
			Function.Call((Hash)0xD3DBCE61A490BE02, _ped.Handle, vehicle.Handle, (int)flag);
		}

		public void MountAnimal(Ped animal, int timeout = -1)
		{
			Function.Call((Hash)0x92DB0739813C5186, _ped.Handle, animal.Handle, timeout, -1, 2f, 1, 0, 0);
		}

		public void DismountAnimal(Ped animal)
		{
			Function.Call((Hash)0x48E92D3DDE23C23A, _ped.Handle, animal.Handle, 0, 0, 0, 0);
		}

		public void HitchAnimal(Ped animal, int flag = 0)
		{
			Function.Call((Hash)0x9030AD4B6207BFE8, _ped.Handle, animal.Handle, flag);
		}

		public void DriveToCoord(Vehicle vehicle, Vector3 pos, float speed, float radius = 6f, VehicleDrivingFlags drivingMode = VehicleDrivingFlags.Default)
		{
			Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, _ped.Handle, vehicle.Handle, pos.X, pos.Y, pos.Z, speed,
				radius, vehicle.Model.Hash, (int)drivingMode);
		}

		public void FollowToEntity(Entity entity, float speed, Vector3 offset = default, int timeout = -1, float stoppingRange = 3f, bool keepFollowing = true)
		{
			Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, _ped.Handle, entity.Handle, offset.X, offset.Y,
				offset.Z, speed, timeout, stoppingRange, keepFollowing);
		}

		public void GoToWhistle(Entity target, int flag = 3)
		{
			Function.Call((Hash)0xBAD6545608CECA6E, _ped.Handle, target.Handle, flag);
		}

		public void LeadHorse(Ped horse)
		{
			Function.Call((Hash)0x9A7A4A54596FE09D, _ped.Handle, horse.Handle);
		}

		public void FlyAway(Entity awayFrom = null)
		{
			Function.Call((Hash)0xE86A537B5A3C297C, awayFrom == null ? 0 : awayFrom.Handle);
		}

		public void WalkAway(Entity awayFrom = null)
		{
			Function.Call((Hash)0x04ACFAC71E6858F9, awayFrom == null ? 0 : awayFrom.Handle);
		}

		public void ReactToShockingEvent()
		{
			Function.Call((Hash)0x452419CBD838065B, 0, _ped.Handle, 0);
		}

		public void ReactTo(Entity target, EventReaction reaction, float p2 = 7.5f, float p3 = 0f, int flag = 4)
		{
			Function.Call((Hash)0xC4C32C31920E1B70, _ped.Handle, target.Handle, (int)reaction, p2, p3, flag);
		}
		public void HuntAnimal(Ped target)
		{
			Function.Call((Hash)0x4B39D8F9D0FE7749, target.Handle, _ped.Handle, 1);
		}
	}

	public enum EventReaction
	{
		TaskCombatHigh = 1103872808,
		TaskCombatMedium = 623557147,
		TaskCombatReact = -1342511871,
		TaskCombatPanic = -996719768,
		DefaultShocked = -372548123,
		DefaultPanic = 1618376518,
		DefaultCurious = -1778605437,
		DefaultBrave = 1781933509,
		DefaultAngry = 1345150177,
		DefaultDefuse = -1675652957,
		DefaultScared = -1967172690,
		FleeHumanMajorThreat = -2111647205,
		FleeScared = 759577278
	}
}
