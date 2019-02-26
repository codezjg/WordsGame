using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.moralabs.cube.manager;
using com.moralabs.cube.util;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour {
    public GameObject scrollSnap;
    public Button[] levelButtons;

    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("LastLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached){
                levelButtons[i].interactable = false;
                levelButtons[i].GetComponent<Image>().SetTransparency(150f / 255f);
                levelButtons[i].GetComponentInChildren<Text>().color = Color.red;
            }
                

        }
    }

    private void Awake()
    {
        Manager.lastPage = (Manager.levelNumber-1)/20;
        scrollSnap.GetComponent<ScrollSnapRect>().startingPage = Manager.lastPage;
    }
}
