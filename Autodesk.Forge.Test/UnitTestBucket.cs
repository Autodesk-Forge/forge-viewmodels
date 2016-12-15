/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Forge Partner Development
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

using Autodesk.Forge.ModelDerivative;
using Autodesk.Forge.OAuth;
using Autodesk.Forge.OSS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Autodesk.Forge.Test
{
  [TestClass]
  public class UnitTestBucket
  {
    private static string ForgeClientID { get; set; }
    private static string ForgeClientSecret { get; set; }

    [ClassInitialize]
    public static void TestClassInitialize(TestContext context)
    {
      ForgeClientID = context.Properties["FORGE_CLIENT_ID"].ToString();
      ForgeClientSecret = context.Properties["FORGE_CLIENT_SECRET"].ToString();
    }

    [TestMethod]
    public async Task OAuth()
    {
      // authenticate
      OAuth.OAuth oauth = await OAuth2LeggedToken.AuthenticateAsync(ForgeClientID, ForgeClientSecret,
        new Scope[] { Scope.BucketRead, Scope.BucketCreate, Scope.DataRead, Scope.DataCreate, Scope.DataWrite });
      Assert.IsFalse(string.IsNullOrWhiteSpace(oauth.AccessToken), "Access token not as expected");
      Assert.IsTrue(oauth.ExpiresAt > DateTime.Now);
    }

    [TestMethod]
    public async Task BucketWorkflow()
    {
      string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\TestFile\Analyze.dwf");
      Assert.IsTrue(File.Exists(testFile), "Test file not found");

      // authenticate
      OAuth.OAuth oauth = await OAuth2LeggedToken.AuthenticateAsync(ForgeClientID, ForgeClientSecret,
        new Scope[] { Scope.BucketRead, Scope.BucketCreate, Scope.DataRead, Scope.DataCreate, Scope.DataWrite });
      Assert.IsFalse(string.IsNullOrWhiteSpace(oauth.AccessToken), "Access token not as expected");

      // create bucket and get list of buckets in different conditions
      AppBuckets buckets = new AppBuckets(oauth);
      IEnumerable<Bucket> listOfBuckets = await buckets.GetBucketsAsync(10);

      buckets = new AppBuckets(oauth);
      await buckets.GetBucketsAsync(120);

      buckets = new AppBuckets(oauth);
      await buckets.GetBucketsAsync(210);

      // create a random bucket
      string bucketKey = string.Format("test{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
      Assert.IsTrue(BucketDetails.IsValidBucketKey(bucketKey));
      Bucket bucket = await buckets.CreateBucketAsync(bucketKey, PolicyKey.Transient);
      Assert.AreEqual(bucket.BucketKey, bucketKey);

      // get all objects
      IEnumerable<OSS.Object> objects = await bucket.GetObjectsAsync(int.MaxValue);

      // upload new object
      OSS.Object newObject = await bucket.UploadObjectAsync(testFile);

      // the list after should have 1 object...
      IEnumerable<OSS.Object> objectsAfter = await bucket.GetObjectsAsync(int.MaxValue);
      foreach (OSS.Object obj in objectsAfter)
        Assert.AreEqual(newObject.ObjectId, obj.ObjectId); // URNs should be the same

      // as there is just 1 object, bucket and object have the same size
      Assert.AreEqual(await bucket.Size(), newObject.Size, "Bucket size and object size don't match");

      // translate
      HttpStatusCode res = await newObject.Translate(new SVFOutput[] { SVFOutput.Views3d, SVFOutput.Views2d });
      Assert.AreEqual(res, HttpStatusCode.OK, "Translate job posted");
    }
  }
}
