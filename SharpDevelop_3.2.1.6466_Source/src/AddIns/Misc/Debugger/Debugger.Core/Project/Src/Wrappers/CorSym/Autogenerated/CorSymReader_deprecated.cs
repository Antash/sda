// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

// This file is automatically generated - any changes will be lost

#pragma warning disable 1591

namespace Debugger.Wrappers.CorSym
{
	using System;
	
	
	public partial class CorSymReader_deprecated
	{
		
		private Debugger.Interop.CorSym.CorSymReader_deprecated wrappedObject;
		
		internal Debugger.Interop.CorSym.CorSymReader_deprecated WrappedObject
		{
			get
			{
				return this.wrappedObject;
			}
		}
		
		public CorSymReader_deprecated(Debugger.Interop.CorSym.CorSymReader_deprecated wrappedObject)
		{
			this.wrappedObject = wrappedObject;
			ResourceManager.TrackCOMObject(wrappedObject, typeof(CorSymReader_deprecated));
		}
		
		public static CorSymReader_deprecated Wrap(Debugger.Interop.CorSym.CorSymReader_deprecated objectToWrap)
		{
			if ((objectToWrap != null))
			{
				return new CorSymReader_deprecated(objectToWrap);
			} else
			{
				return null;
			}
		}
		
		~CorSymReader_deprecated()
		{
			object o = wrappedObject;
			wrappedObject = null;
			ResourceManager.ReleaseCOMObject(o, typeof(CorSymReader_deprecated));
		}
		
		public bool Is<T>() where T: class
		{
			System.Reflection.ConstructorInfo ctor = typeof(T).GetConstructors()[0];
			System.Type paramType = ctor.GetParameters()[0].ParameterType;
			return paramType.IsInstanceOfType(this.WrappedObject);
		}
		
		public T As<T>() where T: class
		{
			try {
				return CastTo<T>();
			} catch {
				return null;
			}
		}
		
		public T CastTo<T>() where T: class
		{
			return (T)Activator.CreateInstance(typeof(T), this.WrappedObject);
		}
		
		public static bool operator ==(CorSymReader_deprecated o1, CorSymReader_deprecated o2)
		{
			return ((object)o1 == null && (object)o2 == null) ||
			       ((object)o1 != null && (object)o2 != null && o1.WrappedObject == o2.WrappedObject);
		}
		
		public static bool operator !=(CorSymReader_deprecated o1, CorSymReader_deprecated o2)
		{
			return !(o1 == o2);
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public override bool Equals(object o)
		{
			CorSymReader_deprecated casted = o as CorSymReader_deprecated;
			return (casted != null) && (casted.WrappedObject == wrappedObject);
		}
		
	}
}

#pragma warning restore 1591
