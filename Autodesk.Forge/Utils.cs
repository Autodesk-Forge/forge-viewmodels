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

using System.Collections.Generic;
using System.IO;

namespace Autodesk.Forge
{
  public static class Utils
  {
    /// <summary>
    /// Get the MIME type for known industry files, such as Revit, AutoCAD, Inventor and Fusion. For other formats, use the standard application/EXTESION
    /// </summary>
    /// <param name="fileName">File name, with extension</param>
    /// <returns>MIME type string</returns>
    public static string MimeType(string fileName)
    {
      Dictionary<string, string> types = new Dictionary<string, string>();
      types.Add("png", "application/image");
      types.Add("jpg", "application/image");
      types.Add("txt", "application/txt");
      types.Add("ipt", "application/vnd.autodesk.inventor.part");
      types.Add("iam", "application/vnd.autodesk.inventor.assembly");
      types.Add("dwf", "application/vnd.autodesk.autocad.dwf");
      types.Add("dwg", "application/vnd.autodesk.autocad.dwg");
      types.Add("f3d", "application/vnd.autodesk.fusion360");
      types.Add("f2d", "application/vnd.autodesk.fusiondoc");
      types.Add("rvt", "application/vnd.autodesk.revit");
      string extension = Path.GetExtension(fileName).Replace(".", string.Empty);
      return (types.ContainsKey(extension) ? types[extension] : "application/" + extension);
    }

    /// <summary>
    /// Base64 encode a string (source: http://stackoverflow.com/a/11743162)
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Base64Encode(this string plainText)
    {
      var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
      return System.Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    /// Base64 dencode a string (source: http://stackoverflow.com/a/11743162)
    /// </summary>
    /// <param name="base64EncodedData"></param>
    /// <returns></returns>
    public static string Base64Decode(this string base64EncodedData)
    {
      var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
      return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
  }
}
