using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class SYN_DataList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/DataList/SYN_DataList.xlsx";
	private static readonly string exportPath = "Assets/Resources/DataList/SYN_DataList.asset";
	private static readonly string[] sheetNames = { "Level1","Level2","Level3", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			SYN_DataList data = (SYN_DataList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(SYN_DataList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<SYN_DataList> ();
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

					SYN_DataList.Sheet s = new SYN_DataList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						SYN_DataList.Param p = new SYN_DataList.Param ();
						
					cell = row.GetCell(0); p.Stage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.게임No = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.난이도 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.음절수 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.Target = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.정답1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.정답2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.정답3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.보기1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.보기2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.보기3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.보기4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.보기5 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.filename = (cell == null ? "" : cell.StringCellValue);
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
