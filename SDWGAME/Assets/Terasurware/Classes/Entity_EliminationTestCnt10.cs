using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_EliminationTestCnt10 : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public double 음절수;
		public string 초성종성;
		public string 자극 ;
		public string 탈락자극 ;
		public string 정답;
		public string 오답1;
		public string 오답2;
		public string 오답3;
		public string 오답4 ;
		public string 정답음성;
		public string 오답음성1;
		public string 오답음성2;
		public string 오답음성3;
		public string 오답음성4;
	}
}

