using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BarcodeExport
{
    static class Program
    {
        /// <summary>
        /// Assembly instance to resolve dll loading.
        /// </summary>
        public class APPAssembly
        {
            public System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
                dllName = dllName.Replace(".", "_");
                if (dllName.EndsWith("_resources")) return null;

                System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                byte[] bytes = (byte[])rm.GetObject(dllName);
                return System.Reflection.Assembly.Load(bytes);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //Carga la dll
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(new APPAssembly().CurrentDomain_AssemblyResolve);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LogoForm(1000, 1500));
            Application.Run(new GUI());
        }
    }
}
