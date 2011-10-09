﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.PythonBinding;
using IronPython.Hosting;
using NUnit.Framework;

namespace PythonBinding.Tests.Expressions
{
	[TestFixture]
	public class ParseImportSystemExpressionTestFixture
	{
		PythonImportExpression importExpression;
		
		[SetUp]
		public void Init()
		{
			string text = "import System";
			importExpression = new PythonImportExpression(Python.CreateEngine(), text);
		}
		
		[Test]
		public void ModuleNameIsSystem()
		{
			Assert.AreEqual("System", importExpression.Module);
		}
		
		[Test]
		public void HasFromAndImportIsFalse()
		{
			Assert.IsFalse(importExpression.HasFromAndImport);
		}
	}
}
