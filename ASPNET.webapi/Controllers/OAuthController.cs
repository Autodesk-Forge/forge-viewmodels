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

using Autodesk.Forge.OAuth;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPISample.Controllers
{
  public class OAuthController : ApiController
  {
    [HttpGet]
    [Route("api/forge/oauth/token")]
    public async Task<string> Get()
    {
      OAuth oauth = await OAuth2LeggedToken.AuthenticateAsync(
        Config.FORGE_CLIENT_ID, Config.FORGE_CLIENT_SECRET,
        // only expose data:read access tokens as endpoints
        // this is required for Viewer
        new Scope[] { Scope.DataRead });

      return oauth.AccessToken;
    }
  }
}