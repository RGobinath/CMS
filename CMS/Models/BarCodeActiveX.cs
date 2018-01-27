using Microsoft.Win32;
using System;
using System.Drawing.Printing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
namespace BarCodeActiveX
{
    [Guid("DDCA5845-0F5F-4696-8BE3-DDC54989265C"), ClassInterface(ClassInterfaceType.AutoDual), ProgId("FX.Barcode.TPL2844")]
	public class BarCodeActiveX : IObjectSafety 
	{
		private string strTextToPrint = string.Empty;
		private string strPort = string.Empty;
		[ComVisible(true)]
		public string TextToPrint
		{
			get
			{
				return this.strTextToPrint;
			}
			set
			{
				this.strTextToPrint = value;
			}
		}
		[ComVisible(true)]
		public int InstalledPrintersCount
		{
			get
			{
				return PrinterSettings.InstalledPrinters.Count;
			}
		}
		[ComVisible(true)]
		public string DefaultPrinter
		{
			get
			{
				PrinterSettings printerSettings = new PrinterSettings();
				string result;
				for (int i = 0; i < PrinterSettings.InstalledPrinters.Count - 1; i++)
				{
					printerSettings.PrinterName = PrinterSettings.InstalledPrinters[i].ToString();
					if (printerSettings.IsDefaultPrinter)
					{
						result = PrinterSettings.InstalledPrinters[i].ToString();
						return result;
					}
				}
				result = "Not found";
				return result;
			}
		}
		[ComVisible(true)]
		public string Port
		{
			get
			{
				return this.strPort;
			}
			set
			{
				this.strPort = value;
			}
		}
		[ComVisible(true)]
		public string InstalledPrintersList(int index)
		{
			return PrinterSettings.InstalledPrinters[index];
		}
		public string PrintBarCode(string PrinterName)
		{
			string result;
			try
			{
				string szString = string.Concat(new object[]
				{
					"O", 
					'\n', 
					"Q200,24", 
					'\n', 
					"D12", 
					'\n', 
					"ZT", 
					'\n', 
					"JF", 
					'\n', 
					"N", 
					'\n', 
					"B2,50,0,1,2,2,60,B,\"", 
					this.TextToPrint, 
					"\"", 
					'\n', 
					"P1", 
					'\n'
				});
				RawPrinterHelper rawPrinterHelper = new RawPrinterHelper();
				rawPrinterHelper.SendStringToPrinter(PrinterName, szString);
				result = "Success";
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}
		public string PrintBarCodeCommand(string PrinterName, string strCommand)
		{
			string result;
			try
			{
				RawPrinterHelper rawPrinterHelper = new RawPrinterHelper();
				rawPrinterHelper.SendStringToPrinter(PrinterName, strCommand);
				result = "Success";
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}
		public int PrintBarCodeDirect(string PrinterPort, string strCommand)
		{
			int result;
			try
			{
				PrintAccessLayer printAccessLayer = new PrintAccessLayer();
				int num = printAccessLayer.StartWrite(this.Port);
				if (num != -1)
				{
					printAccessLayer.Write(strCommand);
					printAccessLayer.EndWrite();
					result = num;
				}
				else
				{
					result = num;
				}
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		[ComRegisterFunction]
		public static void RegisterClass(string key)
		{
			StringBuilder stringBuilder = new StringBuilder(key);
			stringBuilder.Replace("HKEY_CLASSES_ROOT\\", "");
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(stringBuilder.ToString(), true);
			RegistryKey registryKey2 = registryKey.CreateSubKey("Control");
			registryKey2.Close();
			RegistryKey registryKey3 = registryKey.OpenSubKey("InprocServer32", true);
			registryKey3.SetValue("CodeBase", Assembly.GetExecutingAssembly().CodeBase);
			registryKey3.Close();
			registryKey.Close();
		}
		[ComUnregisterFunction]
		public static void UnregisterClass(string key)
		{
			StringBuilder stringBuilder = new StringBuilder(key);
			stringBuilder.Replace("HKEY_CLASSES_ROOT\\", "");
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(stringBuilder.ToString(), true);
			registryKey.DeleteSubKey("Control", false);
			RegistryKey registryKey2 = registryKey.OpenSubKey("InprocServer32", true);
			registryKey.DeleteSubKey("CodeBase", false);
			registryKey.Close();
		}


        //IObjectSafety Implemented
        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;

        public int GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            return S_OK;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            return S_OK;
        }
	}

    [ComImport()]
    [Guid("CB5BDC81-93C1-11CF-8F20-00805F2CD064")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IObjectSafety
    {
        [PreserveSig()]
        int GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions);

        [PreserveSig()]
        int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions);
    }
}
