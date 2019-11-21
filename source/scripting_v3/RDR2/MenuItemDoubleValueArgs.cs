//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;

namespace RDR2
{
	public class MenuItemDoubleValueArgs : EventArgs
	{
		public MenuItemDoubleValueArgs(double value)
		{
			Index = value;
		}

		public double Index { get; }
	}
}
