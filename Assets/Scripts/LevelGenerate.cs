﻿using System; using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; using UnityEngine.EventSystems; using TMPro; using com.moralabs.cube.manager; using com.moralabs.cube.util; using com.moralabs.cube.popup; using com.moralabs.cube.conf;  public static class Extensions {     public static void SetTransparency(this UnityEngine.UI.Image p_image, float p_transparency)     {         if (p_image != null)         {             UnityEngine.Color __alpha = p_image.color;             __alpha.a = p_transparency;             p_image.color = __alpha;         }     } }  public class LevelGenerate : MonoBehaviour {      public Camera mainCamera;     public GameObject prefWord;     public List<GameObject> arrWord;     public GameObject[] botArrWord;     public Sprite normalCircle;     public TextMeshProUGUI gameThemeText, textHint, tutorialText;     public Image tutorialHand;     public Animator mAnimator;     public static GameObject tipObj;     public bool lerpScale = true, lerpMove = true;      [HideInInspector]     public static int selectedWordNumber;     public static int levelNum;      private Transform canvGame;     private string[] requestedLine, lines;     private static string[] tempWordList;     private int beforeRand = -1, totalSubWords=0, totalWords=0, previousLevel, wordFound;     private static int hintIndex = 0, wordListCount;     private List<string> wordColors, subWordList, wordList;     private string gameTheme;     private Transform posText;     private TextMeshProUGUI textWord;     private bool checkNewWord = false, getHint = false, gameStart = false;     private Color myColor;     private static int levelCounterForAds;     private int oldHintNumber;     private float timeInLevel, timeInLevelCurrent, timeDiff;     private string tutorialTextList;       void Start () {         wordColors = new List<string>();         wordList = new List<string>();         subWordList = new List<string>();         botArrWord = new GameObject[6];         myColor = new Color();         canvGame = GameObject.Find("GameCanvas").transform; 
        levelNum = Manager.levelNumber;         if (levelNum < 1) levelNum = 1;          previousLevel = levelNum-1;         levelCounterForAds = 0;         textHint.text = CPlayerPrefs.GetInt("LastHintCount").ToString();         UpdatePaidText();     }      void Update () {         if(levelNum != previousLevel){             previousLevel = levelNum;             ClearLists();             mAnimator.Play("BarIdle");             AdjustBackgroundColor();             if(levelNum > PlayerPrefs.GetInt("LastLevel", 1))                 PlayerPrefs.SetInt("LastLevel", levelNum);             GetLevelText(levelNum, "Levels" + Manager.gameLang);             tempWordList = new string[wordList.Count];             wordList.CopyTo(tempWordList);             StartCoroutine(CreateWord(wordList));             gameThemeText.text = gameTheme;             wordListCount = wordList.Count;             timeInLevelCurrent = 0;             timeInLevel = 30f * subWordList.Count * Time.fixedTime;             gameStart = true;         }          if(gameStart){
            timeInLevelCurrent += Time.fixedTime * 0.06f;             timeDiff = timeInLevel - timeInLevelCurrent;         }      }      public void Reset()
    {
        if (tutorialHand.IsActive() || tutorialText.IsActive()){             tutorialText.gameObject.SetActive(false);             tutorialHand.gameObject.SetActive(false);         }
        ClearLists();         mAnimator.Play("BarIdle");
        GetLevelText(levelNum, "Levels" + Manager.gameLang);         tempWordList = new string[wordList.Count];         wordList.CopyTo(tempWordList);         StartCoroutine(CreateWord(wordList));         gameThemeText.text = gameTheme;         wordListCount = wordList.Count;         timeInLevelCurrent = 0;         timeInLevel = 60f * subWordList.Count * Time.fixedTime;         gameStart = true;
    }

    public static void SetTipActive()     {         tipObj = GameObject.Find("HintButton");         tipObj.GetComponent<Image>().SetTransparency(255f);     }      public static void SetTipDeactive()     {         tipObj = GameObject.Find("HintButton");         tipObj.GetComponent<Image>().SetTransparency(100f / 255f);     }      public void UpdatePaidText(){         textHint.text = CPlayerPrefs.GetInt("LastHintCount", 5).ToString();     }      IEnumerator CreateWord(List<string> wordList)     {         CreateSubWords(wordList);          //oluşacak top sayısı text den gelecek veya statik bir değer olacak         for (int i = 0; i < totalSubWords; i++)         {             arrWord.Add(Instantiate(prefWord, new Vector2(Screen.width/2 + UnityEngine.Random.Range(-60, 60), gameObject.transform.position.y + UnityEngine.Random.Range(-100, 150)), Quaternion.identity, canvGame));             float tempScale = UnityEngine.Random.Range(0.7f, 1.1f);             posText = arrWord[i].transform.GetChild(0);             textWord = posText.gameObject.GetComponent<TextMeshProUGUI>(); textWord.text = subWordList[i];             arrWord[i].transform.localScale = new Vector3(tempScale, tempScale, 1);             AdjustColor(arrWord[i]);             yield return new WaitForSeconds(0.18f);         }      }      IEnumerator LerpScale(float time, GameObject wordObj, Vector3 originalScale, Vector3 targetScale, bool shrink, bool directPass)     {         if(lerpScale || directPass){             lerpScale = false;             directPass = false;
            float originalTime = time;             int tempSelected = selectedWordNumber; tempSelected++;              wordObj.transform.localScale = new Vector3(0, 0, 0);              while (time > 0.0f)             {                 if (wordObj == null)                     break;                 time -= Time.deltaTime;                 wordObj.transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);                 if (wordObj.transform.localScale == targetScale)                     lerpScale = true;                  yield return null;             }               if (shrink && wordObj.transform.localScale == targetScale)             {                 Destroy(wordObj);                 ConcatCheck(true);             }               if (wordObj != null && !shrink && checkNewWord && wordObj.transform.localScale == targetScale)             {                 checkNewWord = false;                 ConcatCheck(false);                 lerpScale = true;             }         }                  }      IEnumerator LerpMove(float time, GameObject wordObj, bool right, bool down, bool directPass)     {         if(lerpMove || directPass){             lerpMove = false;             directPass = false;
            var originalPosition = wordObj.transform.position;             Vector3 targetPosition = right                 ? originalPosition + new Vector3(Screen.width / 12f, 0f, 0f)                 : originalPosition - new Vector3(Screen.width / 12f, 0f, 0f);             float originalTime = time;              if (down){                 lerpMove = true;                 targetPosition = originalPosition + new Vector3(0, -Screen.height / 4, 0f);             }                               while (time > 0.0f && wordObj != null)             {                 time -= Time.deltaTime;                 wordObj.transform.position = Vector3.Lerp(targetPosition, originalPosition, time / originalTime);                 if (wordObj.transform.position == targetPosition)                     lerpMove = true;                  yield return null;             }         } 
     }      private void ClearLists(){         if (wordColors != null)             wordColors.Clear();         if (wordList != null)             wordList.Clear();         if (subWordList != null)             subWordList.Clear();         if (arrWord != null)             arrWord.Clear();          totalSubWords = 0; totalWords = 0;         wordFound = 0; selectedWordNumber = 0;         gameTheme = "";     }      private void CreateSubWords(List<string> wordList){          int subCounter = 0;          for (int i = 0; i < wordList.Count; i++){              subCounter = 0;             int tempLength = wordList[i].Length;              if (tempLength <= 3){                 //tek hece                 subWordList.Add(wordList[i]);                 totalSubWords++;             }else if(tempLength == 4){                 //iki-iki böüecek                 while(subCounter != tempLength){                     subWordList.Add(wordList[i].Remove(subCounter, 2));                     subCounter += 2;                     totalSubWords++;                 }             }else if(tempLength % 2 == 0 && tempLength <= 10){                 //çft sayı ise                 while(subCounter != wordList[i].Length){                     if(tempLength % 3 == 0 && tempLength % 2 == 0){                         int randDivider = UnityEngine.Random.Range(2, 4);                         subWordList.Add(wordList[i].Substring(subCounter, randDivider));                         subCounter += randDivider;                         tempLength -= randDivider;                         totalSubWords++;                     } else if(tempLength % 3 == 0){                         subWordList.Add(wordList[i].Substring(subCounter, 3));                         subCounter += 3;                         tempLength -= 3;                         totalSubWords++;                     } else{                         subWordList.Add(wordList[i].Substring(subCounter, 2));                         subCounter += 2;                         tempLength -= 2;                         totalSubWords++;                     }                 }             }else{                 //tek sayı ise                 while(subCounter != wordList[i].Length){                     if(tempLength % 3 != 0 && tempLength % 2 != 0){                         int randDivider = UnityEngine.Random.Range(2, 4);                         subWordList.Add(wordList[i].Substring(subCounter, randDivider));                         subCounter += randDivider;                         tempLength -= randDivider;                         totalSubWords++;                     } else if(tempLength % 3 == 0){                         subWordList.Add(wordList[i].Substring(subCounter, 3));                         subCounter += 3;                         tempLength -= 3;                         totalSubWords++;                     } else if(tempLength % 2 == 0){                         subWordList.Add(wordList[i].Substring(subCounter, 2));                         subCounter += 2;                         tempLength -= 2;                         totalSubWords++;                     }                     else{                         int randDivider = UnityEngine.Random.Range(2, 4);                         subWordList.Add(wordList[i].Substring(subCounter, randDivider));                         subCounter += randDivider;                         tempLength -= randDivider;                         totalSubWords++;                     }                  }              }         }     }

    private void AdjustBackgroundColor()     {         int tempRand = UnityEngine.Random.Range(0, 4);          switch (tempRand)         {              case 0:                 ColorUtility.TryParseHtmlString("#eaf3ee", out myColor);                 mainCamera.GetComponent<Camera>().backgroundColor = myColor;                 break;              case 1:                 ColorUtility.TryParseHtmlString("#eceff6", out myColor);                 mainCamera.GetComponent<Camera>().backgroundColor = myColor;                 break;              case 2:                 ColorUtility.TryParseHtmlString("#f8f3ec", out myColor);                 mainCamera.GetComponent<Camera>().backgroundColor = myColor;                 break;              case 3:                 ColorUtility.TryParseHtmlString("#e6f4df", out myColor);                 mainCamera.GetComponent<Camera>().backgroundColor = myColor;                 break;              case 4:
                ColorUtility.TryParseHtmlString("#fff5d4", out myColor);                 mainCamera.GetComponent<Camera>().backgroundColor = myColor;                 break;              default:                 ColorUtility.TryParseHtmlString("#eaf3ee", out myColor);                 mainCamera.GetComponent<Camera>().backgroundColor = myColor;                 break;         }     }      private void AdjustColor(GameObject objWord)     {         int tempRand = UnityEngine.Random.Range(0, 4);         if(beforeRand == tempRand)             tempRand = UnityEngine.Random.Range(0, 4);         beforeRand = tempRand;          switch(wordColors[tempRand]){              case "a":                 ColorUtility.TryParseHtmlString("#5B507A", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "b":
                ColorUtility.TryParseHtmlString("#E8360C", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "c":
                ColorUtility.TryParseHtmlString("#FFAB0D", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "d":
                ColorUtility.TryParseHtmlString("#6F0CE8", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "e":                 ColorUtility.TryParseHtmlString("#999999", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "f":                 ColorUtility.TryParseHtmlString("#585B56", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "g":                 ColorUtility.TryParseHtmlString("#BB342F", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              case "h":                 ColorUtility.TryParseHtmlString("#E38188", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;
             case "i":                 ColorUtility.TryParseHtmlString("#D7AF70", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;

            case "j":                 ColorUtility.TryParseHtmlString("#680DFF", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;

            case "k":                 ColorUtility.TryParseHtmlString("#4B9ED1", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;

            case "l":                 ColorUtility.TryParseHtmlString("#E3854B", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;              default:
                ColorUtility.TryParseHtmlString("#4B9ED1", out myColor);                 objWord.GetComponent<Image>().color = myColor;                 break;         }     }      public void InstantiateWord(GameObject instObj){          checkNewWord = true;          if (selectedWordNumber < 0)             selectedWordNumber = 0;          float xAxis=(Screen.width / 2) + (selectedWordNumber * (Screen.width / 12));         botArrWord[selectedWordNumber] = Instantiate(instObj, new Vector3(xAxis, Screen.height / 12, 0), Quaternion.identity, canvGame);          Vector3 targetScale = new Vector3(0.72f - selectedWordNumber / 20f, 0.72f - selectedWordNumber / 20f, 0.72f - selectedWordNumber / 20f);         botArrWord[selectedWordNumber].transform.localScale = targetScale;         StartCoroutine(LerpScale(0.3f, botArrWord[selectedWordNumber], new Vector3(0,0,0), targetScale, false, true));          botArrWord[selectedWordNumber].GetComponent<Rigidbody2D>().isKinematic = true;         botArrWord[selectedWordNumber].GetComponent<EventTrigger>().triggers.Clear();         //botArrWord[selectedWordNumber].GetComponent<CircleCollider2D>().enabled = false;         botArrWord[selectedWordNumber].GetComponent<PolygonCollider2D>().enabled = false;         botArrWord[selectedWordNumber].GetComponent<Image>().sprite = normalCircle;         EventTrigger.Entry entry = new EventTrigger.Entry();         entry.eventID = EventTriggerType.PointerClick;         entry.callback.AddListener((data) => { DestroyBotWord((PointerEventData)data, instObj); });         botArrWord[selectedWordNumber].GetComponent<EventTrigger>().triggers.Add(entry);          int tempSelected = selectedWordNumber; tempSelected--;          if(botArrWord.Length>1){             for (int i = 0; i < selectedWordNumber; i++){                 if(botArrWord[i] != null && botArrWord[tempSelected] != null)                     StartCoroutine(LerpScale(0.3f, botArrWord[i], botArrWord[tempSelected].transform.localScale, targetScale, false, true));             }                               for (int i = 0; i < selectedWordNumber; i++){                 if (botArrWord[i] != null && botArrWord[tempSelected] != null)                     StartCoroutine(LerpMove(0.3f, botArrWord[i], false, false, true));             }         }     }

    public void DestroyBotWord(PointerEventData data, GameObject topObj)     {         topObj.GetComponent<Word>().Clicked();     }      private void ConcatCheck(bool destroyed){         getHint = true;          string concatedWord = "";         if (!destroyed && botArrWord.Length>1){             for (int i = 0; i < selectedWordNumber; i++){                 if (botArrWord[i] != null)
                    concatedWord += String.Concat(botArrWord[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);             }         }else if(destroyed && botArrWord.Length>1){
            for (int i = 0; i < selectedWordNumber; i++){                 if(botArrWord[i] != null)                     concatedWord += String.Concat(botArrWord[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text);             }         }          foreach(string word in wordList){             if(word.Equals(concatedWord)){
                for (int i = 0; i < botArrWord.Length; i++){                     if(botArrWord[i] != null){                         lerpMove = true; lerpScale = true;                         selectedWordNumber--; concatedWord = "";                         StartCoroutine(LerpMove(0.3f, botArrWord[i], false, true, true));                         botArrWord[i].GetComponent<EventTrigger>().triggers.Clear();                         Destroy(botArrWord[i], 0.5f);                         //kelime bulundu, sahnedeki(üst) küreleri sil
                        for (int j = 0; j < arrWord.Count; j++){                             if (arrWord[j] != null && arrWord[j].GetComponent<Word>().isActive){                                 Destroy(arrWord[j].GetComponent<EventTrigger>());                                 StartCoroutine(LerpScale(0.3f, arrWord[j], arrWord[j].transform.localScale, new Vector3(0, 0, 0), true, true));                                 arrWord[j] = null;                                 ArrangeForHint(word);
                            }                         }                     }                 }             }         }      }        public void DestroyWord(string strSubword)
    {         int indexDest = 0, tempSelected = selectedWordNumber; tempSelected--;         Vector3 targetScale = new Vector3(0.72f - tempSelected / 20f, 0.72f - tempSelected / 20f, 0.72f - tempSelected / 20f);          for (int i = 0; i <= botArrWord.Length - 1; i++)
        {             if (botArrWord[i] == null)                 continue;              posText = botArrWord[i].transform.GetChild(0);             textWord = posText.gameObject.GetComponent<TextMeshProUGUI>();             if (textWord.text.Equals(strSubword))
            {                 lerpMove = true; lerpScale = true;                 Destroy(botArrWord[i].GetComponent<EventTrigger>());                 StartCoroutine(LerpScale(0.2f, botArrWord[i], posText.parent.gameObject.transform.localScale, new Vector3(0, 0, 0), true, true));                 botArrWord[i] = null;                 ArrangeArray();                 indexDest = i;             }
        }          for (int i = 0; i <= botArrWord.Length - 1; i++)         {
            if (botArrWord[i] != null && i < indexDest){                 StartCoroutine(LerpMove(0.3f, botArrWord[i], true, false, true));                 StartCoroutine(LerpScale(0.3f, botArrWord[i], botArrWord[i].transform.localScale, targetScale, false, true));             }             else if (botArrWord[i] != null && i >= indexDest){                 StartCoroutine(LerpMove(0.3f, botArrWord[i], false, false, true));                 StartCoroutine(LerpScale(0.3f, botArrWord[i], botArrWord[i].transform.localScale, targetScale, false, true));             }
        }      }      private void ArrangeForHint(string foundedWord){         if(getHint){             getHint = false;             wordFound++;              if(wordFound == wordList.Count){
                Manager.Instance.Sound.StopSounds();
                Manager.Instance.Sound.PlayEffect(Sounds.WIN);                 Invoke("PlayMusic", 2f);

                //levelNum++;
                gameStart = false;                 SuccesPopup.percentOfTime = timeDiff / timeInLevel;
                Manager.Instance.Popup.Open("success");                 levelCounterForAds++;                  if(levelCounterForAds % 3 == 0){                     Manager.Instance.ShowInterstitial();                 }             }              int indexToRemove = wordList.IndexOf(foundedWord);             tempWordList[indexToRemove] = tempWordList[indexToRemove].Remove(0);         }     }      private void PlayMusic(){         Manager.Instance.Sound.PlaySound(Sounds.BGM, true);     }      public static string GetHint(){
         int loopCounter = 0; 
        foreach (string word in tempWordList){

            if (tempWordList[hintIndex] != null && tempWordList.Length - 1 > hintIndex){                 hintIndex++;             }              hintIndex %= (tempWordList.Length - 1); 
            while (tempWordList[hintIndex] != null && tempWordList[hintIndex] == "" && loopCounter <= (wordListCount * 2)){                 hintIndex++; loopCounter++;                 if (tempWordList.Length > hintIndex && tempWordList[hintIndex] == "")                     hintIndex %= (tempWordList.Length - 1);

            }
            break;
        }          return tempWordList[hintIndex];     }      private void ArrangeArray(){         for (int i = 1; i < botArrWord.Length; i++){              if(selectedWordNumber>0 && botArrWord[i] != null && botArrWord[i-1] == null)
            {
                int temp = 0;                 GameObject objBot = botArrWord[i]; 
                do{                     botArrWord[i - temp] = null;                     ++temp;                     botArrWord[i - temp] = objBot;                  } while (botArrWord[i - temp] == null);              }         }     }      private void GetLevelText(int level, string textFileName){          TextAsset levelFile = Resources.Load<TextAsset>("Text/" + textFileName);         string strText = levelFile.text;          lines = strText.Split('\n');         strText = lines[level - 1];         requestedLine = strText.Split(' ');          int wordsStartedAt, count = 1;         string temp = "";
         while(temp != "-"){
            temp = requestedLine[count];             if(temp != "-"){
                gameTheme += requestedLine[count];                 gameTheme += " ";             }             count++;         }          do{             temp = requestedLine[count];             count++;         } while (temp != "-");          wordsStartedAt = count;          for (int i = 2; i < (count - 1); i++){             wordColors.Add(requestedLine[i]);         }          do{             temp = requestedLine[count];             count++;         } while (temp != "-");          for (int i = wordsStartedAt; i < (count - 1); i++){             wordList.Add(requestedLine[i]);             totalWords++;         }

        if (levelNum == 1){             wordsStartedAt = count;              do{                 temp = requestedLine[count];                 count++;             } while (temp != "-");              for (int i = wordsStartedAt; i < (count - 1); i++){                 tutorialTextList += requestedLine[i];                 tutorialTextList += " ";             }              tutorialText.text = tutorialTextList;         }else{             if(tutorialHand.IsActive() || tutorialText.IsActive()){                 tutorialText.gameObject.SetActive(false);                 tutorialHand.gameObject.SetActive(false);             }         }     }  }  