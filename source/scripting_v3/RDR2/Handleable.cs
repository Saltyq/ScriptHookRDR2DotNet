//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;

namespace RDR2
{
	/*public interface ISpatial
	{
		Vector3 Position
		{
			get; set;
		}
		Vector3 Rotation
		{
			get; set;
		}
	}*/

	public interface IHandleable
	{
		int Handle { get; }

		bool Exists();
	}
}
