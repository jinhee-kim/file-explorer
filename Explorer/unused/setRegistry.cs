using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Explorer
{
    class SetRegistry
    {
        /* unused
        // Extension - 파일 확장자 (.zip, .txt 등)
        // MenuName - 메뉴 항목의 이름 (재생, 열기 등)
        // MenuDescription - 표시 될 실제 텍스트
        // MenuCommand - 실행 파일의 경로

        public static void AddContextMenuItem(string Extension, string MenuName, string MenuDescription, string MenuCommand)
        {
            RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(Extension);
            Console.WriteLine(rkey);
            if (rkey != null)
            {
                string extstring = rkey.GetValue("").ToString();
                Console.WriteLine(extstring);
                rkey.Close();
                if (extstring != null)
                {
                    if (extstring.Length > 0)
                    {
                        rkey = Registry.ClassesRoot.OpenSubKey(extstring, true);
                        Console.WriteLine(rkey);
                        if (rkey != null)
                        {
                            string strkey = "shell\\" + MenuName + "\\command";
                            Console.WriteLine(strkey);
                            RegistryKey subky = rkey.CreateSubKey(strkey);
                            Console.WriteLine(subky);
                            if (subky != null)
                            {
                                subky.SetValue("", MenuCommand);
                                Console.WriteLine(subky);
                                subky.Close();
                                subky = rkey.OpenSubKey("shell\\" + MenuName, true);
                                Console.WriteLine(subky);
                                if (subky != null)
                                {
                                    subky.SetValue("", MenuDescription);
                                    Console.WriteLine(subky);
                                    subky.Close();
                                }
                            }
                            rkey.Close();
                        }
                    }
                }
            }
        }*/
    }
}
