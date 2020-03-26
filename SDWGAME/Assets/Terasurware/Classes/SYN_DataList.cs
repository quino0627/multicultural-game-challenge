using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SYN_DataList : ScriptableObject
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
		public string Target;
		public string 정답1;
		public string 정답2;
		public string 정답3;
		public string 보기1;
		public string 보기2;
		public string 보기3;
		public string 보기4;
		public string 보기5;
		public string filename;
	}
}

