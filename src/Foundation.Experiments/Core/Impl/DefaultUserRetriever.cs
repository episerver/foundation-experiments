using System;
using System.Collections.Generic;
using System.Web;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Shell.Security;
using Foundation.Experiments.Core.Config;
using Foundation.Experiments.Core.Interfaces;
using OptimizelySDK.Entity;

namespace Foundation.Experiments.Core.Impl
{
    public class DefaultUserRetriever : IUserRetriever
    {
        private readonly Lazy<UIRoleProvider> _roleProvider;
        private readonly Lazy<IVisitorGroupRepository> _visitorGroupRepository;
        private readonly Lazy<IVisitorGroupRoleRepository> _visitorGroupRoleRepository;
        private readonly object _padlock = new object();

        public DefaultUserRetriever(
            Lazy<UIRoleProvider> roleProvider,
            Lazy<IVisitorGroupRepository> visitorGroupRepository,
            Lazy<IVisitorGroupRoleRepository> visitorGroupRoleRepository)
        {
            _roleProvider = roleProvider;
            _visitorGroupRepository = visitorGroupRepository;
            _visitorGroupRoleRepository = visitorGroupRoleRepository;
        }

        public virtual string GetUserId(HttpContextBase httpContext)
        {
            lock (_padlock)
            {
                // Default implementation that store a user id in a cookie
                if (httpContext.Request.Cookies[DefaultKeys.CookieName] != null)
                    return httpContext.Request.Cookies[DefaultKeys.CookieName].Value;

                var userId = Guid.NewGuid().ToString();
                var cookie = new HttpCookie(DefaultKeys.CookieName, userId) { Secure = true };
                httpContext.Response.Cookies.Add(cookie);
                return userId;
            }
        }

        public virtual UserAttributes GetUserAttributes(HttpContextBase httpContext)
        {
            lock (_padlock)
            {
                if (httpContext.Items.Contains("ExperimentationUserData"))
                {
                    var attributes = httpContext.Items["ExperimentationUserData"] as UserAttributes;
                    return attributes;
                }

                var userAttributes = new UserAttributes
                {
                    {DefaultKeys.VisitorGroup, string.Join(",", GetUserVisitorGroups(httpContext))},
                    {DefaultKeys.UserRoles, string.Join(",", GetUserRoles(httpContext))}
                };

                if (httpContext?.User?.Identity != null)
                    userAttributes.Add(DefaultKeys.UserLoggedIn, httpContext.User.Identity.IsAuthenticated);
                else
                    userAttributes.Add(DefaultKeys.UserLoggedIn, false);

                httpContext.Items.Add("ExperimentationUserData", userAttributes);
                return userAttributes;
            }
        }

        public virtual List<string> GetUserVisitorGroups(HttpContextBase httpContext)
        {
            var visitorGroupId = new List<string>();
            var user = httpContext.User;
            var visitorGroups = _visitorGroupRepository.Value.List();
            foreach (var visitorGroup in visitorGroups)
            {
                if (_visitorGroupRoleRepository.Value.TryGetRole(visitorGroup.Name, out var virtualRoleObject))
                {
                    if (virtualRoleObject.IsMatch(user, httpContext))
                        visitorGroupId.Add(visitorGroup.Name);
                }
            }

            return visitorGroupId;
        }

        public virtual List<string> GetUserRoles(HttpContextBase httpContext)
        {
            try
            {
                if (httpContext?.User?.Identity != null
                    && httpContext.User.Identity.IsAuthenticated
                    && !String.IsNullOrEmpty(httpContext.User.Identity.Name))
                {
                    var username = httpContext.User.Identity.Name;
                    var roles = _roleProvider.Value.GetRolesForUser(username);
                    return new List<string>(roles);
                }
            }
            catch
            {
            }

            return new List<string>();
        }
    }
}
