//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace RDR2
{
	public static class World
	{
		#region Fields
		static readonly string[] weatherNames = {
			"CLEAR",
			"CLOUDS",
			"SMOG",
			"FOGGY",
			"OVERCAST",
			"RAIN",
			"THUNDER",
			"CLEARING",
			"NEUTRAL",
			"SNOW",
			"SNOWLIGHT",
		};

		static readonly GregorianCalendar calendar = new GregorianCalendar();
		#endregion

		#region Time & Day

		public static DateTime CurrentDate
		{
			get
			{
				int year = Function.Call<int>(Hash.GET_CLOCK_YEAR);
				int month = Function.Call<int>(Hash.GET_CLOCK_MONTH);
				int day = System.Math.Min(Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH), calendar.GetDaysInMonth(year, month));
				int hour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int minute = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				int second = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

				return new DateTime(year, month, day, hour, minute, second);
			}
			set
			{
				Function.Call(Hash.SET_CLOCK_DATE, value.Year, value.Month, value.Day);
				Function.Call(Hash.SET_CLOCK_TIME, value.Hour, value.Minute, value.Second);
			}
		}

		public static TimeSpan CurrentDayTime
		{
			get
			{
				int hours = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int minutes = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				int seconds = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

				return new TimeSpan(hours, minutes, seconds);
			}
			set => Function.Call(Hash.SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
		}

		#endregion

		#region Weather & Effects


		/*public static Weather Weather
		{
			get
			{
				for (int i = 0; i < weatherNames.Length; i++)
				{
					if (Function.Call<int>(Hash._GET_CURRENT_WEATHER_TYPE) == Game.GenerateHash(weatherNames[i]))
					{
						return (Weather)i;
					}
				}

				return Weather.Unknown;
			}
			set
			{
				if (Enum.IsDefined(typeof(Weather), value) && value != Weather.Unknown)
				{
					Function.Call(Hash.SET_WEATHER_TYPE_NOW, weatherNames[(int)value]);
				}
			}
		}
		public static Weather NextWeather
		{
			get
			{
				for (int i = 0; i < weatherNames.Length; i++)
				{
					if (Function.Call<bool>(Hash.IS_NEXT_WEATHER_TYPE, weatherNames[i]))
					{
						return (Weather)i;
					}
				}

				return Weather.Unknown;
			}
			set
			{
				if (Enum.IsDefined(typeof(Weather), value) && value != Weather.Unknown)
				{
					int currentWeatherHash, nextWeatherHash;
					float weatherTransition;
					unsafe
					{
						Function.Call(Hash._GET_WEATHER_TYPE_TRANSITION, &currentWeatherHash, &nextWeatherHash, &weatherTransition);
					}
					Function.Call(Hash._SET_WEATHER_TYPE_TRANSITION, currentWeatherHash, Game.GenerateHash(weatherNames[(int)value]), 0.0f);
				}
			}
		}

		public static void TransitionToWeather(Weather value, float duration)
		{
			if (Enum.IsDefined(value.GetType(), value) && value != Weather.Unknown)
			{
				Function.Call(Hash._SET_WEATHER_TYPE_OVER_TIME, weatherNames[(int)value], duration);
			}
		}

		public static float WeatherTransition
		{
			get
			{
				int currentWeatherHash, nextWeatherHash;
				float weatherTransition;
				unsafe
				{
					Function.Call(Hash._GET_WEATHER_TYPE_TRANSITION, &currentWeatherHash, &nextWeatherHash, &weatherTransition);
				}
				return weatherTransition;
			}
			set => Function.Call(Hash._SET_WEATHER_TYPE_TRANSITION, 0, 0, value);
		}

		public static int GravityLevel
		{
			set => Function.Call(Hash.SET_GRAVITY_LEVEL, value);
		}*/

		#endregion

		#region Blips

		/*public static Vector3 GetWaypointPosition()
		{
			if (!Game.IsWaypointActive)
			{
				return Vector3.Zero;
			}

			bool blipFound = false;
			Vector3 position = Vector3.Zero;

			int it = Function.Call<int>(Hash._GET_BLIP_INFO_ID_ITERATOR);
			for (int i = Function.Call<int>(Hash.GET_FIRST_BLIP_INFO_ID, it); Function.Call<bool>(Hash.DOES_BLIP_EXIST, i); i = Function.Call<int>(Hash.GET_NEXT_BLIP_INFO_ID, it))
			{
				if (Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, i) == 4)
				{
					position = Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD, i);
					blipFound = true;
					break;
				}
			}

			if (blipFound)
			{
				bool groundFound = false;
				float height = 0.0f;

				for (int i = 800; i >= 0; i -= 50)
				{
					unsafe
					{
						if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, (float)i, &height))
						{
							groundFound = true;
							position.Z = height;
							break;
						}
					}

					Script.Wait(100);
				}

				if (!groundFound)
				{
					position.Z = 1000.0f;
				}
			}

			return position;
		}

		public static Blip CreateBlip(Vector3 position)
		{
			return Function.Call<Blip>(Hash.ADD_BLIP_FOR_COORD, position.X, position.Y, position.Z);
		}
		public static Blip CreateBlip(Vector3 position, float radius)
		{
			return Function.Call<Blip>(Hash.ADD_BLIP_FOR_RADIUS, position.X, position.Y, position.Z, radius);
		}*/

		#endregion

		#region Entities

		public static Ped[] GetAllPeds()
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPedHandles(), handle => new Ped(handle));
		}
		public static Ped[] GetAllPeds(Model model)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPedHandles(new[] { model.Hash }), handle => new Ped(handle));
		}
		public static Ped[] GetNearbyPeds(Ped ped, float radius)
		{
			int[] handles = RDR2DN.NativeMemory.GetPedHandles(ped.Position.ToArray(), radius);

			var result = new List<Ped>();

			foreach (int handle in handles)
			{
				if (handle == ped.Handle)
				{
					continue;
				}

				result.Add(new Ped(handle));
			}

			return result.ToArray();
		}
		public static Ped[] GetNearbyPeds(Vector3 position, float radius)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPedHandles(position.ToArray(), radius), handle => new Ped(handle));
		}
		public static Ped[] GetNearbyPeds(Vector3 position, float radius, Model model)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPedHandles(position.ToArray(), radius, new[] { model.Hash }), handle => new Ped(handle));
		}

		public static Vehicle[] GetAllVehicles()
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetVehicleHandles(), handle => new Vehicle(handle));
		}
		public static Vehicle[] GetAllVehicles(Model model)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetVehicleHandles(new[] { model.Hash }), handle => new Vehicle(handle));
		}
		public static Vehicle[] GetNearbyVehicles(Ped ped, float radius)
		{
			int[] handles = RDR2DN.NativeMemory.GetVehicleHandles(ped.Position.ToArray(), radius);

			var result = new List<Vehicle>();
			Vehicle ignore = ped.CurrentVehicle;
			int ignoreHandle = Vehicle.Exists(ignore) ? ignore.Handle : 0;

			foreach (int handle in handles)
			{
				if (handle == ignoreHandle)
				{
					continue;
				}

				result.Add(new Vehicle(handle));
			}

			return result.ToArray();
		}
		public static Vehicle[] GetNearbyVehicles(Vector3 position, float radius)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetVehicleHandles(position.ToArray(), radius), handle => new Vehicle(handle));
		}
		public static Vehicle[] GetNearbyVehicles(Vector3 position, float radius, Model model)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetVehicleHandles(position.ToArray(), radius, new[] { model.Hash }), handle => new Vehicle(handle));
		}

		public static Prop[] GetAllProps()
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPropHandles(), handle => new Prop(handle));
		}
		public static Prop[] GetAllProps(Model model)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPropHandles(new[] { model.Hash }), handle => new Prop(handle));
		}
		public static Prop[] GetNearbyProps(Vector3 position, float radius)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPropHandles(position.ToArray(), radius), handle => new Prop(handle));
		}
		public static Prop[] GetNearbyProps(Vector3 position, float radius, Model model)
		{
			return Array.ConvertAll(RDR2DN.NativeMemory.GetPropHandles(position.ToArray(), radius, new[] { model.Hash }), handle => new Prop(handle));
		}

		/*public static Blip[] GetActiveBlips()
		{
			List<Blip> res = new List<Blip>();

			foreach (BlipSprite sprite in Enum.GetValues(typeof(BlipSprite)))
			{
				int handle = Function.Call<int>(Hash.GET_FIRST_BLIP_INFO_ID, (int)sprite);

				while (Function.Call<bool>(Hash.DOES_BLIP_EXIST, handle))
				{
					res.Add(new Blip(handle));

					handle = Function.Call<int>(Hash.GET_NEXT_BLIP_INFO_ID, (int)sprite);
				}
			}

			return res.ToArray();
		}*/ // need blip natives

		public static Entity[] GetAllEntities()
		{
			return Array.ConvertAll<int, Entity>(RDR2DN.NativeMemory.GetEntityHandles(), Entity.FromHandle);
		}
		public static Entity[] GetNearbyEntities(Vector3 position, float radius)
		{
			return Array.ConvertAll<int, Entity>(RDR2DN.NativeMemory.GetEntityHandles(position.ToArray(), radius), Entity.FromHandle);
		}

		public static T GetClosest<T>(Vector3 position, params T[] spatials) where T : ISpatial
		{
			ISpatial closest = null;
			float closestDistance = 3e38f;

			foreach (var spatial in spatials)
			{
				float distance = position.DistanceToSquared(spatial.Position);

				if (distance <= closestDistance)
				{
					closest = spatial;
					closestDistance = distance;
				}
			}
			return (T)closest;
		}
		public static Ped GetClosestPed(Vector3 position, float radius)
		{
			Ped[] peds = Array.ConvertAll(RDR2DN.NativeMemory.GetPedHandles(position.ToArray(), radius), handle => new Ped(handle));
			return GetClosest(position, peds);
		}
		public static Vehicle GetClosestVehicle(Vector3 position, float radius)
		{
			Vehicle[] vehicles = Array.ConvertAll(RDR2DN.NativeMemory.GetVehicleHandles(position.ToArray(), radius), handle => new Vehicle(handle));
			return GetClosest(position, vehicles);

		}

		public static Ped CreatePed(Model model, Vector3 position)
		{
			return CreatePed(model, position, 0.0f);
		}
		public static Ped CreatePed(Model model, Vector3 position, float heading)
		{
			if (!model.IsPed || !model.Request(1000))
			{
				return null;
			}

			return Function.Call<Ped>(Hash.CREATE_PED, model.Hash, position.X, position.Y, position.Z, heading, false, false, 0);
        }

		#endregion

		#region Cameras

		public static void DestroyAllCameras()
		{
			Function.Call(Hash.DESTROY_ALL_CAMS, 0);
		}

		public static Camera CreateCamera(Vector3 position, Vector3 rotation, float fov)
		{
			return Function.Call<Camera>(Hash.CREATE_CAM_WITH_PARAMS, "DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, 1, 2);
		}

		public static Camera RenderingCamera
		{
			get => new Camera(Function.Call<int>(Hash.GET_RENDERING_CAM));
			set
			{
				if (value == null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, false, 0, 3000, 1, 0);
				}
				else
				{
					value.IsActive = true;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, true, 0, 3000, 1, 0);
				}
			}
		}

		#endregion

		#region Others


		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, Model model, int damage)
		{
			ShootBullet(sourcePosition, targetPosition, owner, model, damage, -1.0f);
		}
		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, Model model, int damage, float speed)
		{
			Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, 1, model.Hash, owner.Handle, 1, 0, speed);
		}

		public static void AddExplosion(Vector3 position, int type, float radius, float cameraShake)
		{
			Function.Call(Hash.ADD_EXPLOSION, position.X, position.Y, position.Z, (int)type, radius, true, false, cameraShake);
		}
		public static void AddExplosion(Vector3 position, int type, float radius, float cameraShake, bool Aubidble, bool Invis)
		{
			Function.Call(Hash.ADD_EXPLOSION, position.X, position.Y, position.Z, (int)type, radius, Aubidble, Invis, cameraShake);
		}
		public static void AddOwnedExplosion(Ped ped, Vector3 position, int type, float radius, float cameraShake)
		{
			Function.Call(Hash.ADD_OWNED_EXPLOSION, ped.Handle, position.X, position.Y, position.Z, (int)type, radius, true, false, cameraShake);
		}
		public static void AddOwnedExplosion(Ped ped, Vector3 position, int type, float radius, float cameraShake, bool Aubidble, bool Invis)
		{
			Function.Call(Hash.ADD_OWNED_EXPLOSION, ped.Handle, position.X, position.Y, position.Z, (int)type, radius, Aubidble, Invis, cameraShake);
		}

		public static int AddRelationshipGroup(string groupName)
		{
			int handle = 0;
			unsafe
			{
				Function.Call(Hash.ADD_RELATIONSHIP_GROUP, groupName, &handle);
			};
			return handle;
		}
		public static void RemoveRelationshipGroup(int group)
		{
			Function.Call(Hash.REMOVE_RELATIONSHIP_GROUP, group);
		}
		public static Relationship GetRelationshipBetweenGroups(int group1, int group2)
		{
			return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_GROUPS, group1, group2);
		}
		public static void SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
		{
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group1, group2);
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group2, group1);
		}
		public static void ClearRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
		{
			Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group1, group2);
			Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group2, group1);
		}

		#endregion

		#region Drawing

		
		public static void DrawLightWithRange(Vector3 position, Color color, float range, float intensity)
		{
			Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, position.X, position.Y, position.Z, color.R, color.G, color.B, range, intensity);
		}

		#endregion

		#region Raycasting

		/*public static RaycastResult Raycast(Vector3 source, Vector3 target, IntersectOptions options)
		{
			return Raycast(source, target, options, null);
		}
		public static RaycastResult Raycast(Vector3 source, Vector3 target, IntersectOptions options, Entity ignoreEntity)
		{
			return new RaycastResult(Function.Call<int>(Hash._CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}
		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectOptions options)
		{
			return Raycast(source, direction, maxDistance, options, null);
		}
		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectOptions options, Entity ignoreEntity)
		{
			Vector3 target = source + (direction * maxDistance);
			return new RaycastResult(Function.Call<int>(Hash._CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 target, float radius, IntersectOptions options)
		{
			return RaycastCapsule(source, target, radius, options, null);
		}
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 target, float radius, IntersectOptions options, Entity ignoreEntity)
		{
			return new RaycastResult(Function.Call<int>(Hash._CAST_3D_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, radius, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 direction, float maxDistance, float radius, IntersectOptions options)
		{
			return RaycastCapsule(source, direction, maxDistance, radius, options, null);
		}
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 direction, float maxDistance, float radius, IntersectOptions options, Entity ignoreEntity)
		{
			Vector3 target = source + (direction * maxDistance);
			return new RaycastResult(Function.Call<int>(Hash._CAST_3D_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, radius, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}

		public static RaycastResult GetCrosshairCoordinates()
		{
			return Raycast(GameplayCamera.Position, GameplayCamera.Direction, 1000.0f, IntersectOptions.Everything);
		}*/

		#endregion

		#region Positioning

		public static float GetDistance(Vector3 origin, Vector3 destination)
		{
			return Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 1);
		}
		/*public static float CalculateTravelDistance(Vector3 origin, Vector3 destination)
		{
			return Function.Call<float>(Hash.CALCULATE_TRAVEL_DISTANCE_BETWEEN_POINTS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z);
		}*/
		public static float GetGroundHeight(Vector2 position)
		{
			return GetGroundHeight(new Vector3(position.X, position.Y, 1000f));
		}
		public static float GetGroundHeight(Vector3 position)
		{
			float resultArg;

			unsafe
			{
				Function.Call(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &resultArg, false);
			}

			return resultArg;
		}

		public static Vector3 GetSafeCoordForPed(Vector3 position)
		{
			return GetSafeCoordForPed(position, true, 0);
		}
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk)
		{
			return GetSafeCoordForPed(position, sidewalk, 0);
		}
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk, int flags)
		{
			OutputArgument outPos = new OutputArgument();

			if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, sidewalk, outPos, flags))
			{
				return outPos.GetResult<Vector3>();
			}
			else
			{
				return Vector3.Zero;
			}
		}

		public static Vector3 GetNextPositionOnStreet(Vector3 position)
		{
			return GetNextPositionOnStreet(position, false);
		}
		public static Vector3 GetNextPositionOnStreet(Vector2 position, bool unoccupied)
		{
			return GetNextPositionOnStreet(new Vector3(position.X, position.Y, 0), unoccupied);
		}
		public static Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied)
		{
			OutputArgument outPos = new OutputArgument();

			if (unoccupied)
			{
				for (int i = 1; i < 40; i++)
				{
					Function.Call(Hash.GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, i, outPos, 1, 0x40400000, 0);
					Vector3 newPos = outPos.GetResult<Vector3>();
                    return newPos;
					/*if (!Function.Call<bool>(Hash.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
					{
						return newPos;
					}*/ // unnamed native
				}
			}
			else if (Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, 1, outPos, 1, 0x40400000, 0))
			{
				return outPos.GetResult<Vector3>();
			}

			return Vector3.Zero;
		}

		public static Vector3 GetNextPositionOnSidewalk(Vector2 position)
		{
			return GetNextPositionOnSidewalk(new Vector3(position.X, position.Y, 0));
		}
		public static Vector3 GetNextPositionOnSidewalk(Vector3 position)
		{
			OutputArgument outPos = new OutputArgument();

			if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, true, outPos, 0))
			{
				return outPos.GetResult<Vector3>();
			}
			else if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, false, outPos, 0))
			{
				return outPos.GetResult<Vector3>();
			}
			else
			{
				return Vector3.Zero;
			}
		}


		#endregion
	}
}
