//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public struct Model : IEquatable<Model>, INativeValue
	{
		public Model(int hash)
		{
			Hash = hash;
		}
		public Model(string name) : this(Game.GenerateHash(name))
		{
		}
		public Model(uint hash) : this((int)hash)
		{
		}
		public Model(PedHash hash) : this((int)hash)
		{
		}

		public ulong NativeValue
		{
			get => (ulong)Hash;
			set => Hash = unchecked((int)value);
		}

		public int Hash
		{
			get; private set;
		}

		public bool IsValid => Function.Call<bool>(Native.Hash.IS_MODEL_VALID, Hash);
		public bool IsInCdImage => Function.Call<bool>(Native.Hash.IS_MODEL_IN_CDIMAGE, Hash);

		public bool IsLoaded => Function.Call<bool>(Native.Hash.HAS_MODEL_LOADED, Hash);
		public bool IsCollisionLoaded => Function.Call<bool>(Native.Hash.HAS_COLLISION_FOR_MODEL_LOADED, Hash);

		public bool IsBoat => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_BOAT, Hash);
        public bool IsPed => Function.Call<bool>(Native.Hash.IS_MODEL_A_PED, Hash);
		public bool IsTrain => Function.Call<bool>(Native.Hash.IS_THIS_MODEL_A_TRAIN, Hash);
		public bool IsVehicle => Function.Call<bool>(Native.Hash.IS_MODEL_A_VEHICLE, Hash);

		public void GetDimensions(out Vector3 minimum, out Vector3 maximum)
		{
			var outmin = new OutputArgument();
			var outmax = new OutputArgument();
			Function.Call(Native.Hash.GET_MODEL_DIMENSIONS, Hash, outmin, outmax);
			minimum = outmin.GetResult<Vector3>();
			maximum = outmax.GetResult<Vector3>();
		}
		public Vector3 GetDimensions()
		{
			GetDimensions(out Vector3 min, out Vector3 max);
			return Vector3.Subtract(max, min);
		}

		public void Request()
		{
            if (!Function.Call<bool>(Native.Hash.IS_MODEL_VALID, Hash))
            {
                return;
            }
            Function.Call(Native.Hash.REQUEST_MODEL, Hash);
		}
		public bool Request(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();

				if (DateTime.UtcNow >= endtime)
					return false;
			}

			return true;
		}

		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Native.Hash.SET_MODEL_AS_NO_LONGER_NEEDED, Hash);
		}

		public bool Equals(Model obj)
		{
			return Hash == obj.Hash;
		}
		public override bool Equals(object obj)
		{
			return obj != null && obj.GetType() == GetType() && Equals((Model)obj);
		}

		public static bool operator ==(Model left, Model right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Model left, Model right)
		{
			return !left.Equals(right);
		}

		public static implicit operator int(Model source)
		{
			return source.Hash;
		}
		public static implicit operator PedHash(Model source)
		{
			return (PedHash)source.Hash;
		}
		public static implicit operator uint(Model source)
		{
			return (uint)source.Hash;
		}

		public static implicit operator Model(int source)
		{
			return new Model(source);
		}
		public static implicit operator Model(string source)
		{
			return new Model(source);
		}
		public static implicit operator Model(PedHash source)
		{
			return new Model(source);
		}
		public static implicit operator Model(uint source)
		{
			return new Model(source);
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return "0x" + Hash.ToString("X");
		}
	}
}
