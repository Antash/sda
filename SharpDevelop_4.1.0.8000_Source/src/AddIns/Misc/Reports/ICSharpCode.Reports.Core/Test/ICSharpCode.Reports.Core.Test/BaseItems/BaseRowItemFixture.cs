﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.Reports.Core.Exporter;
using NUnit.Framework;

namespace ICSharpCode.Reports.Core.Test.BaseItems
{
	[TestFixture]
	public class BaseRowItemFixture
	{
		[Test]
		public void DefaultConstructur()
		{
			BaseRowItem bri = new BaseRowItem();
			Assert.IsNotNull(bri,"baseRowItem should not be null");
		}
		
		[Test]
		public void DefaultSettings()
		{
			BaseRowItem bri = new BaseRowItem();
			Assert.IsNotNull(bri.Items,"Items should not be 'null'");
			Assert.AreEqual(0,bri.Items.Count,"Items.Count should be '0'");
		}
		
		[Test] 
		public void ToStringOverride ()
		{
			BaseRowItem bri = new BaseRowItem();
			Assert.AreEqual ("BaseRowItem",bri.ToString(),"ToString override");
		}
		
		#region Exporter
		
		[Test]
		public void ExportColumnIsNotNull ()
		{
			BaseRowItem bri = new BaseRowItem();
			var bec = bri.CreateExportColumn();
			Assert.IsNotNull(bec);
		}
		
		[Test]
		public void TypeofExportShouldBeExportContainer ()
		{
			BaseRowItem bri = new BaseRowItem();
			var bec = bri.CreateExportColumn();
			Type t = typeof(ExportContainer);
			Assert.AreEqual(t,bec.GetType(),"Type should be 'ExportContainer");
		}
		
		[Test]
		public void RowValuesEqualExportedText ()
		{
			BaseRowItem bri = new BaseRowItem();
			ExportContainer ec = (ExportContainer)bri.CreateExportColumn();
			Assert.AreEqual (0,ec.Items.Count,"Items.Count should BeforePrintEventArgs '0'");
			Assert.AreEqual(bri.Size,ec.StyleDecorator.Size);                 
		}
		#endregion
	}
}
