using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using com.moralabs.cube.conf;
using System;

namespace com.moralabs.cube.manager
{
	public enum Sounds{
		BGM,
		WIN
	}

	public class SoundManager
	{
		private GameObject gameObject;

		private AudioSource sounds;

	
		private Dictionary<Sounds, AudioClip> clips = new Dictionary<Sounds, AudioClip>();
		private List<AudioSource> effects = new List<AudioSource>();

		private Dictionary<Sounds, AudioClip> effectList = new Dictionary<Sounds, AudioClip>();

		private bool isCurrentSoundLoop = false;

		public SoundManager (GameObject gameObject)
		{
			AudioClip[] cs = Resources.LoadAll<AudioClip> ("Audio/Music");
			AudioClip[] es = Resources.LoadAll<AudioClip> ("Audio/Effect");

			for (int i = 0; i < cs.Length; i++) {
                //clips.Add(Sounds.BGM, cs[i]);
                clips.Add((Sounds)Enum.Parse(typeof(Sounds), cs[i].name), cs[i]);
			}

			for (int i = 0; i < es.Length; i++) {
				effectList.Add((Sounds)Enum.Parse(typeof(Sounds), es[i].name), es[i]);
			}

			sounds = gameObject.AddComponent<AudioSource>();

			this.gameObject = gameObject;
		}

		public void PlaySound (Sounds type, bool isLoop = false)
		{
			if(Options.Instance.GetOption(Options.SOUND, Options.ON) == Options.ON)
			{
				sounds.Stop();
				sounds.clip = clips[type];
				sounds.loop = isLoop;
				sounds.Play ();

				isCurrentSoundLoop = isLoop;
			}
		}

		public void PlayEffect (Sounds type)
		{
			if(Options.Instance.GetOption(Options.EFFECT, Options.ON) == Options.ON)
			{
				bool wasNull = false;
				AudioSource currentSource = null;
				for(int i = 0; i < effects.Count; i++)
				{
					if(!effects[i].isPlaying)
					{
						currentSource = effects[i];
					}
				}

				if(currentSource == null)
				{
					wasNull = true;
					currentSource = gameObject.AddComponent<AudioSource>();
				}
				
				currentSource.Stop();
				currentSource.clip = effectList[type];
				currentSource.loop = false;
				currentSource.Play ();

				if(wasNull) effects.Add(currentSource);
			}
		}

		public void StopSounds ()
		{
			sounds.Stop();
		}

		public void StopEffects ()
		{
			for(int i = 0; i < effects.Count; i++)
			{
				effects[i].Stop();
			}
		}

		public void PlaySounds ()
		{
			if(isCurrentSoundLoop)
			{
				sounds.Play();
			}
		}
	}
}