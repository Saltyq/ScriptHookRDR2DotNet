//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using RDR2.NaturalMotion;
using System;

namespace RDR2
{
	public sealed class Ped : Entity
	{
		#region Fields
		Tasks _tasks;
		NaturalMotion.Euphoria _euphoria;
		WeaponCollection _weapons;


		#endregion

		public Ped(int handle) : base(handle)
		{
		}

		public void Clone()
		{
			Clone(0.0F);
		}
		public void Clone(float heading)
		{
			Function.Call(Hash.CLONE_PED, Handle, heading, false, false);
		}

		/*public void Kill()
		{
			Health = -1;
		}*/

		#region Styling
		public float Scale
		{
			set => Function.Call((Hash)0x25ACFC650B65C538, Handle, value);
		}
		public bool IsHuman => Function.Call<bool>(Hash.IS_PED_HUMAN, Handle);

		public bool IsCuffed => Function.Call<bool>(Hash.IS_PED_CUFFED, Handle);

		public void ClearBloodDamage()
		{
			Function.Call(Hash.CLEAR_PED_BLOOD_DAMAGE, Handle);
		}

		public void ResetVisibleDamage()
		{
			Function.Call(Hash._0x88A5564B19C15391, Handle);
		}

		public Gender Gender => Function.Call<bool>(Hash.IS_PED_MALE, Handle) ? Gender.Male : Gender.Female;

		public float Sweat
		{
			set {
				if (value < 0)
				{
					value = 0;
				}
				if (value > 100)
				{
					value = 100;
				}

				Function.Call(Hash.SET_PED_SWEAT, Handle, value);
			}
		}

		public float WetnessHeight
		{
			set => Function.Call<float>(Hash.SET_PED_WETNESS_HEIGHT, Handle, value);
		}

		public void RandomizeOutfit()
		{
			Function.Call(Hash.SET_PED_RANDOM_COMPONENT_VARIATION, Handle, false);
		}

		/*public void SetDefaultClothes()
		{
			Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Handle);
		}*/

		public int Outfit
		{
			set => Function.Call((Hash)0x77FF8D35EEC6BBC4, Handle, value, 0);
		}

		#endregion

		#region Configuration


		public override int MaxHealth
		{
			get => Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Handle);
			set => Function.Call(Hash.SET_PED_MAX_HEALTH, Handle, value);
		}

		public bool IsPlayer => Function.Call<bool>(Hash.IS_PED_A_PLAYER, Handle);

