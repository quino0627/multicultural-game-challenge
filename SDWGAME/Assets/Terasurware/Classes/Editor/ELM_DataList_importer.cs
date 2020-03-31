using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ELM_DataList_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/DataList/ELM_DataList.xlsx";
	private static readonly string exportPath = "Assets/Resources/DataList/ELM_DataList.asset";
	private static readonly string[] sheetNames = { "Level1","Level2","Level3", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			ELM_DataList data = (ELM_DataList)AssetDatabase.LoadAssetAtPath (exportPath, typeof(ELM_DataList));
			if (data == null) {
				data = ScriptableObject.CreateInstance<ELM_DataList> ();
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

					ELM_DataList.Sheet s = new ELM_DataList.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						ELM_DataList.Param p = new ELM_DataList.Param ();
						
					cell = row.GetCell(0); p.Stage = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.게임No = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.난이도 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.음절수 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.원자극 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.원자극음성 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.탈락음소 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.정답 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.오답1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.오답2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.오답3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.오답4 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.정답음성 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.오답음성1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.오답음성2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.오답음성3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.오답음성4 = (cell == null ? "" : cell.StringCellValue);
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
