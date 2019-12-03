//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;
using System.Collections.Generic;

namespace RDR2
{
	public sealed class WeaponCollection
	{
		#region Fields
		readonly Ped owner;
		readonly Dictionary<uint, Weapon> weapons = new Dictionary<uint, Weapon>();
		#endregion

		internal WeaponCollection(Ped owner)
		{
			this.owner = owner;
		}

		public Weapon this[uint hash]
		{
			get
			{
				if (!weapons.TryGetValue(hash, out Weapon weapon))
				{
					if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)hash, true, 0))
					{
						return null;
					}

					weapon = new Weapon(owner, (WeaponHash)hash);
					weapons.Add(hash, weapon);
				}

				return weapon;
			}
		}

		public Weapon Current
		{
			get
			{
				int currentWeapon;
				unsafe
				{
					Function.Call(Hash.GET_CURRENT_PED_WEAPON, owner.Handle, &currentWeapon, true);
				}

				var hash = (uint)currentWeapon;

				if (weapons.ContainsKey(hash))
				{
					return weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, (WeaponHash)hash);
					weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public Weapon BestWeapon
		{
			get
			{
				uint hash = Function.Call<uint>(Hash.GET_BEST_PED_WEAPON, owner.Handle, 0, 0);

				if (weapons.ContainsKey(hash))
				{
					return weapons[hash];
				}
				else
				{
					var weapon = new Weapon(owner, (WeaponHash)hash);
					weapons.Add(hash, weapon);

					return weapon;
				}
			}
		}

		public bool HasWeapon(uint wHash)
		{
			return Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)wHash, 0 ,0);
		}

		public bool IsWeaponValid(uint hash)
		{
			return Function.Call<bool>(Hash.IS_WEAPON_VALID, (uint)hash);
		}

		public Prop CurrentWeaponObject
		{
			get
			{
				if (Current.Hash == Function.Call<WeaponHash>(Hash.GET_HASH_KEY, "WEAPON_UNARMED"))
				{
					return null;
				}

				return new Prop(Function.Call<int>(Hash.GET_CURRENT_PED_WEAPON_ENTITY_INDEX, owner.Handle, 0));
			}
		}

		public bool Select(Weapon weapon)
		{
			if (!weapon.IsPresent)
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, owner.Handle, (uint)weapon.Hash, true);

			return true;
		}
		public bool Select(uint wHash)
		{
			return Select(wHash, true);
		}
		public bool Select(uint wHash, bool equipNow)
		{
			if (!Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, owner.Handle, (uint)wHash))
			{
				return false;
			}

			Function.Call(Hash.SET_CURRENT_PED_WEAPON, owner.Handle, (uint)wHash, equipNow);

			return true;
		}

		public Weapon Give(uint hash, int ammoCount, bool equipNow, bool isAmmoLoaded)
		{
			if (!weapons.TryGetValue(hash, out Weapon weapon))
			{
				weapon = new Weapon(owner, (WeaponHash)hash);
				weapons.Add(hash, weapon);
			}

			if (weapon.IsPresent)
			{
				Select(weapon);
			}
			else
			{
				Function.Call(Hash.GIVE_DELAYED_WEAPON_TO_PED, owner.Handle, (uint)weapon.Hash, ammoCount);
			}

			return weapon;
		}


		public void Remove(Weapon weapon)
		{
			var hash = weapon.Hash;

			if (weapons.ContainsKey((uint)hash))
			{
				weapons.Remove((uint) hash);
			}

			Remove((uint)weapon.Hash);
		}
		public void Remove(uint wHash)
		{
			Function.Call(Hash.REMOVE_WEAPON_FROM_PED, owner.Handle, (uint)wHash, 0, 0);
		}

		public void RemoveAll()
		{
			Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, owner.Handle, true, 0);

			weapons.Clear();
		}
	}
}