		public bool GetConfigFlag(int flagID)
		{
			return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, flagID, true);
		}

		public void SetConfigFlag(int flagID, bool value)
		{
			Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, flagID, value);
		}

		public void ResetConfigFlag(int flagID)
		{
			Function.Call(Hash.SET_PED_RESET_FLAG, Handle, flagID, true);
		}

		public int GetBoneIndex(Bone BoneID)
		{
			return Function.Call<int>(Hash.GET_PED_BONE_INDEX, Handle, (int)BoneID);
		}

		public Vector3 GetBoneCoord(Bone BoneID)
		{
			return GetBoneCoord(BoneID, Vector3.Zero);
		}
		public Vector3 GetBoneCoord(Bone BoneID, Vector3 Offset)
		{
			return Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, Handle, (int)BoneID, Offset.X, Offset.Y, Offset.Z);
		}

		#endregion

		#region Tasks

		public bool IsIdle => !IsInjured && !IsRagdoll && !IsInAir && !IsOnFire && !IsGettingIntoAVehicle && !IsInCombat && !IsInMeleeCombat && !(IsInVehicle() && !IsSittingInVehicle());

		public bool IsProne => Function.Call<bool>(Hash.IS_PED_PRONE, Handle);

		public bool IsGettingUp => Function.Call<bool>(Hash.IS_PED_GETTING_UP, Handle);

		public bool IsDiving => Function.Call<bool>(Hash.IS_PED_DIVING, Handle);

		public bool IsJumping => Function.Call<bool>(Hash.IS_PED_JUMPING, Handle);

		public bool IsFalling => Function.Call<bool>(Hash.IS_PED_FALLING, Handle);

		public bool IsVaulting => Function.Call<bool>(Hash.IS_PED_VAULTING, Handle);

		public bool IsClimbing => Function.Call<bool>(Hash.IS_PED_CLIMBING, Handle);

		public bool IsWalking => Function.Call<bool>(Hash.IS_PED_WALKING, Handle);

		public bool IsRunning => Function.Call<bool>(Hash.IS_PED_RUNNING, Handle);

		public bool IsSprinting => Function.Call<bool>(Hash.IS_PED_SPRINTING, Handle);

		public bool IsStopped => Function.Call<bool>(Hash.IS_PED_STOPPED, Handle);

		public bool IsSwimming => Function.Call<bool>(Hash.IS_PED_SWIMMING, Handle);

		public bool IsSwimmingUnderWater => Function.Call<bool>(Hash.IS_PED_SWIMMING_UNDER_WATER, Handle);

		public bool IsOnMount => Function.Call<bool>(Hash.IS_PED_ON_MOUNT, Handle);


		public bool IsHeadtracking(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PED_HEADTRACKING_ENTITY, Handle, entity);
		}

		public bool AlwaysKeepTask
		{
			set => Function.Call(Hash.SET_PED_KEEP_TASK, Handle, value);
		}

		public bool BlockPermanentEvents
		{
			set => Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, Handle, value);
		}

		public Tasks Task => _tasks ?? (_tasks = new Tasks(this));

		public int TaskSequenceProgress => Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Handle);

		#endregion

		#region Euphoria & Ragdoll

		public bool IsRagdoll => Function.Call<bool>(Hash.IS_PED_RAGDOLL, Handle);

		public bool CanRagdoll
		{
			get => Function.Call<bool>(Hash.CAN_PED_RAGDOLL, Handle);
			set => Function.Call(Hash.SET_PED_CAN_RAGDOLL, Handle, value);
		}

		public Euphoria Euphoria => _euphoria ?? (_euphoria = new Euphoria(this));

		#endregion

		#region Weapon Interaction

		public int Accuracy
		{
			get => Function.Call<int>(Hash.GET_PED_ACCURACY, Handle);
			set => Function.Call(Hash.SET_PED_ACCURACY, Handle, value);
		}

		public int ShootRate
		{
			set => Function.Call(Hash.SET_PED_SHOOT_RATE, Handle, value);
		}

		/*public FiringPattern FiringPattern
		{
			set => Function.Call(Hash.SET_PED_FIRING_PATTERN, Handle, (int)value);
		}*/ // firing patterns

		public WeaponCollection Weapons => _weapons ??= new WeaponCollection(this);

		/*public bool CanSwitchWeapons
		{
			set => Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, Handle, value);
		}*/

		public void GiveWeapon(WeaponHash weapon, int ammoCount, bool equipNow = false, bool isLeftHanded = false, float condition = 0.0f)
		{
			Function.Call((Hash)0x5E3BDDBCB83F3D84, Handle, (uint)weapon, ammoCount, equipNow, true, 1, false,
				1056964608, 1065353216, isLeftHanded, condition);
		}

		#endregion

		#region Vehicle Interaction


		public bool IsOnFoot => Function.Call<bool>(Hash.IS_PED_ON_FOOT, Handle);

		public bool IsInTrain => Function.Call<bool>(Hash.IS_PED_IN_ANY_TRAIN, Handle);

		public bool IsInBoat => Function.Call<bool>(Hash.IS_PED_IN_ANY_BOAT, Handle);

		public bool IsGettingIntoAVehicle => Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, Handle);

		public bool IsOnHorse => Function.Call<bool>(Hash.IS_PED_ON_MOUNT, Handle);

		public Ped CurrentMount => Function.Call<Ped>(Hash.GET_MOUNT, Handle);

		public bool CanBeKnockedOffBike
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, value);
		}

		public bool IsInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, Handle, 0);
		}
		public bool IsInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Handle, vehicle.Handle, 0);
		}

		public bool IsSittingInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Handle);
		}
		public bool IsSittingInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Handle, vehicle.Handle);
		}

		public void SetIntoVehicle(Vehicle vehicle, int seat)
		{
			Function.Call(Hash.SET_PED_INTO_VEHICLE, Handle, vehicle.Handle, (int)seat);
		}

		public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_IN, Handle, true);

		public Vehicle CurrentVehicle => IsInVehicle() ? Function.Call<Vehicle>(Hash.GET_VEHICLE_PED_IS_IN, Handle, false) : null;


		#endregion

		#region Driving

		public float DrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_CRUISE_SPEED, Handle, value);
		}

		public float MaxDrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_MAX_CRUISE_SPEED, Handle, value);
		}
		#endregion

		#region Jacking

		public bool IsJacking => Function.Call<bool>(Hash.IS_PED_JACKING, Handle);

		public bool IsBeingJacked => Function.Call<bool>(Hash.IS_PED_BEING_JACKED, Handle);


		public Ped GetJacker()
		{
			return Function.Call<Ped>(Hash.GET_PEDS_JACKER, Handle);
		}

		public Ped GetJackTarget()
		{
			return Function.Call<Ped>(Hash.GET_JACK_TARGET, Handle);
		}

		#endregion

		#region Combat

		/*public bool IsEnemy
		{
			set => Function.Call(Hash.SET_PED_AS_ENEMY, Handle, value);
		}*/

		public bool IsPriorityTargetForEnemies
		{
			set => Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, Handle, value, 0);
		}

		public bool IsFleeing => Function.Call<bool>(Hash.IS_PED_FLEEING, Handle);

		public bool IsInjured => Function.Call<bool>(Hash.IS_PED_INJURED, Handle);

		public bool IsInCombat => Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle);

		public bool IsInMeleeCombat => Function.Call<bool>(Hash.IS_PED_IN_MELEE_COMBAT, Handle);

		public bool IsShooting => Function.Call<bool>(Hash.IS_PED_SHOOTING, Handle);

		public bool IsReloading => Function.Call<bool>(Hash.IS_PED_RELOADING, Handle);

		public bool IsGoingIntoCover => Function.Call<bool>(Hash.IS_PED_GOING_INTO_COVER, Handle);

		public bool IsAimingFromCover => Function.Call<bool>(Hash.IS_PED_AIMING_FROM_COVER, Handle);

		public bool IsBeingStunned => Function.Call<bool>(Hash.IS_PED_BEING_STUNNED, Handle);

		public bool IsBeingStealthKilled => Function.Call<bool>(Hash.IS_PED_BEING_STEALTH_KILLED, Handle);

		public bool IsInCover()
		{
			return IsInCover(false);
		}
		public bool IsInCover(bool expectUseWeapon)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_COVER, Handle, expectUseWeapon, 0);
		}

		public bool IsInCoverFacingLeft => Function.Call<bool>(Hash.IS_PED_IN_COVER_FACING_LEFT, Handle);

		public bool CanBeTargetted
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, Handle, value);
		}

		public Ped GetMeleeTarget()
		{
			return Function.Call<Ped>(Hash.GET_MELEE_TARGET_FOR_PED, Handle);
		}

		public bool IsInCombatAgainst(Ped target)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle, target);
		}

		#endregion

		#region Damaging

		/*public bool CanWrithe
		{
			get => !GetConfigFlag(281);
			set => SetConfigFlag(281, !value);
		}*/

		public bool CanSufferCriticalHits
		{
			set => Function.Call(Hash._0x34EDDD59364AD74A, Handle, value);
		}

		/*public bool AlwaysDiesOnLowHealth
		{
			set => Function.Call(Hash.SET_PED_DIES_WHEN_INJURED, Handle, value);
		}

		public bool DiesInstantlyInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_INSTANTLY_IN_WATER, Handle, value);
		}

		public bool DrownsInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_WATER, Handle, value);
		}

		public bool DrownsInSinkingVehicle
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_SINKING_VEHICLE, Handle, value);
		}*/

		public bool DropsWeaponsOnDeath
		{
			/*get
			{
				IntPtr address = RDR2DN.NativeMemory.GetEntityAddress(Handle);
				if (address == IntPtr.Zero)
					return false;

				int offset = (Game.Version >= GameVersion.VER_1_0_877_1_STEAM ? 0x13E5 : 0x13BD);
				offset = (Game.Version >= GameVersion.VER_1_0_944_2_STEAM ? 0x13F5 : offset);

				return RDR2DN.NativeMemory.IsBitSet(address + offset, 6);
			}*/
			set => Function.Call(Hash.SET_PED_DROPS_WEAPONS_WHEN_DEAD, Handle, value);
		}

		public void ApplyDamage(int damageAmount)
		{
			Function.Call(Hash.APPLY_DAMAGE_TO_PED, Handle, damageAmount, 0, 0, 0);
		}

		public Vector3 GetLastWeaponImpactCoords()
		{
			var outCoords = new OutputArgument();
			if (Function.Call<bool>(Hash.GET_PED_LAST_WEAPON_IMPACT_COORD, Handle, outCoords))
				return outCoords.GetResult<Vector3>();
			return Vector3.Zero;
		}

		#endregion

		#region Relationship

		public Relationship GetRelationshipWithPed(Ped ped)
		{
			return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, Handle, ped.Handle);
		}

		public int RelationshipGroup
		{
			get => Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Handle);
			set => Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, Handle, value);
		}

		#endregion

		#region Group

		public bool IsInGroup => Function.Call<bool>(Hash.IS_PED_IN_GROUP, Handle);

		public void LeaveGroup()
		{
			Function.Call(Hash.REMOVE_PED_FROM_GROUP, Handle);
		}

		/*public bool NeverLeavesGroup
		{
			set => Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, Handle, value);
		}*/

		public RelationshipGroup CurrentPedGroup => IsInGroup ? Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Handle) : 0;

		#endregion

		#region Speech & Animation

		public bool CanPlayGestures
		{
			set => Function.Call(Hash.SET_PED_CAN_PLAY_GESTURE_ANIMS, Handle, value, 0);
		}

		public string Voice
		{
			set => Function.Call(Hash.SET_AMBIENT_VOICE_NAME, Handle, value);
		}

		/*public string MovementAnimationSet
		{
			set
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, value);
				var endtime = DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, 1000);

				while (!Function.Call<bool>(Hash.HAS_ANIM_SET_LOADED, value))
				{
					Script.Yield();

					if (DateTime.UtcNow >= endtime)
					{
						return;
					}
				}

				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, value, 1.0f);
			}
		}*/

		#endregion

		#region Cores
		public int HealthCore
		{
			get => GetCoreValue(PedCore.Health);
			set => SetCoreValue(PedCore.Health, value);
		}

		public int HealthCoreRank
		{
			get => GetCoreRank(PedCore.Health);
			set => SetCoreRank(PedCore.Health, value);
		}

		public int StaminaCore
		{
			get => GetCoreValue(PedCore.Stamina);
			set => SetCoreValue(PedCore.Stamina, value);
		}

		public int StaminaCoreRank
		{
			get => GetCoreRank(PedCore.Stamina);
			set => SetCoreRank(PedCore.Stamina, value);
		}

		public int DeadEyeCore
		{
			get => GetCoreValue(PedCore.DeadEye);
			set => SetCoreValue(PedCore.DeadEye, value);
		}

		public int DeadEyeRank
		{
			get => GetCoreRank(PedCore.DeadEye);
			set => SetCoreRank(PedCore.DeadEye, value);
		}

		public int GetCoreValue(PedCore core)
		{
			return Function.Call<int>((Hash)0x36731AC041289BB1, Handle, (int)core);
		}

		public void SetCoreValue(PedCore core, int value)
		{
			Function.Call((Hash)0xc6258f41d86676e0, Handle, (int)core, value);
		}

		public int GetCoreRank(PedCore core)
		{
			return Function.Call<int>(Hash.GET_ATTRIBUTE_RANK, Handle, (int)core);
		}

		public void SetCoreRank(PedCore core, int level)
		{
			Function.Call(Hash.SET_ATTRIBUTE_POINTS, GetExperienceByRank(level));
		}

		private static int GetExperienceByRank(int level)
		{
			switch (level)
			{
				case -1:
					return -1;
				case 0:
					return 0;

				case 1:
					return 50;

				case 2:
					return 100;

				case 3:
					return 200;

				case 4:
					return 350;

				case 5:
					return 550;

				case 6:
					return 800;

				case 7:
					return 1100;

				default:
					return 0;
			}
		}
		#endregion
	}
	public enum PedCore
	{
		Health = 0,
		Stamina,
		DeadEye
	}
}
