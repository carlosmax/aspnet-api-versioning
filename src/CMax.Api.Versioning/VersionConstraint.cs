using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Text.RegularExpressions;

namespace CMax.Api.Versioning
{
    /// <summary>
    /// A Constraint implementation that matches an HTTP header against an expected version value.  Matches
    /// both custom request header ("api-version") and custom content type vnd.myservice.vX+json (or other dt type)
    /// 
    /// Adapted from ASP .NET samples
    /// </summary>
    public class VersionConstraint : IActionConstraint, IActionConstraintMetadata
    {
        private const string VersionHeaderName = "api-version";
        private const int DefaultVersion = 1;

        public VersionConstraint(int allowedVersion)
        {
            AllowedVersion = allowedVersion;
        }

        public int AllowedVersion { get; private set; }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var headers = context.RouteContext.HttpContext.Request.Headers;

            // try custom request header "api-version"
            int? version = GetVersionFromCustomRequestHeader(headers);

            // not found?  Try custom content type in accept header
            if (version == null)
            {
                version = GetVersionFromCustomContentType(headers);
            }

            return ((version ?? DefaultVersion) == AllowedVersion);
        }

        private int? GetVersionFromCustomContentType(IHeaderDictionary header)
        {
            string versionAsString = null;

            // get the accept header.
            var mediaTypes = header.GetCommaSeparatedValues("accept");
            string matchingMediaType = null;

            // find the one with the version number - match through regex
            Regex regEx = new Regex(@"application\/vnd\.api\.v([\d]+)\+json");

            foreach (var mediaType in mediaTypes)
            {
                if (regEx.IsMatch(mediaType))
                {
                    matchingMediaType = mediaType;
                    break;
                }
            }

            if (matchingMediaType == null)
                return null;

            // extract the version number
            Match m = regEx.Match(matchingMediaType);
            versionAsString = m.Groups[1].Value;

            // ... and return
            int version;
            if (versionAsString != null && int.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }

        private int? GetVersionFromCustomRequestHeader(IHeaderDictionary header)
        {
            string versionAsString;
            StringValues headerValues;
            if (header.TryGetValue(VersionHeaderName, out headerValues) && headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
            }
            else
            {
                return null;
            }

            int version;
            if (versionAsString != null && int.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }
    }
}