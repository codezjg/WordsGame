using UnityEngine;
using UnityEngine.UI;
using com.moralabs.cube.util;
using com.moralabs.cube.manager;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Image))]
public class Level : MonoBehaviour {
    public int ID;
    public int stars;
    public int levelNumber;
    private Sprite levelImage;
    private Color pageColor;
    private GameObject left, middle, right;

    private void Awake()
    {
        levelImage = gameObject.GetComponentInParent<LevelPage>().levelPageImage;
        pageColor = gameObject.GetComponentInParent<LevelPage>().pageColor;
        gameObject.GetComponent<Image>().sprite = levelImage;
        //gameObject.GetComponent<Image>().color = pageColor;

        //parent LevelPage_{Sayı} şeklinde olmak zorunda
        string[] tokens = transform.parent.name.Split('_');
        levelNumber = (int.Parse(tokens[1]) - 1) * 20 + ID;
        gameObject.GetComponentInChildren<Text>().text = levelNumber.ToString()+"\n";

        stars = PlayerPrefs.GetInt("LevelStars" + levelNumber.ToString(), -1);

        left = transform.GetChild(1).GetChild(0).gameObject;
        middle = transform.GetChild(1).GetChild(1).gameObject;
        right = transform.GetChild(1).GetChild(2).gameObject;

        if (stars < 1)
            left.GetComponent<Image>().SetTransparency(100f / 255f);
        else
            left.GetComponent<Image>().SetTransparency(255f);
        if (stars < 2)
            middle.GetComponent<Image>().SetTransparency(100f / 255f);
        else
            middle.GetComponent<Image>().SetTransparency(255f);
        if (stars < 3)
            right.GetComponent<Image>().SetTransparency(100f / 255f);
        else
            right.GetComponent<Image>().SetTransparency(255f);


    }

    public void OnClick()
    {
		//Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
        Manager.levelNumber = levelNumber;
        SceneManager.LoadScene(2);
    }
	
}
