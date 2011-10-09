﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.UnitTesting;
using NUnit.Framework;
using UnitTesting.Tests.Utils;

namespace UnitTesting.Tests.Project
{
	[TestFixture]
	public class TestClassIsTestMethodUsesTestFrameworksTestFixture
	{
		TestClass testClass;
		MockClass mockTestClass;
		MockMethod testMethod;
		MockRegisteredTestFrameworks testFrameworks;
		MockClass mockBaseTestClass;
		MockMethod baseClassTestMethod;
		
		[SetUp]
		public void Init()
		{
			mockTestClass = MockClass.CreateMockClassWithoutAnyAttributes();
			mockTestClass.FullyQualifiedName = "DerivedClass";
			
			testMethod = new MockMethod(mockTestClass, "myTestMethod");
			mockTestClass.Methods.Add(testMethod);
			baseClassTestMethod = new MockMethod(mockTestClass, "myBaseTestMethod");
			
			testFrameworks = new MockRegisteredTestFrameworks();
			testFrameworks.AddTestMethod(testMethod);
			testFrameworks.AddTestMethod(baseClassTestMethod);

			mockBaseTestClass = MockClass.CreateMockClassWithoutAnyAttributes();
			mockBaseTestClass.FullyQualifiedName = "BaseClass";
			mockBaseTestClass.Methods.Add(baseClassTestMethod);
			
			mockTestClass.AddBaseClass(mockBaseTestClass);
			
			testClass = new TestClass(mockTestClass, testFrameworks);
		}
		
		[Test]
		public void TestClassHasTestMethod()
		{
			Assert.AreEqual(testMethod, testClass.TestMethods[0].Method);
		}
		
		[Test]
		public void TestClassHasBaseClassTestMethod()
		{
			BaseTestMethod baseTestMethod = testClass.TestMethods[1].Method as BaseTestMethod;
			Assert.AreEqual(baseClassTestMethod, baseTestMethod.Method);
		}
	}
}
