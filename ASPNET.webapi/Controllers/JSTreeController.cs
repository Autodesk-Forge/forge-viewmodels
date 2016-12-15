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
using Autodesk.Forge.OAuth;
using Autodesk.Forge.OSS;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPISample.Controllers
{
  public class JSTreeController : ApiController
  {
    public class TreeNode
    {
      public TreeNode(string id, string text, string type, bool children)
      {
        this.id = id;
        this.text = text;
        this.type = type;
        this.children = children;
      }

      public string id { get; set; }
      public string text { get; set; }
      public string type { get; set; }
      public bool children { get; set; }
    }

    [HttpGet]
    [Route("api/forge/tree")]
    public async Task<IList<TreeNode>> GetTreeDataAsync([FromUri]string id)
    {
      IList<TreeNode> nodes = new List<TreeNode>();

      OAuth oauth = await OAuth2LeggedToken.AuthenticateAsync(Config.FORGE_CLIENT_ID, Config.FORGE_CLIENT_SECRET, new Scope[] { Scope.BucketRead, Scope.DataRead });
      if (id == "#") // root
      {
        // in this case, let's return all buckets
        AppBuckets appBuckets = new AppBuckets(oauth);
        IEnumerable<Bucket> buckets = await appBuckets.GetBucketsAsync(int.MaxValue);
        foreach (Bucket b in buckets)
          nodes.Add(new TreeNode(b.BucketKey, b.BucketKey, "bucket", true));
      }
      else
      {
        // as we have the id (bucketKey), let's return all objects
        Bucket bucket = new Bucket(oauth,  id/*bucketKey*/);
        IEnumerable<Autodesk.Forge.OSS.Object> objects = await bucket.GetObjectsAsync(int.MaxValue);
        foreach (Autodesk.Forge.OSS.Object obj in objects)
          nodes.Add(new TreeNode(obj.ObjectId.Base64Encode(), obj.ObjectKey, "object", false));
      }
      return nodes;
    }
  }
}