//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RDR2
{
    public sealed class Vehicle : Entity
    {
        public Vehicle(int handle) : base(handle)
        {
        }

        public void Repair()
        {
            Function.Call(Hash.SET_VEHICLE_FIXED, Handle);
        }


        #region Styling

        public bool IsExtraOn(int extra)
        {
            return Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Handle, extra);
        }

        public bool ExtraExists(int extra)
        {
            return Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Handle, extra);
        }

        public void ToggleExtra(int extra, bool toggle)
        {
            Function.Call(Hash.SET_VEHICLE_EXTRA, Handle, extra, !toggle);
        }

        #endregion

        #region Configuration

        public bool IsStolen
        {
            set => Function.Call(Hash.SET_VEHICLE_IS_STOLEN, Handle, value);
        }

        /*public bool IsWanted
		{
			set => Function.Call(Hash.SET_VEHICLE_IS_WANTED, Handle, value);
		}*/


        #endregion

        #region Health

        public float BodyHealth
        {
            get => Function.Call<float>(Hash.GET_VEHICLE_BODY_HEALTH, Handle);
            set => Function.Call(Hash.SET_VEHICLE_BODY_HEALTH, Handle, value);
        }

        #endregion

        #region Performance & Driving


        public float Speed
        {
            get => Function.Call<float>(Hash.GET_ENTITY_SPEED, Handle);
            set
            {
                if (Model.IsTrain)
                {
                    Function.Call(Hash.SET_TRAIN_SPEED, Handle, value);
                    Function.Call(Hash.SET_TRAIN_CRUISE_SPEED, Handle, value);
                }
                else
                {
                    Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, Handle, value);
                }
            }
        }

        #endregion

        #region Damaging

        public bool IsDamaged
        {
            get
            {
                int health = Function.Call<int>(Hash.GET_ENTITY_HEALTH, Handle);
				int maxHealth = Function.Call<int>(Hash.GET_ENTITY_MAX_HEALTH, Handle, 0);

				if (health < maxHealth)
                {
                    return true;
                }
				if (health > maxHealth)
                {
                    return false;
                }
                return false;
            }
		}
		public bool IsDriveable
		{
			get => Function.Call<bool>(Hash.IS_VEHICLE_DRIVEABLE, Handle, 1, 0);
			set => Function.Call(Hash.SET_VEHICLE_UNDRIVEABLE, Handle, !value);
		}

		public bool IsAxlesStrong
		{
			set => Function.Call<bool>(Hash.SET_VEHICLE_HAS_STRONG_AXLES, Handle, value);
		}

		public bool CanWheelsBreak
		{
			set => Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, Handle, value);
		}

		public bool CanBeVisiblyDamaged
		{
			set => Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, Handle, value);
		}


		public void ApplyDamage(Vector3 loc, float damageAmount, float radius)
		{
			Function.Call(Hash.SET_VEHICLE_DAMAGE, loc.X, loc.Y, loc.Z, damageAmount, radius, true);
		}

		#endregion

		#region Occupants

		public Ped Driver => GetPedOnSeat(-1);

		public Ped GetPedOnSeat(int seat)
		{
			return Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, Handle, (int)(seat));
		}

		public Ped[] Occupants
		{
			get
			{
				Ped driver = Driver;

				int arraySize = Entity.Exists(driver) ? PassengerCount + 1 : PassengerCount;
				Ped[] occupantsArray = new Ped[arraySize];
				int occupantIndex = 0;

				if (arraySize == 0)
				{
					return occupantsArray;
				}

				if (Entity.Exists(driver))
				{
					occupantsArray[0] = driver;
					++occupantIndex;
				}

				for (int i = 0, seats = PassengerSeats; i < seats; i++)
				{
					Ped ped = GetPedOnSeat((int)i);

					if (!Entity.Exists(ped))
					{
						continue;
					}

					occupantsArray[occupantIndex] = ped;
					++occupantIndex;

					if (occupantIndex >= arraySize)
					{
						return occupantsArray;
					}
				}

				return occupantsArray;
			}
		}

		public Ped[] Passengers
		{
			get
			{
				var passengersArray = new Ped[PassengerCount];
				int passengerIndex = 0;

				if (passengersArray.Length == 0)
				{
					return passengersArray;
				}

				for (int i = 0, seats = PassengerSeats; i < seats; i++)
				{
					Ped ped = GetPedOnSeat((int)i);

					if (!Entity.Exists(ped))
					{
						continue;
					}

					passengersArray[passengerIndex] = ped;
					++passengerIndex;

					if (passengerIndex >= passengersArray.Length)
					{
						return passengersArray;
					}
				}

				return passengersArray;
			}
		}

		public int PassengerCount => Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, Handle);

		public int PassengerSeats => Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, Handle);

		public Ped CreatePedOnSeat(int seat, RDR2.Model model)
		{
			if (!model.IsPed || !model.Request(1000))
			{
				return null;
			}

			return Function.Call<Ped>(Hash.CREATE_PED_INSIDE_VEHICLE, Handle, 26, model.Hash, (int)seat, 1, 1);
		}

		
		public bool IsSeatFree(int seat)
		{
			return Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, Handle, (int)seat);
		}

		#endregion

		#region Positioning

		public bool IsStopped => Function.Call<bool>(Hash.IS_VEHICLE_STOPPED, Handle);

		public bool IsOnAllWheels => Function.Call<bool>(Hash.IS_VEHICLE_ON_ALL_WHEELS, Handle);

		public bool PlaceOnGround()
		{
			return Function.Call<bool>(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, Handle, 0);
		}

		public void PlaceOnNextStreet()
		{
			Vector3 pos = Position;
			OutputArgument outPos = new OutputArgument();

			for (int i = 1; i < 40; i++)
			{
				float heading;
				float val;
				unsafe
				{
					Function.Call(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, pos.X, pos.Y, pos.Z, i, outPos, &heading, &val, 1, 0x40400000, 0);
				}
				Vector3 newPos = outPos.GetResult<Vector3>();

				if (1 == 1)
				{
					Position = newPos;
					PlaceOnGround();
					Heading = heading;
					break;
				}
			}
		}

		public bool ProvidesCover
		{
			set => Function.Call(Hash.SET_VEHICLE_PROVIDES_COVER, Handle, value);
		}

		#endregion

	}
}
