using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Xml;
using System.Collections;

using com.moralabs.cube.conf;
using com.moralabs.cube.ui;

namespace com.moralabs.cube.util
{
	public class Language
	{
		private static XmlDocument doc = null;

		private static string lang;

		public static string getText(string text, params string[] values){
			try{
				createXML();
				if(string.IsNullOrEmpty(text)) return "";

				XmlNodeList list = doc.GetElementsByTagName(text);
				string returnText = list[0].InnerText;

				if(string.IsNullOrEmpty(returnText)) return text;

				if(values != null){
					for(int i = 0; i < values.Length; i++){
						returnText = returnText.Replace("["+(i+1)+"]", values[i]);
					}
				}
				return returnText;
			} catch (Exception e){
				Debug.Log("Language.getText Text Error : " + text + " - " + e);
				return text;
			}
		}

		public static void createXML(){
			SelectLanguage ();
			if(doc == null || lang != Config.LANGUAGE){
				doc = new XmlDocument ();
				TextAsset textFile = Resources.Load<TextAsset>("XML/Language/Language_"+Config.LANGUAGE);
				doc.LoadXml(textFile.text);

				lang = Config.LANGUAGE;
			}
		}

		public static void SelectLanguage(){
			if (Config.LANGUAGE.Length == 0) {
				string temp = CPlayerPrefs.GetString ("LANG", "");

				if (temp.Length == 0) {

					temp = Config.LANGUAGES [0];

					for (int i = 1; i < Config.LANGUAGES.Length; i++) {
						if (Config.LANGUAGES [i] == GetSystemLanguage ()) {
							temp = Config.LANGUAGES [i];
							break;
						}
					}
				}

				Config.LANGUAGE = temp;
			}
		}

		public static void SetLanguage(string lang){
			for (int i = 0; i < Config.LANGUAGES.Length; i++) {
				if (Config.LANGUAGES [i] == lang)
					break;
				if (i == Config.LANGUAGES.Length - 1)
					return;
			}

			Config.LANGUAGE = lang;
			Options.Instance.SetOption(Options.LANGUAGE, lang);

			LanguageController[] conts = GameObject.FindObjectsOfType<LanguageController>();

			for (int i = 0; i < conts.Length; i++) {
				conts [i].SetLanguage ();
			}
		}

		public static string GetSystemLanguage(){
			switch (Application.systemLanguage) {
			case SystemLanguage.Turkish:
				return "tr";
			}

			return "en";
		}
	}
}

