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
using Autodesk.Forge.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using static WebAPISample.Utility.OAuth;

namespace WebAPISample.Controllers
{
  public class ModelDerivativeController : ApiController
  {
    public class TranslateObjectModel
    {
      public string bucketKey { get; set; }
      public string objectKey { get; set; }
      public string rootFilename { get; set; }
    }


    [HttpPost]
    [Route("api/forge/modelderivative/translateObject")]
    public async Task<dynamic> TranslateObject([FromBody]TranslateObjectModel objModel)
    {
      dynamic oauth = await Utility.OAuth.Get2LeggedTokenAsync(new Scope[] { Scope.DataRead, Scope.DataWrite, Scope.DataCreate });

      List<JobPayloadItem> outputs = new List<JobPayloadItem>()
      {
       new JobPayloadItem(
         JobPayloadItem.TypeEnum.Svf,
         new List<JobPayloadItem.ViewsEnum>()
         {
           JobPayloadItem.ViewsEnum._2d,
           JobPayloadItem.ViewsEnum._3d
         })
      };
      JobPayload job;
      if (string.IsNullOrEmpty( objModel.rootFilename))
         job = new JobPayload(new JobPayloadInput(objModel.objectKey), new JobPayloadOutput(outputs));
      else
        job = new JobPayload(new JobPayloadInput(objModel.objectKey, true, objModel.rootFilename), new JobPayloadOutput(outputs));


      DerivativesApi derivative = new DerivativesApi();
      derivative.Configuration.AccessToken = oauth.access_token;
      dynamic jobPosted = await derivative.TranslateAsync(job);
      return jobPosted;
    }
  }
}