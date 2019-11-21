//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using RDR2.Math;
using RDR2.Native;

namespace RDR2
{
	public static class Audio
	{

		public static int PlaySoundFrontend(string soundFile)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, soundFile, 0, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}
		public static int PlaySoundFrontend(string soundFile, string soundSet)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, soundFile, soundSet, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}

		public static void StopSound(int id)
		{
			Function.Call(Hash._0x0F2A2175734926D8, id, 0);
		}
		public static void ReleaseSound(int id)
		{
			Function.Call(Hash.RELEASE_SOUND_ID, id);
		}

		public static void SetAudioFlag(string flag, bool toggle)
		{
			Function.Call(Hash.SET_AUDIO_FLAG, flag, toggle);
		}
		public static void SetAudioFlag(AudioFlag flag, bool toggle)
		{
			Function.Call(Hash.SET_AUDIO_FLAG, flag.ToString(), toggle);
		}
	}
}
