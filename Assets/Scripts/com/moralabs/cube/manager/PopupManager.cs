using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.moralabs.cube.ui;
using com.moralabs.cube.popup;
using UnityEngine.UI;

namespace com.moralabs.cube.manager
{
	public class PopupManager
	{
		public const string NO = "No";

		private Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();
		private ArrayList openedPopups = new ArrayList();

		private String activePopup = NO;

		private GameObject popupLayer;
		private GameObject popupBackground;
		
		public PopupManager ()
		{
			Init();
		}

		public void Init()
		{
			popupLayer = GameObject.Find("PopupLayer");
			popupBackground = GameObject.Find("PopupBackground");			
			if(popupBackground != null) popupBackground.SetActive(false);

			while(openedPopups.Count > 0)
			{
				openedPopups.RemoveAt(0);
			}

			foreach(KeyValuePair<string, GameObject> entry in popups)
			{
				MonoBehaviour.DestroyImmediate(entry.Value);
			}
            

			popups.Clear();
			activePopup = NO;
		}
        	
		public void Open(string type, ArrayList parameters = null)
		{
			popupBackground.SetActive(true);

			if(openedPopups.IndexOf(type) != -1){
				openedPopups.Remove(type);
			}

			openedPopups.Add(type);

			if(!popups.ContainsKey(type) || popups[type] == null){
                popups[type] = GameObject.Instantiate(Resources.Load<GameObject>("Popup/" + type), Vector3.zero, Quaternion.identity) as GameObject;
				popups[type].transform.SetParent(popupLayer.transform, false);
				popups[type].transform.localPosition = new Vector3(0,0,0);
			}

            if(activePopup != NO) popups[activePopup].GetComponent<Popup>().Close();	

			popups[type].GetComponent<Popup>().UpdatePopup(parameters);
			popups[type].SetActive(true);

            //Manager.Instance.Sound.PlayEffect (Sounds.POPUP_OPEN);

            popups[type].GetComponent<Popup>().Opened();
			
            //en öne popup u taşı çizilmesi için
			popups[type].transform.SetAsLastSibling();

			activePopup = type;
		}

		public bool Close()
		{
			if(activePopup == NO) return false;

			try{
				popups[activePopup].GetComponent<Popup>().Close();
			}catch(System.Exception ex){
				Debug.Log("PopupController.Close() Exception : "+ex);

				popups[activePopup].SetActive(false);
			}

			openedPopups.RemoveAt(openedPopups.Count - 1);

			activePopup = openedPopups.Count == 0 ? NO : openedPopups [openedPopups.Count - 1] as String;

			if(activePopup == NO)
			{
				popupBackground.SetActive(false);
			}

			return true;
		}
			
		public void ProcessInternalMessage (string type, string action, ArrayList parameters = null)
		{
			try{
				popups[type].GetComponent<Popup>().ProcessInternalMessage(action, parameters);
			} catch (Exception e){
				Debug.Log("ProcessInternalMessage Error: " + e.ToString());
			}
		}

		public void CloseAll()
		{
			while(Close());
		}

		public bool IsOpen(string type)
		{
			if(activePopup.Equals(type))
			{
				return true;
			}
			return false;
		}

		public bool IsOpened(string type)
		{
			if(popups.ContainsKey(type))
			{
				return true;
			}
			return false;
		}

		public bool IsAnyPopupOpened()
		{
			if(activePopup != NO) return true;

			return false;
		}
	}
}