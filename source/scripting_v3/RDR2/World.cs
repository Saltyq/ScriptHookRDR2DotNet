using RDR2.Math;
using RDR2.Native;
using System;
using System.Drawing;
using System.Globalization;
using System.Collections.Generic;

namespace RDR2
{
	public static class World
	{
		#region Fields
		static readonly GregorianCalendar calendar = new GregorianCalendar();
		#endregion

		#region Time & Day

		public static DateTime CurrentDate
		{
			get {
				int year = Function.Call<int>(Hash.GET_CLOCK_YEAR);
				int month = Function.Call<int>(Hash.GET_CLOCK_MONTH);
				int day = System.Math.Min(Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH), calendar.GetDaysInMonth(year, month));
				int hour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int minute = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				int second = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

				return new DateTime(year, month, day, hour, minute, second);
			}
			set {
				Function.Call(Hash.SET_CLOCK_DATE, value.Year, value.Month, value.Day);
				Function.Call(Hash.SET_CLOCK_TIME, value.Hour, value.Minute, value.Second);
			}
		}

		public static TimeSpan CurrentDayTime
		{
			get {
				int hours = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int minutes = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				int seconds = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

				return new TimeSpan(hours, minutes, seconds);
			}
			set => Function.Call(Hash.SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
		}

		#endregion

		#region Weather & Effects


		private static WeatherType _currentWeather;
		public static WeatherType CurrentWeather
		{
			get => GetCurrentWeatherType();
			set {
				_currentWeather = value;
				Function.Call(Hash._SET_WEATHER_TYPE_TRANSITION, (uint)GetCurrentWeatherType(), (uint)value, 1f);
			}
		}

		private static WeatherType _nextWeather;
		public static WeatherType NextWeather
		{
			get {
				GetCurrentWeatherType();
				return _nextWeather;
			}
		}

		private static WeatherType GetCurrentWeatherType()
		{
			var currentWeather = new OutputArgument();
			var nextWeather = new OutputArgument();
			var percent = new OutputArgument();
			Function.Call(Hash._GET_WEATHER_TYPE_TRANSITION, currentWeather, nextWeather, percent);
			_currentWeather = currentWeather.GetResult<WeatherType>();
			_nextWeather = nextWeather.GetResult<WeatherType>();
			var pct = percent.GetResult<float>();
			if (pct >= 0.5f)
			{
				return _nextWeather;
			}
			return _currentWeather;
		}


		public static float WeatherTransition
		{
			get {
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

		/*
		public static int GravityLevel
		{
			set => Function.Call(Hash.SET_GRAVITY_LEVEL, value);
		}*/

		#endregion

		#region Blips
		public static bool IsWaypointActive => Function.Call<bool>((Hash)0x202B1BBFC6AB5EE4);

		public static Vector3 WaypointPosition => Function.Call<Vector3>((Hash)0x29B30D07C3F7873B);

		public static Blip CreateBlip(Vector3 position, BlipType type)
		{
			var blip = Function.Call<int>((Hash)0x554D9D53F696D002, (uint)type, position.X, position.Y, position.Z);
			return new Blip(blip);
		}
		#endregion

		#region Entities


		public static Ped[] GetAllPeds()
		{
			int[] peds = new int[1024];
			int entityCount = RDR2DN.NativeMemory.getAllPeds(peds, 1024);
			List<Ped> Peds = new List<Ped>();
			for (int i = 0; i < entityCount; i++)
				Peds.Add(new Ped(peds[i]));

			return Peds.ToArray();
		}

		public static Vehicle[] GetAllVehicles()
		{
			int[] vehs = new int[1024];
			int entityCount = RDR2DN.NativeMemory.getAllVehicles(vehs, 1024);

			List<Vehicle> Vehs = new List<Vehicle>();
			for (int i = 0; i < entityCount; i++)
				Vehs.Add(new Vehicle(vehs[i]));

			return Vehs.ToArray();
		}

		public static Prop[] GetAllProps()
		{
			int[] props = new int[1024];
			int count = RDR2DN.NativeMemory.getAllObjects(props, 1024);

			List<Prop> Prop = new List<Prop>();
			for (int i = 0; i < count; i++)
				Prop.Add(new Prop(props[i]));

			return Prop.ToArray();
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
		/// <summary>
		/// This method is not fully tested. It uses a straight import from ScriptHookRDR2.dll, and if it is returning 0, then it is *probably* a SHRDR2 issue.
		/// </summary>
		public static Ped GetClosestPed(Vector3 position)
		{
			int[] peds = new int[1024];
			int entityCount = RDR2DN.NativeMemory.getAllPeds(peds, 1024);

			List<Ped> Peds = new List<Ped>();
			for (int i = 0; i < entityCount; i++)
				Peds.Add(new Ped(peds[i]));

			return GetClosest(position, Peds.ToArray());
		}
		/// <summary>
		/// This method is not fully tested. It uses a straight import from ScriptHookRDR2.dll, and if it is returning 0, then it is *probably* a SHRDR2 issue.
		/// </summary>
		public static Vehicle GetClosestVehicle(Vector3 position)
		{
			int[] vehs = new int[1024];
			int entityCount = RDR2DN.NativeMemory.getAllVehicles(vehs, 1024);

			List<Vehicle> Vehs = new List<Vehicle>();
			for (int i = 0; i < entityCount; i++)
				Vehs.Add(new Vehicle(vehs[i]));

			return GetClosest(position, Vehs.ToArray());
		}

				public static Prop GetClosestProp(Vector3 position)
		{
			int[] props = new int[1024];
			int count = RDR2DN.NativeMemory.getAllObjects(props, 1024);

			List<Prop> Prop = new List<Prop>();
			for (int i = 0; i < count; i++)
				Prop.Add(new Prop(props[i]));

			return GetClosest(position, Prop.ToArray());
		}
		
		public static Ped CreatePed(PedHash hash, Vector3 position, float heading = 0f, bool isNet = true, bool isMission = true)
		{
			var model = new Model(hash);
			if (!model.Request(4000))
			{
				return null;
			}
			var id = Function.Call<int>((Hash)0xD49F9B0955C367DE, (uint)hash, position.X, position.Y, position.Z, heading,
				isNet, !isMission, 0, 0);
			Function.Call((Hash)0x283978A15512B2FE, id, true);
			return id == 0 ? null : (Ped)Entity.FromHandle(id);
		}

		public static Pickup CreatePickup(PickupType type, Vector3 pos)
		{
			var handle = Function.Call<int>(Hash.CREATE_PICKUP, (int)type, pos.X, pos.Y, pos.Z, 0f, 0f, 0f, 32770, -1,
				2, 1, 0, 0);
			return new Pickup(handle);
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
			set {
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
		
		/// <summary>
		/// Draws a marker in the world, this needs to be done on a per frame basis
		/// </summary>
		/// <param name="type">The type of marker.</param>
		/// <param name="pos">The position of the marker.</param>
		/// <param name="dir">The direction the marker points in.</param>
		/// <param name="rot">The rotation of the marker.</param>
		/// <param name="scale">The amount to scale the marker by.</param>
		/// <param name="color">The color of the marker.</param>
		/// <param name="bobUpAndDown">if set to <c>true</c> the marker will bob up and down.</param>
		/// <param name="faceCamera">if set to <c>true</c> the marker will always face the camera, regardless of its rotation.</param>
		/// <param name="rotateY">if set to <c>true</c> rotates only on the y axis(heading).</param>
		/// <param name="textueDict">Name of texture dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="textureName">Name of texture inside the dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="drawOnEntity">if set to <c>true</c> draw on any <see cref="Entity"/> that intersects the marker.</param>
		public static void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color,
		 bool bobUpAndDown = false, bool faceCamera = false, bool rotateY = false, string textueDict = null, string textureName = null, bool drawOnEntity = false)
		{
			if (!string.IsNullOrEmpty(textueDict) && !string.IsNullOrEmpty(textureName))
			{
				Function.Call(Hash._DRAW_MARKER, (uint)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X,
				 scale.Y, scale.Z, color.R, color.G, color.B, color.A, bobUpAndDown, faceCamera, 2, rotateY, textueDict,
				 textureName, drawOnEntity);
			}
			else
			{
				Function.Call(Hash._DRAW_MARKER, (uint)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X,
				 scale.Y, scale.Z, color.R, color.G, color.B, color.A, bobUpAndDown, faceCamera, 2, rotateY, 0, 0, drawOnEntity);
			}
		}
		#endregion

		#region Raycasting
		public static RaycastResult Raycast(Vector3 source, Vector3 target, IntersectOptions options, Entity ignoreEntity = null)
		{
			return new RaycastResult(Function.Call<int>(Hash._START_SHAPE_TEST_RAY, source.X, source.Y, source.Z,
				target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}

		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDist, IntersectOptions options, Entity ignoreEntity = null)
		{
			var target = source + direction * maxDist;
			return new RaycastResult(Function.Call<int>(Hash._START_SHAPE_TEST_RAY, source.X, source.Y, source.Z,
				target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}

		public static RaycastResult CrosshairRaycast(float maxDist, IntersectOptions options, Entity ignoreEntity = null)
		{
			var source = GameplayCamera.Position;
			var rotation = (float)(System.Math.PI / 180.0) * GameplayCamera.Rotation;
			var forward = Vector3.Normalize(new Vector3(
				(float)-System.Math.Sin(rotation.Z) * (float)System.Math.Abs(System.Math.Cos(rotation.X)),
				(float)System.Math.Cos(rotation.Z) * (float)System.Math.Abs(System.Math.Cos(rotation.X)),
				(float)System.Math.Sin(rotation.X)));
			var target = source + forward * maxDist;
			return Raycast(source, target, options, ignoreEntity);
		}
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

	public enum WeatherType : uint
	{
		Overcast = 0xBB898D2D,
		Rain = 0x54A69840,
		Fog = 0xD61BDE01,
		Snowlight = 0x23FB812B,
		Thunder = 0xB677829F,
		Blizzard = 0x27EA2814,
		Snow = 0xEFB6EFF6,
		Misty = 0x5974E8E5,
		Sunny = 0x614A1F91,
		HighPressure = 0xF5A87B65,
		Clearing = 0x6DB1A50D,
		Sleet = 0xCA71D7C,
		Drizzle = 0x995C7F44,
		Shower = 0xE72679D5,
		SnowClearing = 0x641DFC11,
		OvercastDark = 0x19D4F1D9,
		Thunderstorm = 0x7C1C4A13,
		Sandstorm = 0xB17F6111,
		Hurricane = 0x320D0951,
		Hail = 0x75A9E268,
		Whiteout = 0x2B402288,
		GroundBlizzard = 0x7F622122
	}
}
