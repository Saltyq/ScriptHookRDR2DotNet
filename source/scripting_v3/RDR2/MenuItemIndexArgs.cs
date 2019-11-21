//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace RDR2
{
	public class MenuItemIndexArgs : EventArgs
	{
		public MenuItemIndexArgs(int index)
		{
			Index = index;
		}

		public int Index { get; }
	}
}
