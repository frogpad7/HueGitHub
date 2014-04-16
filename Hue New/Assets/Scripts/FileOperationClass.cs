using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class FileOperations : MonoBehaviour {
	public static void WriteToFile(string Target, string Text){
		File.WriteAllText(Target, Text);
	}
	
	public static string ReadFile(string Target){
		return File.ReadAllText(Target);
	}
}
//By Erdroy (c) HLTC
//