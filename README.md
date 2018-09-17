# model.derivative-csharp.webapi-viewer.sample

![Platforms](https://img.shields.io/badge/platform-Windows-lightgray.svg)
![.NET](https://img.shields.io/badge/.NET-4.5.2-blue.svg)
[![ASP.NET](https://img.shields.io/badge/ASP.NET-4.5.2-blue.svg)](https://asp.net/)
[![License](http://img.shields.io/:license-mit-blue.svg)](http://opensource.org/licenses/MIT)

[![oAuth2](https://img.shields.io/badge/oAuth2-v1-green.svg)](http://developer.autodesk.com/)
[![Data-Management](https://img.shields.io/badge/Data%20Management-v1-green.svg)](http://developer.autodesk.com/)
[![OSS](https://img.shields.io/badge/OSS-v2-green.svg)](http://developer.autodesk.com/)
[![Model-Derivative](https://img.shields.io/badge/Model%20Derivative-v2-green.svg)](http://developer.autodesk.com/)
[![Viewer](https://img.shields.io/badge/Viewer-v3.3-green.svg)](http://developer.autodesk.com/)

# Description

This basic C# WebAPI back-end sample implements a basic list of Buckets and Objects with an [Autodesk Forge 2 Legged OAuth](https://developer.autodesk.com/en/docs/oauth/v2/tutorials/get-2-legged-token/). The front-end was desiged with pure HTML + JavaScript (jQuery, Bootstrap), no ASPx features (i.e. no WebForms or MVC on this sample). It should look like:

![thumbnail](/thumbnail.png)

 The Visual Studio solution includes 1 project: 

**1. ASPNET.Webapi**: WebAPI backend that expose specific endpoints to the fron-end (pude HTML + JavaScript) via Controllers.

### Live demo

Try it live at [modelderivative.apphb.com](http://modelderivative.apphb.com/) (hosted by [AppHarbor](https://appharbor.com/)). **Important**: this sample is public, therefore any file uploaded will be visible to others. 

# Setup

For using this sample, you need an Autodesk developer credentials. Visit the [Forge Developer Portal](https://developer.autodesk.com), sign up for an account, then [create an app](https://developer.autodesk.com/myapps/create) that uses Data Management and Model Derivative APIs. For this new app, use **http://localhost:3000/api/forge/callback/oauth** as Callback URL, although is not used on 2-legged flow. Finally take note of the **Client ID** and **Client Secret**.

## Run Locally

Open the **web.config** file and adjust the Forge Client ID & Secret. If you plan to deploy to Appharbor, configure the variables (no need to change this web.config file).

```xml
<appSettings>
  <add key="FORGE_CLIENT_ID" value="" />
  <add key="FORGE_CLIENT_SECRET" value="" />
</appSettings>
```

Compile the solution, Visual Studio should download the NUGET packages ([Autodesk Forge](https://www.nuget.org/packages/Autodesk.Forge/), [RestSharp](https://www.nuget.org/packages/RestSharp) and [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/))

Start the **ASPNET.webapi** project, the **index.html** is marked as start page. At the webpage, the **New Bucket** blue button allow create new buckets (as of now, minimum input validation is implemented). For any bucket, right-click to upload a file (objects). For demonstration, objects **are not** automatically translated, but right-click on a object and select **Translate**. If is a .ZIP file, it will request to type the root file.

# Deployment

For Appharbor deployment, following [this steps to configure your Forge Client ID & Secret](http://adndevblog.typepad.com/cloud_and_mobile/2017/01/deploying-forge-aspnet-samples-to-appharbor.html).

# Known issues

The **ASPNET.webapi** project is adding reference to Newtonsoft.Json library due a dependency from another library (jsTree), but this is not required and cause a conflict of versions. If it happens, you can safely remove this reference (from ASPNET.webapi project).

# License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT).
Please see the [LICENSE](LICENSE) file for full details.

## Written by

Augusto Goncalves [@augustomaia](https://twitter.com/augustomaia), [Forge Partner Development](http://forge.autodesk.com)
