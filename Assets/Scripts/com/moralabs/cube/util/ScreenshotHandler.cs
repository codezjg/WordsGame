#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

public class ScreenshotHandler : MonoBehaviour
{

	public static event Action ScreenshotFinishedSaving;
	public static event Action ImageFinishedSaving;

	public static string savedImagePath = string.Empty;

#if UNITY_IPHONE
	
	[DllImport("__Internal")]
	private static extern bool saveToGallery (string path);

#endif

	public static IEnumerator Save (string fileName, string albumName = "MyScreenshots", bool callback = false)
	{
#if UNITY_IPHONE
		
		bool photoSaved = false;

		string date = System.DateTime.Now.ToString ("dd-MM-yy");

		ScreenshotHandler.ScreenShotNumber++;

		string screenshotFilename = fileName + "_" + ScreenshotHandler.ScreenShotNumber + "_" + date + ".png";

		
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			
			string iosPath = Application.persistentDataPath + "/" + fileName;
			savedImagePath = iosPath;
			ScreenCapture.CaptureScreenshot (screenshotFilename);

			while (!photoSaved) {
				photoSaved = saveToGallery (iosPath);
					
				yield return new WaitForSeconds (.5f);
			}				
			
			UnityEngine.iOS.Device.SetNoBackupFlag (iosPath);
			
		} else {
			
			ScreenCapture.CaptureScreenshot (screenshotFilename);
			
		}
			

#endif
		yield return 0;
		if (callback)
			ScreenshotFinishedSaving ();
	}


	public static IEnumerator SaveExisting (string filePath, bool callback = false)
	{
		yield return 0;

		bool photoSaved = false;


#if UNITY_IPHONE
		
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			
			while (!photoSaved) {
				photoSaved = saveToGallery (filePath);
					
				yield return new WaitForSeconds (.5f);
			}
			
			UnityEngine.iOS.Device.SetNoBackupFlag (filePath);
		}
			
#endif
		
		if (callback)
			ImageFinishedSaving ();
	}


	public static int ScreenShotNumber {
		set { PlayerPrefs.SetInt ("screenShotNumber", value); }

		get { return PlayerPrefs.GetInt ("screenShotNumber"); }
	}
}