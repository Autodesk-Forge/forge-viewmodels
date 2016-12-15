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
using Autodesk.Forge.ModelDerivative;
using Autodesk.Forge.OAuth;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPISample.Controllers
{
  public class ModelDerivativeController : ApiController
  {
    public class TranslateObjectModel
    {
      public string bucketKey { get; set; }
      public string objectKey { get; set; }
    }

    private async Task<OAuth> GetOAuth(Scope[] scope)
    {
      OAuth oauth = await OAuth2LeggedToken.AuthenticateAsync(
        Config.FORGE_CLIENT_ID, Config.FORGE_CLIENT_SECRET,
        (scope == null ? Config.FORGE_SCOPE_PUBLIC : scope));
      return oauth;
    }

    [HttpPost]
    [Route("api/forge/modelderivative/translateObject")]
    public async Task<HttpStatusCode> TranslateObject([FromBody]TranslateObjectModel objModel)
    {
      Autodesk.Forge.OSS.Object obj = new Autodesk.Forge.OSS.Object(
        await GetOAuth(new Scope[] { Scope.DataRead, Scope.DataWrite, Scope.DataCreate }),
        objModel.bucketKey, objModel.objectKey.Base64Decode());

      return await obj.Translate(new SVFOutput[] { SVFOutput.Views2d, SVFOutput.Views3d });
    }
  }
}