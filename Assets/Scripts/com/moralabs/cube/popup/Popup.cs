using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.moralabs.cube.conf;
using com.moralabs.cube.manager;
using com.moralabs.cube.animation;

namespace com.moralabs.cube.popup
{
	public abstract class Popup : MonoBehaviour
	{
        //SettingsPopup class ı oluşturup Popup dan extend et
		protected virtual void Awake() {
		}

		protected virtual void Start() {
		}

		protected virtual void Update() {
		}

		protected virtual void OnEnable(){
		}

		protected virtual void OnDisable(){
		}

		// Overriden
		public virtual void UpdatePopup(ArrayList parameters){}
		public virtual void ProcessInternalMessage(string action, ArrayList parameters = null){}
		public virtual void Clicked(string action){

			if(action == ButtonConfig.POPUP_CLOSE){
				Manager.Instance.Popup.Close();
			}
		}

		public virtual void Opened(){
            
            if(GetComponent<Animator>() != null && GetComponent<Animator>().GetBehaviour<PopupAnimationBehaviour>()){
                GetComponent<Animator>().GetBehaviour<PopupAnimationBehaviour>().popup = this;
                GetComponent<Animator>().SetTrigger("opened");
            }
		}

		public virtual void Close(){
			if(GetComponent<Animator>() != null && !GetComponent<Animator>().name.Contains("ChestOpenPopup")){
				try{
					GetComponent<Animator>().SetTrigger("closed");
				}catch(System.Exception ex){
					Debug.Log("Popup.Close() Exception : "+ex.Message);
					gameObject.SetActive(false);
				}
			}else{
				gameObject.SetActive(false);
			}
		}

        public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex){
			if(animatorStateInfo.IsName("PopupClose")){
				gameObject.SetActive(false);
			}
		}
	}
}

