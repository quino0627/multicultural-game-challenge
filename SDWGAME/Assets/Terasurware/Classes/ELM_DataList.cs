using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ELM_DataList : ScriptableObject
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
		
		public int Stage;
		public int 게임No;
		public string 난이도;
		public int 음절수;
		public string 원자극;
		public string 탈락음소;
		public string 정답;
		public string 오답1;
		public string 오답2;
		public string 오답3;
		public string 오답4;
		public string 정답음성;
		public string 오답음성1;
		public string 오답음성2;
		public string 오답음성3;
		public string 오답음성4;
	}
}

