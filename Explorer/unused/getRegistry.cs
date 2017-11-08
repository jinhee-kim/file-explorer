using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Explorer
{
    class getRegistry
    {
        /* unused
        public static void getreg(string extension)
        {
            RegistryKey rKey = Registry.ClassesRoot.OpenSubKey(extension);
            Console.WriteLine(rKey);
            if (rKey != null)
            {
                var extstr = rKey.GetValue("") as string;
                Console.WriteLine(extstr);
                rKey.Close();
                if (extstr != null)
                {
                    var subKey = Registry.ClassesRoot.OpenSubKey(extstr);
                    Console.WriteLine(subKey);
                    if (subKey != null)
                    {
                        var shellKey = subKey.OpenSubKey("shell");
                        Console.WriteLine(shellKey);
                        subKey.Close();
                        if (shellKey != null)
                        {
                            var subKeys = shellKey.GetSubKeyNames();
                            if (subKeys != null)
                            {
                                var ctx = new ContextMenu();

                                foreach (var key in subKeys)
                                {
                                    Console.WriteLine(key);
                                    var openKey = shellKey.OpenSubKey(key);
                                    Console.WriteLine(openKey);
                                    if (openKey != null)
                                    {
                                        var text = openKey.GetValue("") as string;
                                        Console.WriteLine(text);
                                        if (text != null)
                                        {
                                            var item = new MenuItem();
                                            item.Text = text;

                                            ctx.MenuItems.Add(item);
                                        }
                                    }
                                    openKey.Close();
                                }
                                shellKey.Close();
                            }
                            // ctx.Show();
                        }
                    }
                }
            }
        }*/
    }
}
