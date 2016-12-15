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
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Autodesk.Forge.OAuth
{
  public enum Scope
  {
    DataRead,
    DataCreate,
    DataWrite,
    BucketRead,
    BucketCreate

    //public static string DataRead { get { return "data:read"; } }
    //public static string BucketRead { get { return "bucket:read"; } }
  }

  public abstract class OAuth
  {
    internal static string ToString(Scope[] listOfScope)
    {
      var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

      StringBuilder scopeAsString = new StringBuilder();
      foreach (Scope scope in listOfScope) scopeAsString.AppendFormat("{0} ", r.Replace(Enum.GetName(typeof(Scope), scope), ":"));

      return scopeAsString.ToString().ToLower().TrimEnd();
    }

    [JsonProperty("token_type")]
    public string TokenType { get; internal set; }

    private DateTime _expiresAt;
    public DateTime ExpiresAt
    {
      get
      {
        return _expiresAt;
      }
    }

    [JsonProperty("expires_in")]
    private long ExpiresIn
    {
      get
      {
        return _expiresAt.Ticks;
      }
      set
      {
        _expiresAt = DateTime.Now.AddSeconds(value);
      }
    }
    
    private string _accessToken;

    [JsonProperty("access_token")]
    public string AccessToken
    {
      get
      {
        if (_expiresAt < DateTime.Now)
          throw new Exception("Token expired.");
        return _accessToken;
      }
      internal set
      {
        _accessToken = value;
      }
    }
  }
}
