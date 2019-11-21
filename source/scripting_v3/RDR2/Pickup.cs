//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public sealed class Pickup : PoolObject
	{
		public Pickup(int handle) : base(handle)
		{
			Handle = handle;
		}


		public Vector3 Position => Function.Call<Vector3>(Hash.GET_PICKUP_COORDS, Handle);

		public bool IsCollected => Function.Call<bool>(Hash.HAS_PICKUP_BEEN_COLLECTED, Handle);

		public bool ObjectExists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_OBJECT_EXIST, Handle);
		}

		public override void Delete()
		{
			Function.Call(Hash.REMOVE_PICKUP, Handle);
		}

		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_PICKUP_EXIST, Handle);
		}
		public static bool Exists(Pickup pickup)
		{
			return pickup != null && pickup.Exists();
		}

		public bool Equals(Pickup obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public sealed override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Pickup)obj);
		}

		public static bool operator ==(Pickup left, Pickup right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Pickup left, Pickup right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
