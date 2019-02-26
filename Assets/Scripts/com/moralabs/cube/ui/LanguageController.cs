using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Collections;

using com.moralabs.cube.util;

namespace com.moralabs.cube.ui{
	
public class LanguageController : MonoBehaviour {

        //level geçişlerinde level textlerinin hepsine LanguageController componeneti ekliyoruz
		public string languageTag = "";
		public string[] values;
		private bool languaged = false;

		public bool activate = true;

		// Use this for initialization
		void Start () {
			SetLanguage();
		}
		
		public void SetLanguage(){
			if (activate) {
				string resultText = languageTag;

				if(!languaged){
					resultText = Language.getText(languageTag, values);
				}else{
					if(values != null){
						for(int i = 0; i < values.Length; i++){
							resultText = resultText.Replace("["+(i+1)+"]", values[i]);
						}
					}
				}

				if(GetComponent<Text>() != null)
					GetComponent<Text>().text = resultText;	
				
			}
			
		}
        //GetComponenet<LanguageController>().ChangeValues(new string[]{"10"})
		// Update is called once per frame
		public void ChangeValues (string[] values) {
			this.values = values;

			SetLanguage();
		}

		// Update is called once per frame
		public void ChangeTag (string tag, bool languaged = false) {
			this.languageTag = tag;
			this.languaged = languaged;

			SetLanguage();
		}

		public void Change(string tag, string[] values, bool languaged = false){
			this.languageTag = tag;
			this.languaged = languaged;
			this.values = values;

			SetLanguage();
		}

	}
}