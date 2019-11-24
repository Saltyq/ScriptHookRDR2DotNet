//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RDR2
{
	public static class Game
	{
		static Player cachedPlayer = null;


		public static float FPS => 1.0f / LastFrameTime;
		public static float LastFrameTime => Function.Call<float>(Hash.GET_FRAME_TIME);

		public static Size ScreenResolution
		{
			get
			{
				int w, h;
				unsafe { Function.Call(Hash.GET_SCREEN_RESOLUTION, &w, &h); }
				return new Size(w, h);
			}
		}

		public static Player Player
		{
			get
			{
				int handle = Function.Call<int>(Hash.PLAYER_ID);

				if (cachedPlayer == null || handle != cachedPlayer.Handle)
				{
					cachedPlayer = new Player(handle);
				}

				return cachedPlayer;
			}
		}

		public static Language Language => Function.Call<Language>(Hash.GET_CURRENT_LANGUAGE);

		public static GameVersion Version => (GameVersion)(RDR2DN.NativeMemory.GetGameVersion() + 1);

		public static GlobalCollection Globals { get; private set; } = new GlobalCollection();

		public static void Pause(bool value)
		{
			Function.Call(Hash.SET_GAME_PAUSED, value);
		}
		public static void PauseClock(bool value)
		{
			Function.Call(Hash.PAUSE_CLOCK, value, value);
		}

		public static bool IsPaused
		{
			get => Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);
			set => Function.Call(Hash.SET_GAME_PAUSED, value);
		}


		public static bool IsLoading => Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);
		public static bool IsScreenFadedIn => Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
		public static bool IsScreenFadedOut => Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
		public static bool IsScreenFadingIn => Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
		public static bool IsScreenFadingOut => Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);

		public static void FadeScreenIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		public static void FadeScreenOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}

		public static bool IsWaypointActive => Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE);


		

		public static int GameTime => Function.Call<int>(Hash.GET_GAME_TIMER);
		public static int FrameCount => Function.Call<int>(Hash.GET_FRAME_COUNT);

		public static float TimeScale
		{
			set => Function.Call(Hash.SET_TIME_SCALE, value);
		}

		public static int RadarZoom
		{
			set => Function.Call(Hash.SET_RADAR_ZOOM, value);
		}
		public static int MaxWantedLevel
		{
			get => Function.Call<int>(Hash.GET_MAX_WANTED_LEVEL);
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 5)
				{
					value = 5;
				}

				Function.Call(Hash.SET_MAX_WANTED_LEVEL, value);
			}
		}
		public static float WantedMultiplier
		{
			set => Function.Call(Hash.SET_WANTED_LEVEL_MULTIPLIER, value);
		}

		public static bool MissionFlag
		{
			get => Function.Call<bool>(Hash.GET_MISSION_FLAG);
			set => Function.Call(Hash.SET_MISSION_FLAG, value);
		}

		public static bool ShowsPoliceBlipsOnRadar
		{
			set => Function.Call(Hash.SET_POLICE_RADAR_BLIPS, value);
		}

		public static string GetUserInput(int maxLength)
		{
			return GetUserInput("", "", maxLength);
		}
		public static string GetUserInput(string windowTitle, int maxLength)
		{
			return GetUserInput(windowTitle, "", maxLength);
		}
		public static string GetUserInput(string windowTitle, string defaultText, int maxLength)
		{
			RDR2DN.ScriptDomain.CurrentDomain.PauseKeyEvents(true);

			Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, 0, windowTitle, "", defaultText, "", "", "", maxLength + 1);

			while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
			{
				Script.Yield();
			}

			RDR2DN.ScriptDomain.CurrentDomain.PauseKeyEvents(false);

			return Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
		}

		public static int GetControlValue(int index, Control control)
		{
			return Function.Call<int>(Hash.GET_CONTROL_VALUE, index, (int)control);
		}
		public static float GetControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_CONTROL_NORMAL, index, (int)control);
		}
		public static float GetDisabledControlNormal(int index, Control control)
		{
			return Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, index, (int)control);
		}

		public static void SetControlNormal(int index, Control control, float value)
		{
			Function.Call(Hash._SET_CONTROL_NORMAL, index, (int)control, value);
		}

		public static bool IsKeyPressed(Keys key)
		{
			return RDR2DN.ScriptDomain.CurrentDomain.IsKeyPressed(key);
		}
		public static bool IsControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, index, (int)control);
		}
		public static bool IsControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, index, (int)control);
		}
		public static bool IsControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, index, (int)control);
		}
		public static bool IsDisabledControlPressed(int index, Control control)
		{
			return IsControlPressed(index, control) && !IsControlEnabled(index, control);
		}
		public static bool IsDisabledControlJustPressed(int index, Control control)
		{
			return IsControlJustPressed(index, control) && !IsControlEnabled(index, control);
		}
		public static bool IsDisabledControlJustReleased(int index, Control control)
		{
			return IsControlJustReleased(index, control) && !IsControlEnabled(index, control);
		}
		public static bool IsEnabledControlPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_PRESSED, index, (int)control);
		}
		public static bool IsEnabledControlJustPressed(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, index, (int)control);
		}
		public static bool IsEnabledControlJustReleased(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_JUST_RELEASED, index, (int)control);
		}

		public static bool IsControlEnabled(int index, Control control)
		{
			return Function.Call<bool>(Hash.IS_CONTROL_ENABLED, index, (int)control);
		}
		
		public static void EnableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.ENABLE_CONTROL_ACTION, index, (int)control, true);
		}
	
		public static void DisableControlThisFrame(int index, Control control)
		{
			Function.Call(Hash.DISABLE_CONTROL_ACTION, index, (int)control, true);
		}

		public static void DisableAllControlsThisFrame(int index)
		{
			Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, index);
		}

		public static void PlayMusic(string musicFile)
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicFile);
		}
		public static void StopMusic(string musicFile)
		{
			Function.Call(Hash.CANCEL_MUSIC_EVENT, musicFile); // Needs a general Game.StopMusic()
		}

		public static int GenerateHash(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return 0;
			}

			return Function.Call<int>(Hash.GET_HASH_KEY, input);
		}

		public static string GetGXTEntry(string entry)
		{
			return Function.Call<string>(Hash._GET_LABEL_TEXT, entry);
		}
	}
}
