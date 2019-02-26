using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;

namespace com.moralabs.cube.util
{
	public class Library {

		public static string GetTimeFormatted(long second){

			if(second < 0) return "0"+Language.getText("secondShort");

			if(second <= 60){
				int time = (int)second;

				return time + Language.getText("secondShort");
			}else if(second <= 60 * 60){
				int time = (int)(second / 60);

				return time + Language.getText("minutesShort");
			}else if(second <= 60 * 60 * 24){
				int time = (int)second / (60 * 60);

				return time + Language.getText("hourShort");
			}else{
				int time = (int)second / (60 * 60 * 24);

				return time + Language.getText("dayShort");
			}
		}

		public static string GetTimeLongFormatted(long second){

			if(second <= 0) return "00:00:00";
			
			int hour = (int)(second / (60 * 60));
			second = second - 60 * 60 * hour;
			int minute = (int)(second / 60);
			second = second - 60 * minute;

			string hourStr = hour < 10 ? "0"+hour : "" + hour;
			string minuteStr = minute < 10 ? "0"+minute : "" + minute;
			string secondStr = second < 10 ? "0"+second : "" + second;

			return hourStr + ":" + minuteStr + ":" + secondStr;
		}

		public static string GetTimeGameFormatted(float second){
			if(second <= 0) return "00:00";

			int minute = (int)(second / 60);
			second = second - 60 * minute;

			string minuteStr = minute < 10 ? "0"+minute : "" + minute;
			string secondStr = second < 10 ? "0"+second : "" + second;

			return minuteStr + ":" + secondStr;
		}

		public static string GetFormattedString(string str){
			if(str == null) return "";
			return System.Uri.UnescapeDataString(str).Replace("+", " ");
		}
		
		public static string GetMD5(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);
			
			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}

		public static Color UIntToColor(uint color)
		{
			byte a = (byte)(color >> 24);
			byte r = (byte)(color >> 16);
			byte g = (byte)(color >> 8);
			byte b = (byte)(color >> 0);
			return new Color(r, g, b, a);
		}

		public static Color StringToColor(string color)
		{
			if(color == null || color.Length == 0) return Color.black;

			try{
				string[] rgb = color.Split(new char[]{'|'});
				return new Color(int.Parse(rgb[0]) / 255f, int.Parse(rgb[1]) / 255f, int.Parse(rgb[2]) / 255f);
			}catch(System.Exception ex){
				Debug.LogWarning ("Library.StringToColor Color : "+color+" Exception : "+ex.Message);
				return Color.black;
			}
		}

		public static string ColorToString(Color color)
		{
			return (int)(color.r * 255f) + "|" + (int)(color.g * 255f) + "|" + (int)(color.b * 255f);
		}
	}
}