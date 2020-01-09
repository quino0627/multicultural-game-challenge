using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ALT_List_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/ALT_List.xlsx";
	private static readonly string exportPath = "Assets/Resources/ALT_List.asset";
	private static readonly string[] sheetNames = { "LEVEL1","LEVEL2","LEVEL3", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_Alternative data = (Entity_Alternative)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_Alternative));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_Alternative> ();
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

					Entity_Alternative.Sheet s = new Entity_Alternative.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_Alternative.Param p = new Entity_Alternative.Param ();
						
					cell = row.GetCell(0); p.level = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.no = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.sylnum = (cell == null ? 0.0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.origin = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.expect = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.target = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.cor = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.ex1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.ex2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.ex3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.ex4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.filename = (cell == null ? "" : cell.StringCellValue);
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
