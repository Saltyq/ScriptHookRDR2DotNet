using System;
using RDR2.Native;

namespace RDR2.UI
{
	public sealed class Prompt : PoolObject, IEquatable<Prompt>
	{
		private string _text;

		public string Text
		{
			get => _text;
			set {
				_text = value;
				var strPtr = Function.Call<string>((Hash)0xFA925AC00EB830B9, 10, "LITERAL_STRING", value);
				Function.Call(Hash._UIPROMPT_SET_TEXT, Handle, strPtr);
			}
		}

		public bool IsVisible
		{
			get => Function.Call<bool>(Hash._UIPROMPT_IS_ACTIVE, Handle);
			set => Function.Call(Hash._UIPROMPT_SET_VISIBLE, Handle, value);
		}

		public int HoldTime
		{
			set => Function.Call(Hash._UIPROMPT_SET_PRESSED_TIMED_MODE, value);
		}

		public bool IsPressed => Function.Call<bool>(Hash._UIPROMPT_IS_PRESSED, Handle);
		public bool IsJustPressed => Function.Call<bool>(Hash._UIPROMPT_IS_JUST_PRESSED, Handle);
		public bool IsReleased => Function.Call<bool>(Hash._UIPROMPT_IS_RELEASED, Handle);
		public bool IsJustReleased => Function.Call<bool>(Hash._UIPROMPT_IS_JUST_RELEASED, Handle);

		public Control Control
		{
			set => Function.Call(Hash._UIPROMPT_SET_CONTROL_ACTION, (uint)value);
		}

		public int Priority
		{
			set => Function.Call(Hash._UIPROMPT_SET_PRIORITY, Handle, value);
		}

		public bool IsPulsating
		{
			get => Function.Call<bool>(Hash._UIPROMPT_GET_URGENT_PULSING_ENABLED, Handle);
			set => Function.Call(Hash._UIPROMPT_SET_URGENT_PULSING_ENABLED, Handle, value);
		}

		public Prompt(int handle) : base(handle)
		{

		}

		public bool Equals(Prompt other)
		{
			return !ReferenceEquals(null, other) && other.Handle == Handle;
		}

		public override bool Exists()
		{
			return Function.Call<bool>(Hash._UIPROMPT_IS_VALID, Handle);
		}

		public override void Delete()
		{
			Function.Call(Hash._UIPROMPT_DELETE, Handle);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Prompt)obj);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
