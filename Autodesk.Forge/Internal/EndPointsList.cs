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

namespace Autodesk.Forge.Internal
{
  public static class END_POINTS
  {
    /// <summary>
    /// Base URL for all API/endpoint calls
    /// </summary>
    public const string BASE_URL =            "https://developer.api.autodesk.com";

    /// <summary>
    /// End point to list buckets
    /// </summary>
    public const string GET_BUCKETS =         "/oss/v2/buckets";

    /// <summary>
    /// Endpoint for details of a specific bucket.
    /// /oss/v2/buckets/:bucketKey/details
    /// </summary>
    public const string GET_BUCKET_DETAILS =  "/oss/v2/buckets/{0}/details";

    /// <summary>
    /// Endpoint to create new buckets
    /// </summary>
    public const string POST_BUCKETS =        "/oss/v2/buckets";

    /// <summary>
    /// Endpoint to list all objects inside a bucket.
    /// /oss/v2/buckets/:bucketKey/objects
    /// </summary>
    public const string GET_BUCKET_OBJECTS =  "/oss/v2/buckets/{0}/objects";

    /// <summary>
    /// Endpoint to upload an object (file).
    /// /oss/v2/buckets/:bucketKey/objects/:objectName
    /// </summary>
    public const string PUT_BUCKET_OBJECT = "oss/v2/buckets/{0}/objects/{1}";

    public const string POST_JOB = "/modelderivative/v2/designdata/job";
  }
}
