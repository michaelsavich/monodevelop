//
// RepositoryTests.cs
//
// Author:
//       Therzok <teromario@yahoo.com>
//
// Copyright (c) 2013 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using MonoDevelop.Core;
using MonoDevelop.VersionControl.Subversion;
using MonoDevelop.VersionControl.Subversion.Unix;
using NUnit.Framework;
using System.IO;
using System;
using MonoDevelop.VersionControl;

namespace VersionControl.Subversion.Unix.Tests
{
	[TestFixture]
	public class UnixSvnUtilsTest : MonoDevelop.VersionControl.Subversion.Tests.BaseSvnUtilsTest
	{
		[SetUp]
		public override void Setup ()
		{
			rootUrl = new FilePath (FileService.CreateTempDirectory ());
			repoLocation = "file://" + rootUrl + "/repo";
			base.Setup ();
		}

		[Test]
		public override void DiffIsProper ()
		{
			string added = rootCheckout + "testfile";
			File.Create (added).Close ();
			repo.Add (added, false, new MonoDevelop.Core.ProgressMonitoring.NullProgressMonitor ());
			ChangeSet changes = repo.CreateChangeSet (repo.RootPath);
			changes.AddFile (added);
			changes.GlobalComment = "File committed";
			repo.Commit (changes, new MonoDevelop.Core.ProgressMonitoring.NullProgressMonitor ());
			File.AppendAllText (added, "text" + Environment.NewLine);

			string difftext = @"--- testfile	(revision 1)
+++ testfile	(working copy)
@@ -0,0 +1 @@
+text
";
			Assert.AreEqual (difftext, repo.GenerateDiff (added, repo.GetVersionInfo (added)).Content);
		}

		protected override Repository GetRepo (string path, string url)
		{
			return new SubversionRepository (new SvnClient (), url, path);
		}
	}
}

