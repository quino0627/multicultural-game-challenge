using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_sort : ScriptableObject
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
		
		public int level;
		public int no;
		public int sylnum;
		public string target;
		public string cor;
		public string ex1;
		public string ex2;
		public string ex3;
		public string ex4;
		public string filename;
	}
}

