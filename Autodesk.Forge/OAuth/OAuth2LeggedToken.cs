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

using Autodesk.Forge.Internal;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Autodesk.Forge.OAuth
{
  public class OAuth2LeggedToken : OAuth
  {
    async public static Task<OAuth2LeggedToken> AuthenticateAsync(string clientID, string clientSecret, Scope[] scope)
    {
      Dictionary<string, string> headers = new Dictionary<string, string>();
      headers.AddHeader(PredefinedHeadersExtension.PredefinedHeaders.ContentTypeFormUrlEncoded);

      Dictionary<string, string> parameters = new Dictionary<string, string>();
      parameters.Add("client_id", clientID);
      parameters.Add("client_secret", clientSecret);
      parameters.Add("grant_type", "client_credentials");
      parameters.Add("scope", ToString(scope));

      IRestResponse response = await REST.MakeRequestAsync("authentication/v1/authenticate", RestSharp.Method.POST, headers, parameters);
      OAuth2LeggedToken token = JsonConvert.DeserializeObject<OAuth2LeggedToken>(response.Content);
      return token;
    }
  }
}
