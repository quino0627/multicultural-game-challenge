using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DET_DataList : ScriptableObject
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
		
		public double Stage;
		public double 게임No;
		public string 난이도;
		public double 음절수;
		public string Target;
		public string 정답;
		public string 오답1;
		public string 오답2;
		public string 오답3;
		public string 오답4;
		public string 정답음성;
	}
}

