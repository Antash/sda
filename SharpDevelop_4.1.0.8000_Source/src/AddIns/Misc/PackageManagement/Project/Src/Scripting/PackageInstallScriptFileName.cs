﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using NuGet;

namespace ICSharpCode.PackageManagement.Scripting
{
	public class PackageInstallScriptFileName : PackageScriptFileName
	{
		public PackageInstallScriptFileName(string packageInstallDirectory)
			: base(packageInstallDirectory)
		{
		}
		
		public PackageInstallScriptFileName(IFileSystem fileSystem)
			: base(fileSystem)
		{
		}
		
		public override string Name {
			get { return "install.ps1"; }
		}
	}
}
