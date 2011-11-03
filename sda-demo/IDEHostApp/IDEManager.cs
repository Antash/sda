using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpDevelop.Sda;

namespace IDEHostApp
{
   class IDEManager
   {
      private string baseDir;
      private string binDir;
      private string dataDir;
      private string addInDir;

      private WorkbenchSettings workbenchSettings;
      private SharpDevelopHost host;
      private StartupSettings startup;

      public IDEManager()
      {
         baseDir = @"C:\home\master\sda\SharpDevelop_4.1.0.8000_Source";
         //baseDir = @"C:\Program Files\SharpDevelop\4.1";
         binDir = baseDir + @"\bin\";
         dataDir = baseDir + @"\data\";
         addInDir = baseDir + @"\AddIns\";

         AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

         ConfigureEnviorenment();
      }

      private bool suppressResolve;

      Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
      {
         if (suppressResolve)
            return null;
         Assembly result = null;
         if (args != null && !string.IsNullOrEmpty(args.Name))
         {
            try
            {
               var assemblyPath = FindAssembly(args.Name);
               if (!String.IsNullOrEmpty(assemblyPath))
                  result = Assembly.LoadFrom(assemblyPath);
               else
               {
                  suppressResolve = true;
                  string nn = args.Name;
                 // result = Assembly.Load(nn);
                  suppressResolve = false;
               }

            }
            catch (Exception e)
            {
               Console.Error.WriteLine("SharpDevelop assembly loading error: " + e.Message);
            }
         }

         return result;
      }

      private string FindAssembly(string name)
      {
         var typeName = name.Split(new string[] { "," }, StringSplitOptions.None)[0];
         var assemblyPath = Path.Combine(binDir, string.Format("{0}.dll", typeName));

         var strs = typeName.Split('.');
         var assemblyName = string.Empty;

         for (int j = strs.Length - 1; j >= 0; j--)
         {

            //assemblyName += strs[j];
            assemblyPath = Path.Combine(binDir, string.Format("{0}.dll", typeName));
            if (!File.Exists(assemblyPath))
            {
               assemblyPath = Path.ChangeExtension(assemblyPath, "exe");
               if (!File.Exists(assemblyPath))
               {
                  var strings = typeName.Split('.');
                  var sb = new StringBuilder();
                  for (int i = 0; i < strings.Length - 1; i++)
                  {
                     sb.Append(strings[i]);
                     sb.Append('.');
                  }
                  typeName = sb.ToString();
                  var assembls = Directory.GetFiles(addInDir, typeName + "*", SearchOption.AllDirectories);
                  if (assembls.Length > 0)
                  {
                     assemblyPath = Path.Combine(Path.GetDirectoryName(assembls[0]), string.Format("{0}dll", typeName));
                     if (!File.Exists(assemblyPath))
                     {
                        typeName += strings[strings.Length - 1];
                        assemblyPath = Path.Combine(Path.GetDirectoryName(assembls[0]), string.Format("{0}.dll", typeName));
                     }
                     else break;
                  }
               }
               else break;
            }
            else break;
            if (typeName.IndexOf(strs[j]) > 0)
               typeName = typeName.Substring(0, typeName.IndexOf(strs[j]) - 1);
         }
         if (!File.Exists(assemblyPath))
         {
            assemblyPath = string.Empty;
         }
         return assemblyPath;
      }

      private void ConfigureEnviorenment()
      {
         if (host != null) return;

         startup = new StartupSettings { AllowUserAddIns = true, DataDirectory = dataDir };
         startup.AddAddInsFromDirectory(addInDir);

         host = new SharpDevelopHost(AppDomain.CurrentDomain, startup);

         workbenchSettings = new WorkbenchSettings();

         RunIDE();
      }

      public void RunIDE()
      {
         System.Threading.ThreadPool.QueueUserWorkItem(ThreadedRun);
      }

      void ThreadedRun(object state)
      {
         host.RunWorkbench(workbenchSettings);
      }
   }
}
