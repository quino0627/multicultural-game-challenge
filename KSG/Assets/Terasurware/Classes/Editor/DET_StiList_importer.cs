using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class DET_StiList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/DET_StiList.xlsx";
	private static readonly string exportPath = "Assets/Resources/DET_StiList.asset";
	private static readonly string[] sheetNames = { "LEVEL1","LEVEL2","LEVEL3", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_sort data = (Entity_sort)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_sort));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_sort> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_sort.Sheet s = new Entity_sort.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_sort.Param p = new Entity_sort.Param ();
						
					cell = row.GetCell(0); p.level = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.no = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.sylnum = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.target = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.cor = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.ex1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.ex2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.ex3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.ex4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.filename = (cell == null ? "" : cell.StringCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
