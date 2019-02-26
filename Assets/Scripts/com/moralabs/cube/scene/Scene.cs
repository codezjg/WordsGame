using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.moralabs.cube.manager;

namespace com.moralabs.cube.scene
{
	public class Scene : MonoBehaviour
	{
        /// <summary>
        /// scene den extende ettikten sonra override ederek kullan ve base.Awake yap
        /// //bütün awakelerde bir şey istediğimiz zaman avantaj sağlıycak
        /// </summary>
		protected virtual void Awake() {
			Manager.Instance.Create();
		}

		protected virtual void Start() {
			//Manager.Instance.CurrentScene = this;
		}

		protected virtual void Update() {
		}

		protected virtual void OnEnable(){
		}

		protected virtual void OnDisable(){
		}

		public virtual void UpdateUI(){}

		public virtual void ProcessMessage(string action, ArrayList parameters){}

        //dashboard scene i scene den extend ettikten sonra switch in içerisine action stringini parametre olarak vererek
        //istediğin eventi yaptır
		public virtual void OnClick(string action){}
	}
}

