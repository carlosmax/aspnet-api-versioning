using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace CMax.Api.Versioning
{
    public class VersionedRoute : RouteAttribute, IActionConstraintFactory
    {
        private readonly IActionConstraint _constraint;

        public VersionedRoute(string template, int allowedVersion = 1) : base(template)
        {
            Order = -10;
            _constraint = new VersionConstraint(allowedVersion);
        }

        public bool IsReusable => true;

        public IActionConstraint CreateInstance(IServiceProvider services)
        {
            return _constraint;
        }
    }
}
