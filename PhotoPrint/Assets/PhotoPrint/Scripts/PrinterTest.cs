using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using PrintUtil;

using System.Drawing;
using System.Drawing.Printing;

public class PrinterTest : MonoBehaviour {
	public delegate void ImageCreatedDelegate ();

	[SerializeField] RenderTexture rTexture;
	[SerializeField] string printerName;
	[SerializeField] string fileName;


	private ImageCreatedDelegate imageCreated;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.S)) {
			SaveToImage ();
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			PrintToImage ();
		}
		if (Input.GetKeyDown (KeyCode.C)) {
			PrinterJobCheck ();
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			PrinterJobDelete ();
		}
	}

	public void SaveToImage(){
		Debug.Log ("Save to PNG");

		Texture2D tex = new Texture2D (rTexture.width, rTexture.height, TextureFormat.ARGB32, false);
		RenderTexture.active = rTexture;
		tex.ReadPixels (new Rect (0, 0, rTexture.width, rTexture.height), 0, 0);
		tex.Apply ();

		byte[] bytes = tex.EncodeToPNG ();
		Object.Destroy (tex);

		File.WriteAllBytes (Application.dataPath + "/" + fileName, bytes);
//		imageCreated ();
	}

//	public void addImageCreateListener(ImageCreatedDelegate fnc) {
//		imageCreated += fnc;
//	}

	public void PrintToImage(){
		Debug.Log ("Print to PNG");
		PrintPhotoUtil.PrintPhoto (Application.dataPath + "/" + fileName, 1, 218, 854, SendQueeuEnd);
	}

	public void PrinterJobCheck(){
		PrintPhotoUtil.JobCheck (OutputHandler, ErrorOutputHanlder, Process_Exit);
	}
	public void PrinterJobDelete(){
		Debug.Log ("job deleta");
		PrintPhotoUtil.JobDelete (printerName, OutputHandler, ErrorOutputHanlder, Process_Exit);
	}

	void SendQueeuEnd(object sender, System.Drawing.Printing.PrintEventArgs args){
		Debug.Log (args.PrintAction);
		Debug.Log ("Send Queeu End.");
	}

	void OutputHandler(object sender, System.Diagnostics.DataReceivedEventArgs args){
		if (!string.IsNullOrEmpty(args.Data)){

			Debug.Log("output : " + args.Data);

			string _result = args.Data;
			string _str = "列挙された印刷ジョブの数";
			int jobNum = 0;

			if(_result.IndexOf(_str) != -1){
				jobNum =  int.Parse(_result.Substring(_str.Length + _result.IndexOf(_str)));	
				Debug.Log ("Job Num : " + jobNum);
			}
		}
	}
	void ErrorOutputHanlder(object sender, System.Diagnostics.DataReceivedEventArgs args){
		if (!string.IsNullOrEmpty(args.Data)){
			Debug.Log("error : " + args.Data);
		}
	}
	void Process_Exit(object sender, System.EventArgs e){
		System.Diagnostics.Process proc = (System.Diagnostics.Process)sender;
		proc.Kill();
	}
}
