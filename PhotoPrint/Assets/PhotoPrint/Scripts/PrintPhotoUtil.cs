using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;

/// <summary>
/// 画像プリントユーティリティクラス.
/// windows環境
/// </summary>

namespace PrintUtil{

	public class PrintPhotoUtil {

		/// <summary>
		/// 画像プリントメソッド.
		/// </summary>
		/// <param name="photoPath">画像ファイルパス.</param>
		/// <param name="copies">プリント枚数.</param>
		/// <param name="paperWidth">プリント紙幅.</param>
		/// <param name="paperHeight">プリント紙高.</param>
		/// <param name="OnEndPrint">プリント終了イベント.</param>
		public static void PrintPhoto(string photoPath, int copies = 1, int paperWidth = 0, int paperHeight = 0, PrintEventHandler OnEndPrint = null){
			int printedNum = 1;
			PrintDocument pd = new PrintDocument();
			pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
			pd.PrinterSettings.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

			// 紙サイズ設定（）
			if(paperWidth != 0 && paperHeight != 0){
				pd.DefaultPageSettings.PaperSize = pd.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize ("Sticker", paperWidth, paperHeight);
			}

			pd.PrinterSettings.Copies = (short)copies;

			pd.PrintPage += (sndr, args) => {
				System.Drawing.Image i = System.Drawing.Image.FromFile(photoPath);
				Rectangle m = args.MarginBounds;
				args.Graphics.DrawImage(i, m);
				if(printedNum < copies){
					args.HasMorePages = true;
					printedNum ++;
				}else{
					args.HasMorePages = false;
				}

				i.Dispose();
			};
				
			pd.EndPrint += OnEndPrint;
			pd.Print();
		}
		public static void JobCheck(DataReceivedEventHandler OutputHandler = null, 
									DataReceivedEventHandler ErrorOutputHanlder = null, 
									System.EventHandler ProcessExit = null){

			System.Diagnostics.Process process = new System.Diagnostics.Process();

			process.StartInfo.FileName = "CScript";
			process.StartInfo.UseShellExecute = false;

			process.StartInfo.RedirectStandardOutput = true;
			process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);

			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += new DataReceivedEventHandler(ErrorOutputHanlder);

			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.LoadUserProfile = true;

			process.StartInfo.Arguments = "-E:vbscript C:/Windows/System32/Printing_Admin_Scripts/ja-JP/prnjobs.vbs -l";

			process.EnableRaisingEvents = true;

			process.Exited += new System.EventHandler(ProcessExit);
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

		}
		public static void JobDelete(string printer, DataReceivedEventHandler OutputHandler = null, 
													DataReceivedEventHandler ErrorOutputHanlder = null, 
													System.EventHandler ProcessExit = null){

			System.Diagnostics.Process process = new System.Diagnostics.Process();

			process.StartInfo.FileName = "CScript";
			process.StartInfo.UseShellExecute = false;

			process.StartInfo.RedirectStandardOutput = true;
			process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);

			process.StartInfo.RedirectStandardError = true;
			process.ErrorDataReceived += new DataReceivedEventHandler(ErrorOutputHanlder);

			process.StartInfo.RedirectStandardInput = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.LoadUserProfile = true;

			process.StartInfo.Arguments = "-E:vbscript C:/Windows/System32/Printing_Admin_Scripts/ja-JP/prnqctl.vbs -p " + printer + " -x";
			process.EnableRaisingEvents = true;

			process.Exited += new System.EventHandler(ProcessExit);
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

		}
	}
}