//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Native;

namespace RDR2.UI
{
	/// <summary>
	/// Methods to manage the display of notifications above the minimap.
	/// </summary>
	/*public static class Notification
	{
		#region Fields
		static readonly string[] iconNames = new string[] {
			
		};
		#endregion

		/// <summary>
		/// Creates a <see cref="Notification"/> above the minimap with the given message.
		/// </summary>
		/// <param name="message">The message in the notification.</param>
		/// <param name="blinking">if set to <c>true</c> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(string message, bool blinking = false)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, RDR2DN.NativeMemory.CellEmailBcon);
			RDR2DN.NativeFunc.PushLongString(message);
			return Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, blinking, true);
		}

		/// <summary>
		/// Creates a more advanced (SMS-alike) <see cref="Notification"/> above the minimap showing a sender icon, subject and the message.
		/// </summary>
		/// <param name="icon">The notification icon.</param>
		/// <param name="sender">The sender name.</param>
		/// <param name="subject">The subject line.</param>
		/// <param name="message">The message itself.</param>
		/// <param name="fadeIn">If <c>true</c> the message will fade in.</param>
		/// <param name="blinking">if set to <c>true</c> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(NotificationIcon icon, string sender, string subject, string message, bool fadeIn = false, bool blinking = false)
		{
			string iconName = iconNames[(int)icon];

			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, RDR2DN.NativeMemory.CellEmailBcon);
			RDR2DN.NativeFunc.PushLongString(message);
			Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, iconName, iconName, fadeIn, 1, sender, subject);

			return Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, blinking, true);
		}

		/// <summary>
		/// Hides a <see cref="Notification"/> instantly.
		/// </summary>
		/// <param name="handle">The handle of the <see cref="Notification"/> to hide.</param>
		public static void Hide(int handle)
		{
			Function.Call(Hash.THEFEED_REMOVE_ITEM, handle);
		}
	}*/
}
