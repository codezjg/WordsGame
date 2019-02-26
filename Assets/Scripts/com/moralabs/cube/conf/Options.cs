using UnityEngine;
using System.Collections.Generic;
using com.moralabs.cube.util;

namespace com.moralabs.cube.conf
{
	public class Options
	{
        //oyunla ilgili options ları yönetiyoruz ses için;
        //Options.Instance.SetOption(Options.SOUND, Options.ON)
		public static string ON = "on";
		public static string OFF = "off";

		public static string SOUND = "sound";
		public static string EFFECT = "effect";
		public static string NOTIFICATION = "notification";
		public static string LANGUAGE = "language";

		public static Dictionary<int, string> languages = new Dictionary<int, string>(){{0,"tr"}, {1,"en"}};

		private static Options _instance;

		public Options ()
		{
		}

		public static Options Instance
		{
			get
			{
				if(_instance == null) _instance = new Options();
				return _instance;	
			}
		}

		public int GetOption(string type, int def){
			return CPlayerPrefs.GetInt(type, def);
		}

		public string GetOption(string type, string def){
			return CPlayerPrefs.GetString(type, def);
		}

		public void SetOption(string type, int option){
			CPlayerPrefs.SetInt(type, option);
		}

		public void SetOption(string type, string option){
			CPlayerPrefs.SetString(type, option);
		}
	}
}

