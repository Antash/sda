﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Gui;

namespace WixBinding.Tests.Utils
{
	/// <summary>
	/// Mock IViewContent class.
	/// </summary>
	public class MockViewContent : IViewContent
	{
		OpenedFile primaryFile;
		List<IViewContent> secondaryViews = new List<IViewContent>();
		
		public MockViewContent()
		{
			SetFileName("dummy.name");
		}
		
		public void SetFileName(string fileName)
		{
			primaryFile = new MockOpenedFile(fileName, false);
		}
		
		public void SetUntitledFileName(string fileName)
		{
			primaryFile = new MockOpenedFile(fileName, true);
		}
		
		#pragma warning disable 67
		public event EventHandler TabPageTextChanged;
		public event EventHandler Disposed;
		public event EventHandler IsDirtyChanged;
		public event EventHandler TitleNameChanged;
		public event EventHandler InfoTipChanged;
		#pragma warning restore 67
		
		public IList<OpenedFile> Files {
			get {
				throw new NotImplementedException();
			}
		}
		
		public OpenedFile PrimaryFile {
			get { return primaryFile; }
		}
		
		public FileName PrimaryFileName {
			get { return primaryFile.FileName; }
		}
		
		public bool IsDisposed {
			get { return false; }
		}
		
		public ICollection<IViewContent> SecondaryViewContents {
			get {
				return secondaryViews;
			}
		}
		
		public void Save(OpenedFile file, Stream stream)
		{
			throw new NotImplementedException();
		}
		
		public void Load(OpenedFile file, Stream stream)
		{
			throw new NotImplementedException();
		}
		
		public bool SupportsSwitchFromThisWithoutSaveLoad(OpenedFile file, IViewContent newView)
		{
			throw new NotImplementedException();
		}
		
		public bool SupportsSwitchToThisWithoutSaveLoad(OpenedFile file, IViewContent oldView)
		{
			throw new NotImplementedException();
		}
		
		public void SwitchFromThisWithoutSaveLoad(OpenedFile file, IViewContent newView)
		{
			throw new NotImplementedException();
		}
		
		public void SwitchToThisWithoutSaveLoad(OpenedFile file, IViewContent oldView)
		{
			throw new NotImplementedException();
		}
		
		public object Control {
			get {
				throw new NotImplementedException();
			}
		}
		
		public object InitiallyFocusedControl {
			get {
				throw new NotImplementedException();
			}
		}
		
		public IWorkbenchWindow WorkbenchWindow {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		
		public string TabPageText {
			get {
				throw new NotImplementedException();
			}
		}
		
		public string TitleName {
			get {
				throw new NotImplementedException();
			}
		}
		
		public string InfoTip {
			get {
				throw new NotImplementedException();
			}
		}
		
		public bool IsReadOnly {
			get {
				throw new NotImplementedException();
			}
		}
		
		public bool IsViewOnly {
			get {
				throw new NotImplementedException();
			}
		}
		
		public bool CloseWithSolution {
			get { throw new NotImplementedException(); }
		}
		
		public bool IsDirty {
			get {
				throw new NotImplementedException();
			}
		}
		
		public void RedrawContent()
		{
			throw new NotImplementedException();
		}
		
		public INavigationPoint BuildNavPoint()
		{
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
			throw new NotImplementedException();
		}
		
		public object GetService(Type serviceType)
		{
			return null;
		}
	}
}
