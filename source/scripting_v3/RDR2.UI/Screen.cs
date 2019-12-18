//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RDR2.UI
{
	/// <summary>
	/// Methods to handle UI actions that affect the whole screen.
	/// </summary>
	public static class Screen
	{
		#region Fields
		private static readonly string[] _effects = new string[] {
			
		};
		#endregion

		// Dimensions

		/// <summary>
		/// The base width of the screen used for all UI Calculations, unless ScaledDraw is used
		/// </summary>
		public const float Width = 1920f;
		/// <summary>
		/// The base height of the screen used for all UI Calculations
		/// </summary>
		public const float Height = 1080f;

		public static bool IsCinematicModeEnabled
		{
			set => Function.Call(Hash.SET_CINEMATIC_BUTTON_ACTIVE, value);
		}

		public static bool IsCinematicModeActive
		{
			set => Function.Call(Hash.SET_CINEMATIC_MODE_ACTIVE, value);
		}

		/// <summary>
		/// Gets the actual screen resolution the game is being rendered at
		/// </summary>
		public static Size Resolution
		{
			get
			{
				int width, height;
				unsafe
				{
					Function.Call(Hash.GET_SCREEN_RESOLUTION, &width, &height);
				}

				return new Size(width, height);
			}
		}
		/// <summary>
		/// Gets the current screen aspect ratio
		/// </summary>
		public static float AspectRatio => 1.0f;
		/// <summary>
		/// Gets the screen width scaled against a 720pixel height base.
		/// </summary>
		public static float ScaledWidth => Height * 1.0f;

		// Fading

		/// <summary>
		/// Gets a value indicating whether the screen is faded in.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is faded in; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadedIn => Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
		/// <summary>
		/// Gets a value indicating whether the screen is faded out.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is faded out; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadedOut => Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
		/// <summary>
		/// Gets a value indicating whether the screen is fading in.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is fading in; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadingIn => Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
		/// <summary>
		/// Gets a value indicating whether the screen is fading out.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is fading out; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadingOut => Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);

		/// <summary>
		/// Fades the screen in over a specific time, useful for transitioning
		/// </summary>
		/// <param name="time">The time for the fade in to take</param>
		public static void FadeIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		/// <summary>
		/// Fades the screen out over a specific time, useful for transitioning
		/// </summary>
		/// <param name="time">The time for the fade out to take</param>
		public static void FadeOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}

		// Screen Effects

		/// <summary>
		/// Gets a value indicating whether the specific screen effect is running.
		/// </summary>
		/// <param name="effectName">The <see cref="ScreenEffect"/> to check.</param>
		/// <returns><c>true</c> if the screen effect is active; otherwise, <c>false</c>.</returns>
		public static bool IsEffectActive(ScreenEffect effectName)
		{
			return Function.Call<bool>(Hash.ANIMPOSTFX_IS_RUNNING, _effects[(int)effectName]);
		}

		/// <summary>
		/// Stops all currently running effects.
		/// </summary>
		public static void StopEffects()
		{
			Function.Call(Hash.ANIMPOSTFX_STOP_ALL);
		}

		// Text

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time
		/// </summary>
		/// <param name="message">The message to display.</param>
		public static void ShowSubtitle(string message)
		{
			try
			{
				string varString = Function.Call<string>(Hash.CREATE_STRING, 10, "LITERAL_STRING", message);
				Function.Call(Hash._UILOG_SET_CACHED_OBJECTIVE, varString);
				Function.Call(Hash._UILOG_PRINT_CACHED_OBJECTIVE);
				Function.Call(Hash._UILOG_CLEAR_CACHED_OBJECTIVE);
			}
			catch (Exception ex)
			{
				RDR2DN.Log.Message(RDR2DN.Log.Level.Error, ex.ToString());
			}
		}
		// Space Conversion

		/// <summary>
		/// Translates a point in WorldSpace to its given Coordinates on the <see cref="Screen"/>
		/// </summary>
		/// <param name="position">The position in the World.</param>
		/// <param name="scaleWidth">if set to <c>true</c> Returns the screen position scaled by <see cref="ScaledWidth"/>; otherwise, returns the screen position scaled by <see cref="Width"/>.</param>
		/// <returns></returns>
		public static PointF WorldToScreen(Vector3 position, bool scaleWidth = false)
		{
			float pointX, pointY;

			unsafe
			{
				if (!Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, position.X, position.Y, position.Z, &pointX, &pointY))
				{
					return PointF.Empty;
				}
			}

			pointX *= scaleWidth ? ScaledWidth : Width;
			pointY *= Height;

			return new PointF(pointX, pointY);
		}
	}
}
