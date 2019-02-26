using UnityEngine;
using System.Collections;
using com.moralabs.cube.manager;
using com.moralabs.cube.popup;

namespace com.moralabs.cube.animation{

	public class PopupAnimationBehaviour : StateMachineBehaviour {

		public Popup popup;

		// This will be called once the animator has transitioned out of the state.
		override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "SettingsClose")
				//Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
			popup.OnStateExit(animator, stateInfo, layerIndex);
		}
	}
}