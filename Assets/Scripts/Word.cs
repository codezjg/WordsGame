using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Word : MonoBehaviour {

    public Sprite borderCircle, normalCircle;
    public bool isActive, isCollide;

    private GameObject insWord;
    private Transform canvGame;
    private LevelGenerate mLevelGenerate;
    private Transform posText;
    private TextMeshProUGUI textWord;
    private Rigidbody2D mRigidbody;

    void Start()
    {
        canvGame = GameObject.Find("GameCanvas").transform;
        mLevelGenerate = FindObjectOfType<LevelGenerate>();

        mRigidbody = GetComponent<Rigidbody2D>();

        posText = transform.GetChild(0);
        textWord = posText.gameObject.GetComponent<TextMeshProUGUI>();
        textWord.color = gameObject.GetComponent<Image>().color;

        isActive = false;
        isCollide = false;
    }

    void FixedUpdate()
    {
        mRigidbody.AddForce(new Vector2(Input.acceleration.x, Input.acceleration.y) * 500, ForceMode2D.Impulse);

        Vector3 rot = transform.localEulerAngles;
        rot.z = ClampAngle(rot.z, -60f, 60f);

        transform.localEulerAngles = rot;

    }

    float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    public void Clicked(){

        if (!isActive && LevelGenerate.selectedWordNumber < 5)
        {
            if (mLevelGenerate.lerpScale && mLevelGenerate.lerpMove)
            {
                isActive = true; 
                gameObject.transform.SetAsLastSibling();
                gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                gameObject.GetComponent<Image>().sprite = borderCircle;

                mLevelGenerate.InstantiateWord(gameObject);
                LevelGenerate.selectedWordNumber++;
            }else{
                Debug.Log("girmedi 1");
            }


        }
        else if(isActive)
        {
            if (mLevelGenerate.lerpScale && mLevelGenerate.lerpMove)
            {
                isActive = false; LevelGenerate.selectedWordNumber--;
                gameObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                gameObject.GetComponent<Image>().sprite = normalCircle;

                mLevelGenerate.DestroyWord(textWord.text);
            }else{
                Debug.Log("girmedi 2");
            }
                

        }
    }


}
