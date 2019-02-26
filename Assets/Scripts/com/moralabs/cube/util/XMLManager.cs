using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System;

namespace com.moralabs.cube.util
{
	public class XMLManager
	{
		public const string LEVEL = "Level";

		private static Dictionary<string, XmlDocument>  docs = new Dictionary<string, XmlDocument>();

		public static void createXML(string type){
			if(!docs.ContainsKey(type)){
				XmlDocument doc = new XmlDocument ();
				TextAsset textFile = Resources.Load<TextAsset>("XML/"+type);
				doc.LoadXml(textFile.text);
				docs.Add(type, doc);
			}
		}

		public static long GetProperty(string type, string element, int id = -1, string property = null)
		{
			try{
				return long.Parse(GetPropertyString(type, element, id, property));
			}catch(System.Exception ex){
				Debug.LogWarning("XMLManager.GetProperty Exception2 : "+ex);
			}

			return long.MinValue;
		}
			
		public static string GetPropertyString(string type, string element, int id = -1, string property = null)
		{
			createXML(type);

			XmlNode node = null;
			XmlNodeList list;

			try{
				list = docs[type].GetElementsByTagName(element);

				for(int i = 0; i < list.Count; i++){
					XmlAttributeCollection col = list.Item(i).Attributes;

					XmlNode idNode = col.GetNamedItem("id");

					if(int.Parse(idNode.Value) == id){
						node = list.Item(i);
					}
				}
			}catch(System.Exception ex){
				Debug.LogWarning("XMLManager.GetPropertyString Exception1 : "+ex);
			}

			if(node == null) return ""; // No Building

			try{
				list = node.ChildNodes;

				for(int i = 0; i < list.Count; i++){
					if(list.Item(i).Name == property){
						return list.Item(i).InnerXml;
					}
				}
			}catch(System.Exception ex){
				Debug.LogError("XMLManager.GetPropertyString Exception2 : "+ex);
			}

			return "";
		}
	}

}