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

using Autodesk.Forge;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.IO;
using Autodesk.Forge.Model;
using System;
using static WebAPISample.Utility.Buckets;
using static WebAPISample.Utility.OAuth;

namespace WebAPISample.Controllers
{
  public class BucketsController : ApiController
  {
    /// <summary>
    /// Data model for CreateBucket end point
    /// </summary>
    public class CreateBucketModel
    {
      public string bucketKey { get; set; }
      public PostBucketsPayload.PolicyKeyEnum policyKey { get; set; }
      public Region region { get; set; }
    }

    [HttpPost]
    [Route("api/forge/buckets/createBucket")]
    public async Task<dynamic> CreateBucket([FromBody]CreateBucketModel bucket)
    {
      if (!Utility.Buckets.IsValidBucketKey(bucket.bucketKey)) return null;

      BucketsApi buckets = new BucketsApi();
      dynamic token = await Utility.OAuth.Get2LeggedTokenAsync(new Scope[] { Scope.BucketCreate });
      buckets.Configuration.AccessToken = token.access_token;
      PostBucketsPayload bucketPayload = new PostBucketsPayload(bucket.bucketKey, null, bucket.policyKey);
      return await buckets.CreateBucketAsync(bucketPayload, Enum.GetName(typeof(Region), bucket.region));
    }

    public class UploadObjectModel
    {
      public string bucketKey { get; set; }
      public HttpPostedFileBase fileToUpload { get; set; }
    }

    [HttpPost]
    [Route("api/forge/buckets/uploadObject")]
    public async Task<Object> UploadObject()//[FromBody]UploadObjectModel obj)
    {
      // basic input validation
      HttpRequest req = HttpContext.Current.Request;
      if (string.IsNullOrWhiteSpace(req.Params["bucketKey"]))
        throw new System.Exception("BucketKey parameter was not provided.");

      if (req.Files.Count != 1)
        throw new System.Exception("Missing file to upload"); // for now, let's support just 1 file at a time

      string bucketKey = req.Params["bucketKey"];
      HttpPostedFile file = req.Files[0];

      // save the file on the server
      var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), file.FileName);
      file.SaveAs(fileSavePath);

      // get the bucket...
      dynamic oauth = await Get2LeggedTokenAsync(new Scope[] { Scope.DataCreate, Scope.DataWrite });
      ObjectsApi objects = new ObjectsApi();
      objects.Configuration.AccessToken = oauth.access_token;

      // upload the file/object, which will create a new object
      dynamic uploadedObj;
      using (StreamReader streamReader = new StreamReader(fileSavePath))
      {
        uploadedObj = await objects.UploadObjectAsync(bucketKey,
               file.FileName, (int)streamReader.BaseStream.Length, streamReader.BaseStream,
               "application/octet-stream");
      }

      // cleanup
      File.Delete(fileSavePath);

      return uploadedObj;
    }
  }
}