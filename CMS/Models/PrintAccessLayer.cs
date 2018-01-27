using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
namespace BarCodeActiveX
{
	public class PrintAccessLayer
	{
		private const int GENERIC_WRITE = 1073741824;
		private const int OPEN_EXISTING = 3;
		private const int FILE_SHARE_WRITE = 2;
		private StreamWriter _fileWriter;
		private FileStream _outFile;
		private int _hPort;
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int CloseHandle(int hObject);
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int CreateFileA(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, int hTemplateFile);
		public int StartWrite(string printerPath)
		{
            IntPtr handle = IntPtr.Zero;
			this._hPort = PrintAccessLayer.CreateFileA(printerPath, 1073741824, 2, IntPtr.Zero, 3, 0, 0);
			int hPort;
			if (this._hPort == -1)
			{
				hPort = this._hPort;
			}
			else
			{
				handle = new IntPtr(this._hPort);
				this._outFile = new FileStream(handle, FileAccess.Write);
				this._fileWriter = new StreamWriter(this._outFile);
				hPort = this._hPort;
			}
			return hPort;
		}
		public void Write(string rawLine)
		{
			this._fileWriter.WriteLine(rawLine);
		}
		public void EndWrite()
		{
			this._fileWriter.Flush();
			this._fileWriter.Close();
			this._outFile.Close();
			PrintAccessLayer.CloseHandle(this._hPort);
		}
		public int PrintBarCode(string printerPath, string strCommand)
		{
            IntPtr handle = IntPtr.Zero;
			this._hPort = PrintAccessLayer.CreateFileA(printerPath, 1073741824, 2, IntPtr.Zero, 3, 0, 0);
			int hPort;
			if (this._hPort == -1)
			{
				hPort = this._hPort;
			}
			else
			{
				handle = new IntPtr(this._hPort);
				this._outFile = new FileStream(handle, FileAccess.Write);
				byte[] array = new byte[strCommand.Length];
				array = Encoding.ASCII.GetBytes(strCommand);
				this._outFile.Write(array, 0, array.Length);
				this._outFile.Close();
				PrintAccessLayer.CloseHandle(this._hPort);
				hPort = this._hPort;
			}
			return hPort;
		}
	}
}
