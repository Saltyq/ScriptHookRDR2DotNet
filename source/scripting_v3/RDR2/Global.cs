//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Text;
using System.Runtime.InteropServices;
using RDR2.Math;

namespace RDR2
{
	public unsafe struct Global
	{
		readonly IntPtr address;

		internal Global(int index)
		{
			address = RDR2DN.NativeMemory.GetGlobalPtr(index);
		}

		public unsafe ulong* MemoryAddress => (ulong*)address.ToPointer();

		public unsafe void SetInt(int value)
		{
			RDR2DN.NativeMemory.WriteInt32(address, value);
		}
		public unsafe void SetFloat(float value)
		{
			RDR2DN.NativeMemory.WriteFloat(address, value);
		}
		public unsafe void SetString(string value)
		{
			int size = Encoding.UTF8.GetByteCount(value);
			Marshal.Copy(Encoding.UTF8.GetBytes(value), 0, address, size);
			*((byte*)MemoryAddress + size) = 0;
		}
		public unsafe void SetVector3(Vector3 value)
		{
			RDR2DN.NativeMemory.WriteVector3(address, value.ToArray());
		}

		public unsafe int GetInt()
		{
			return RDR2DN.NativeMemory.ReadInt32(address);
		}
		public unsafe float GetFloat()
		{
			return RDR2DN.NativeMemory.ReadFloat(address);
		}
		public unsafe string GetString()
		{
			return RDR2DN.NativeMemory.PtrToStringUTF8(address);
		}
		public unsafe Vector3 GetVector3()
		{
			var data = RDR2DN.NativeMemory.ReadVector3(address);
			return new Vector3(data[0], data[1], data[2]);
		}
	}
}
