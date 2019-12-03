//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;
using System.Drawing;

namespace RDR2
{
	public sealed class Player : INativeValue
	{
		#region Fields
		Ped ped;

        /*struct StatIDArgs
        {
            long BaseId;
            long PermutationId;
        }*/ // for money, will fix later
		
        #endregion

        public Player(int handle)
		{
			Handle = handle;
		}

		public int Handle
		{
			get;
			private set;
		}

		public ulong NativeValue
		{
			get => (ulong)Handle;
			set => Handle = unchecked((int)value);
		}

		public Ped Character
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_PLAYER_PED, Handle);

				if (ped == null || handle != ped.Handle)
				{
					ped = new Ped(handle);
				}

				return ped;
			}
		}

		public string Name => Function.Call<string>(Hash.GET_PLAYER_NAME, Handle);

		public static int Money
		{
			get => Function.Call<int>((Hash)0x0C02DABFA3B98176);
			set {
				var source = Money;
				var target = value;
				if (target < source)
				{
					Function.Call((Hash)0x466BC8769CF26A7A, source - target);
				}
				else
				{
					Function.Call((Hash)0xBC3422DC91667621, target - source);
				}
			}
		}

		public int WantedLevel
		{
			get => Function.Call<int>(Hash.GET_PLAYER_WANTED_LEVEL, Handle);
			set
			{
				Function.Call(Hash.SET_PLAYER_WANTED_LEVEL, Handle, value, false);
			}
		}

		public bool IsDead => Function.Call<bool>(Hash.IS_PLAYER_DEAD, Handle);

		public bool IsAlive => !IsDead;

		public bool IsAiming => Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING, Handle);

		public bool IsClimbing => Function.Call<bool>(Hash.IS_PLAYER_CLIMBING, Handle);

		public bool IsRidingTrain => Function.Call<bool>(Hash.IS_PLAYER_RIDING_TRAIN, Handle);


		public bool IsPlaying => Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Handle);

		public bool IsInvincible
		{
			get => Function.Call<bool>(Hash.GET_PLAYER_INVINCIBLE, Handle);
			set => Function.Call(Hash.SET_PLAYER_INVINCIBLE, Handle, value);
		}

		public bool IgnoredByEveryone
		{
			set => Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Handle, value);
		}

		public bool CanUseCover
		{
			set => Function.Call(Hash.SET_PLAYER_CAN_USE_COVER, Handle, value);
		}

		public bool CanStartMission
		{
			get => Function.Call<bool>(Hash.CAN_PLAYER_START_MISSION, Handle);
		}

		public bool CanControlCharacter
		{
			get => Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Handle);
			set => Function.Call(Hash.SET_PLAYER_CONTROL, Handle, value, 0, 0);
		}

		public bool ChangeModel(Model model)
		{
			if (!model.IsInCdImage || !model.IsPed || !model.Request(1000))
			{
				return false;
			}

			Function.Call(Hash.SET_PLAYER_MODEL, Handle, model.Hash);
			model.MarkAsNoLongerNeeded();
			return true;
		}


		public Vehicle LastVehicle => Function.Call<Vehicle>(Hash.GET_PLAYERS_LAST_VEHICLE);

		public bool IsTargetting(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity.Handle, 0);
		}

		public bool IsTargettingAnything => Function.Call<bool>(Hash.IS_PLAYER_TARGETTING_ANYTHING, Handle);


		public void DisableFiringThisFrame(bool toggle)
		{
			Function.Call(Hash.DISABLE_PLAYER_FIRING, Handle, toggle);
		}

		public void SetSuperJumpThisFrame()
		{
			Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Handle);
		}

		public void SetMayNotEnterAnyVehicleThisFrame()
		{
			Function.Call(Hash.SET_PLAYER_MAY_NOT_ENTER_ANY_VEHICLE, Handle);
		}

		public void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle)
		{
			Function.Call(Hash.SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle);
		}

		public bool Exists()
		{
			// IHandleable forces us to implement this unfortunately,
			// so we'll implement it explicitly and return true
			return true;
		}

		public bool Equals(Player obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Player)obj);
		}

		public static bool operator ==(Player left, Player right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Player left, Player right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
