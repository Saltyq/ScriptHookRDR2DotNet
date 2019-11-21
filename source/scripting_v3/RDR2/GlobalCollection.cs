//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace RDR2
{
	public class GlobalCollection
	{
		internal GlobalCollection()
		{
		}

		public Global this[int index]
		{
			get => new Global(index);
			set
			{
				unsafe
				{
					*(ulong*)RDR2DN.NativeMemory.GetGlobalPtr(index).ToPointer() = *value.MemoryAddress;
				}
			}
		}
	}
}
