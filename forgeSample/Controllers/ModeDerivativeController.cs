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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Autodesk.Forge;
using Autodesk.Forge.Model;

namespace forgeSample.Controllers
{
    [ApiController]
    public class ModelDerivativeController : ControllerBase
    {
        /// <summary>
        /// Start the translation job for a give bucketKey/objectName
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/forge/modelderivative/jobs")]
        public async Task<dynamic> TranslateObject([FromBody]ObjectModel objModel)
        {
            dynamic oauth = await OAuthController.GetInternalAsync();

            // prepare the payload
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
            if (string.IsNullOrEmpty(objModel.rootFilename))
                job = new JobPayload(new JobPayloadInput(objModel.objectName), new JobPayloadOutput(outputs));
            else
                job = new JobPayload(new JobPayloadInput(objModel.objectName, true, objModel.rootFilename), new JobPayloadOutput(outputs));


            // start the translation
            DerivativesApi derivative = new DerivativesApi();
            derivative.Configuration.AccessToken = oauth.access_token;
            dynamic jobPosted = await derivative.TranslateAsync(job, true/* force re-translate if already here, required data:write*/);
            return jobPosted;
        }

        [HttpDelete]
        [Route("api/forge/modelderivative/manifest")]
        public async Task<IActionResult> DeleteObjectAsync([FromBody]ObjectModel objectModel)
        {
            dynamic token = await OAuthController.GetInternalAsync();
            DerivativesApi derivative = new DerivativesApi();
            derivative.Configuration.AccessToken = token.access_token;
            await derivative.DeleteManifestAsync(objectModel.objectName);
            return Ok();
        }
    }

    public class ObjectModel
    {
        /// <summary>
        /// Model for TranslateObject method
        /// </summary>
        public string bucketKey { get; set; }
        public string objectName { get; set; }
        public string rootFilename { get; set; }
    }
}
