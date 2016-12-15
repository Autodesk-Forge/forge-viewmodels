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

using Autodesk.Forge.OSS;
using Autodesk.Forge.OAuth;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.IO;

namespace WebAPISample.Controllers
{
  public class BucketsController : ApiController
  {
    private async Task<OAuth> GetOAuth(Scope[] scope)
    {
      OAuth oauth = await OAuth2LeggedToken.AuthenticateAsync(
        Config.FORGE_CLIENT_ID, Config.FORGE_CLIENT_SECRET,
        (scope == null ? Config.FORGE_SCOPE_PUBLIC : scope));
      return oauth;
    }

    /// <summary>
    /// Data model for CreateBucket end point
    /// </summary>
    public class CreateBucketModel
    {
      public string bucketKey { get; set; }
      public PolicyKey policyKey { get; set; }
      public Region region { get; set; }
    }

    [HttpPost]
    [Route("api/forge/buckets/createBucket")]
    public async Task<BucketDetails> CreateBucket([FromBody]CreateBucketModel bucket)
    {
      if (!Bucket.IsValidBucketKey(bucket.bucketKey)) return null;

      AppBuckets buckets = new AppBuckets(await GetOAuth(new Scope[] { Scope.BucketCreate }));
      return await buckets.CreateBucketAsync(bucket.bucketKey, bucket.policyKey, bucket.region);
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
      Bucket bucket = new Bucket(await GetOAuth(new Scope[] { Scope.DataCreate, Scope.DataWrite }), bucketKey);
      // upload the file/object, which will create a new object
      Object newObj = await bucket.UploadObjectAsync(fileSavePath);

      // cleanup
      File.Delete(fileSavePath);

      return newObj;
    }


    #region Demonstration endpoints, not used on this sample

    [HttpGet]
    [Route("api/forge/buckets")]
    public async Task<IEnumerable<Bucket>> GetBuckets([FromUri]int limit = 100, [FromUri]Region region = Region.US, [FromUri]string startAt = "")
    {
      OAuth oauth = await GetOAuth(new Scope[] { Scope.BucketRead });
      AppBuckets buckets = new AppBuckets(oauth);
      return await buckets.GetBucketsAsync(limit, region, startAt);
    }

    [HttpGet]
    [Route("api/forge/buckets/{bucketKey}/details")]
    public async Task<BucketDetails> GetBucket(string bucketKey)
    {
      OAuth oauth = await GetOAuth(new Scope[] { Scope.BucketRead });
      BucketDetails bucket = await BucketDetails.InitializeAsync(oauth, bucketKey);
      return bucket;
    }

    [HttpGet]
    [Route("api/forge/buckets/{bucketKey}/objects")]
    public async Task<IEnumerable<Autodesk.Forge.OSS.Object>> GetObjects(string bucketKey, [FromUri]int limit = 100, [FromUri]string startAt = "")
    {
      OAuth oauth = await GetOAuth(new Scope[] { Scope.DataRead });
      Bucket bucket = new Bucket(oauth, bucketKey);
      return await bucket.GetObjectsAsync(limit, startAt);
    }

    #endregion

  }
}