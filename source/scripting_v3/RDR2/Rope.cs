//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;

namespace RDR2
{
	public sealed class Rope : PoolObject
	{
		public Rope(int handle) : base(handle)
		{
			Handle = handle;
		}


		public int VertexCount => Function.Call<int>(Hash.GET_ROPE_VERTEX_COUNT, Handle);

		public float Length
		{
           // get => Function.Call(Hash._0x3D69537039F8D824, Handle);
			set => Function.Call(Hash.ROPE_FORCE_LENGTH, Handle, value);
		}

		public void ActivatePhysics()
		{
			Function.Call(Hash.ACTIVATE_PHYSICS, Handle);
		}

		public void DetachEntity(Entity entity)
		{
			Function.Call(Hash.DETACH_ROPE_FROM_ENTITY, Handle, entity.Handle);
		}

		public void AttachEntities(Entity entityOne, Entity entityTwo, float length)
		{
			AttachEntities(entityOne, Vector3.Zero, entityTwo, Vector3.Zero, length);
		}
		public void AttachEntities(Entity entityOne, Vector3 positionOne, Entity entityTwo, Vector3 positionTwo, float length)
		{
			Function.Call(Hash.ATTACH_ENTITIES_TO_ROPE, Handle, entityOne.Handle, entityTwo.Handle, positionOne.X, positionOne.Y, positionOne.Z, positionTwo.X, positionTwo.Y, positionTwo.Z, length, 0, 0, 0, 0);
		}

		public Vector3 GetVertexCoord(int vertex)
		{
			return Function.Call<Vector3>(Hash.GET_ROPE_VERTEX_COORD, Handle, vertex);
		}

		public override void Delete()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.DELETE_ROPE, &handle);
			}
		}

		public override bool Exists()
		{
			int handle = Handle;
			unsafe
			{
				return Function.Call<bool>(Hash.DOES_ROPE_EXIST, &handle);
			}
		}
		public static bool Exists(Rope rope)
		{
			return rope != null && rope.Exists();
		}

		public bool Equals(Rope obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Rope)obj);
		}

		public static bool operator ==(Rope left, Rope right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Rope left, Rope right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
