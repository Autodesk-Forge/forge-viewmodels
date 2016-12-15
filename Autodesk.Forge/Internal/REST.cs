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

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Autodesk.Forge.Internal
{
  public class REST
  {
    private REST() { }

    internal static async Task<IRestResponse> MakeRequestAsync(string endPoint, RestSharp.Method method,
      Dictionary<string, string> headers = null, Dictionary<string, string> bodyParameters = null,
      object dataBody = null, String filePath = null)
    {
      var client = new RestClient(END_POINTS.BASE_URL);
      var request = new RestRequest(endPoint, method);

      // ToDo: create method overrides

      if (headers != null)
        foreach (KeyValuePair<string, string> item in headers)
          request.AddHeader(item.Key, item.Value);

      if (bodyParameters != null)
        foreach (KeyValuePair<string, string> item in bodyParameters)
          request.AddParameter(item.Key, item.Value);

      if (!string.IsNullOrWhiteSpace(filePath))
      {
        request.AddHeader("Content-Type", Utils.MimeType(filePath));
        request.AddHeader("Content-Disposition", string.Format("file; filename=\"{0}\"", Path.GetFileNameWithoutExtension(filePath)));
        request.AddParameter(Utils.MimeType(filePath), File.ReadAllBytes(filePath), ParameterType.RequestBody);
        //request.AddFile(Path.GetFileNameWithoutExtension(filePath), filePath);
        //request.AlwaysMultipartFormData = false;
        //request.AddHeader("Content-Type", MimeType(filePath));
      }

      if (dataBody != null)
        // the request.AddJsonBody() override the Content-Type header to RestSharp... this is a workaround
        request.AddParameter(headers["Content-Type"], JsonConvert.SerializeObject(dataBody), ParameterType.RequestBody);

      IRestResponse response = await client.ExecuteTaskAsync(request);

      return response;
    }

    /// <summary>
    /// Call the specified endPoint with the [Authorization: Beader TOKEN] heander
    /// </summary>
    internal static async Task<IRestResponse> MakeAuthorizedRequestAsync(OAuth.OAuth oauth, string endPoint, RestSharp.Method method,
       Dictionary<string, string> headers = null, Dictionary<string, string> bodyParameters = null,
       object dataBody = null, string file = null)
    {
      if (headers == null) headers = new Dictionary<string, string>();
      headers.Add("Authorization", "Bearer " + oauth.AccessToken);
      return await MakeRequestAsync(endPoint, method, headers, bodyParameters, dataBody, file);
    }
  }

  internal static class PredefinedHeadersExtension
  {
    public enum PredefinedHeaders
    {
      /// <summary>
      /// Content-Type: application/json
      /// </summary>
      ContentTypeJson,
      /// <summary>
      /// Content-Type: application/vnd.api+json
      /// </summary>
      ContentTypeVndApiJson,
      /// <summary>
      /// Content-Type: application/x-www-form-urlencoded
      /// </summary>
      ContentTypeFormUrlEncoded,
      /// <summary>
      /// Accept: application/vnd.api+json
      /// </summary>
      AcceptJson

    }

    internal static void AddHeader(this Dictionary<string, string> obj, PredefinedHeaders header)
    {
      switch (header)
      {
        case PredefinedHeaders.ContentTypeJson:
          obj.Add("Content-Type", "application/json");
          break;
        case PredefinedHeaders.ContentTypeVndApiJson:
          obj.Add("Content-Type", "application/vnd.api+json");
          break;
        case PredefinedHeaders.AcceptJson:
          obj.Add("Accept", "application/vnd.api+json");
          break;
        case PredefinedHeaders.ContentTypeFormUrlEncoded:
          obj.Add("Content-Type", "application/x-www-form-urlencoded");
          break;
      }
    }
  }
}
