using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace SharpDevelopIDEHost
{

    /// <summary>
    /// This class contains business logic for FrmGenerateEventHandler form.
    /// All this code is used only in IDE host.
    /// </summary>
    public class EventHandlersGenerator
    {
        /// <summary>
        /// This manager keeps information about all accessible events in objects that can be used in scripts (SDA) for one project item.
        /// All information is loaded from AddIn template compiled to .dll.
        /// </summary>
        public class HandlerSignatureManager
        {
            // <instanced object -> object type>
            private Dictionary<string, WithEventsObject> declaredObjectsTypes = new Dictionary<string,WithEventsObject>();

            // <object type -> declared events>
            private Dictionary<string, WithEventsObject> withEventObjects = new Dictionary<string,WithEventsObject>();


            private class WithEventsObject
            {   
                public string TypeName { get; set; }
                public Dictionary<string, string> Events; // <event name -> signature>
            }


            private string baseClassName;

            public string BaseClassName
            {
                get { return baseClassName; }
                private set { baseClassName = value; }
            }


            /// <summary>
            /// Initializes HandlerSignatureManager for specified add-in template and specified project item
            /// </summary>
            public HandlerSignatureManager(Assembly assembly, string className)
            {
                BaseClassName = className;
                var type = assembly.GetType(className);
                foreach (var prop in type.GetProperties())
                {
                    var instanceName = prop.Name;
                    var objectType = prop.PropertyType.FullName;

                    if (!withEventObjects.ContainsKey(objectType))
                    {
                        var newObj = new WithEventsObject { TypeName = objectType, Events = new Dictionary<string,string>() };
                        foreach (var ev in prop.PropertyType.GetEvents())
                            newObj.Events[ev.Name] = GetArgumentsListFromDelegateType(ev.EventHandlerType);
                        withEventObjects[objectType] = newObj;
                    }

                    declaredObjectsTypes[instanceName] = withEventObjects[objectType];
                }
            }


            /// <summary>
            /// Retrieve all necessary information from reflected delegate type and create 
            /// function argument declaration in VB syntax (ex. ByVal ArgumentName as ArgumantType)
            /// </summary>
            private static string GetArgumentsListFromDelegateType(Type t)
            {
                var res = new StringBuilder();
                foreach (var param in t.GetMethod("Invoke").GetParameters())
                {
                    if (res.Length > 0)
                        res.Append(", ");

                	var modifier = param.ParameterType.FullName.EndsWith("&") ? "ByRef" : "ByVal";
                    res.Append(String.Format("{0} {1} As {2}", modifier, param.Name, param.ParameterType.FullName.TrimEnd('&')));
                }
                return res.ToString();
            }


            /// <summary>
            /// Enumerates all available objects (objects that could contain events)
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> GetAllInstancedObjects()
            {
                return declaredObjectsTypes.Keys;
            }


            /// <summary>
            /// Enumerates all available events for specified object
            /// </summary>
            public IEnumerable<string> GetAllEvents(string objName)
            {
                return declaredObjectsTypes[objName].Events.Keys;
            }


            /// <summary>
            /// Creates event-handler signature for specified event of specified object
            /// </summary>
            /// <param name="objName">object name</param>
            /// <param name="eventName">event name</param>
            /// <returns>Fully generated event-hendler signature</returns>
            public string GetEventSignature(string objName, string eventName)
            {
				return String.Format("({0}) Handles {1}.{2}", declaredObjectsTypes[objName].Events[eventName], objName, eventName);
            }

        }

   

        /// <summary>
        /// Keeps HandlerSignatureManager-s for all loaded project items.
        /// Singleton design pattern is used.
        /// </summary>
        public class HandlerSignatureCollection
        {
            private static HandlerSignatureCollection me = null;

            private readonly static string dllBasePath;
            private readonly string[] projectItems = { "zebCtlNTAddIn.ITMBase", "zebCtlNTAddIn.Basispositionen", "zebCtlNTAddIn.Plangroessen" };

            private Dictionary<string, HandlerSignatureManager> loadedProjItems = new Dictionary<string,HandlerSignatureManager>();


            /// <summary>
            /// Returns single instance of a class
            /// </summary>
            public static HandlerSignatureCollection Instance
            {
                get
                {
                    if (me == null)
                        me = new HandlerSignatureCollection();
                    return me;
                }
            }


            private HandlerSignatureCollection()
            {
            }


            static HandlerSignatureCollection()
            {
                dllBasePath = Path.GetDirectoryName(Application.StartupPath) + Path.DirectorySeparatorChar;
            }


            /// <summary>
            /// Initialize HandlerSignatureManager for each project item
            /// </summary>
            /// <param name="assemblyPath">Path to .dll (that is compiled actual add-in template with generated .Designer.vb files)</param>
            public void Initialize(string assemblyPath)
            {
                var assembly = LoadScriptAssembly(assemblyPath);
                foreach (var projItem in projectItems)
                {
                    var shortName = projItem.Substring(projItem.IndexOf('.') + 1);
                    loadedProjItems[shortName] = new HandlerSignatureManager(assembly, projItem);
                }
            }


            /// <summary>
            /// Returns HandlerSignatureManager for specified project item
            /// </summary>
            /// <param name="projItem">project item</param>
            /// <returns></returns>
            public HandlerSignatureManager GetEvHandlersManager(string projItem)
            {
                if (!loadedProjItems.ContainsKey(projItem))
                    return null;
                return loadedProjItems[projItem];
            }


            /// <summary>
            /// Loads add-in assembly (resolving all needed references)
            /// </summary>
            /// <param name="assembly">Path to .dll (that is compiled actual add-in template with generated .Designer.vb files)</param>
            /// <returns>assembly</returns>
            private static Assembly LoadScriptAssembly(string assembly)
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
                using (var fin = new BinaryReader(new FileStream(assembly, FileMode.Open)))
                {
                    var data = fin.ReadBytes((int)fin.BaseStream.Length);
					return Assembly.Load(data);// ReflectionOnlyLoad(data);
                }
            }


            /// <summary>
            /// Loads references assembly for add-in .dll
            /// </summary>
            private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
            {
                var dllPath = dllBasePath + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                if (File.Exists(dllPath))
                    return Assembly.ReflectionOnlyLoadFrom(dllPath);
                else
                    return Assembly.ReflectionOnlyLoad(args.Name);
            }
        }

    }

}

