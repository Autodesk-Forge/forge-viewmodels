using Autodesk.Forge;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAPISample.Utility
{
  public static class OAuth
  {
    public static async Task<dynamic> Get2LeggedTokenAsync(Scope[] scopes)
    {
      TwoLeggedApi apiInstance = new TwoLeggedApi();
      string grantType = "client_credentials";
      dynamic bearer = await apiInstance.AuthenticateAsync(Config.FORGE_CLIENT_ID, Config.FORGE_CLIENT_SECRET, grantType, scopes);
      return bearer;
    }
  }
}