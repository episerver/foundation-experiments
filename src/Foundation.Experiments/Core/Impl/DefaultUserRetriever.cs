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
        private readonly Lazy<HttpContextBase> _httpContext;
        private readonly Lazy<UIRoleProvider> _roleProvider;
        private readonly Lazy<IVisitorGroupRepository> _visitorGroupRepository;
        private readonly Lazy<IVisitorGroupRoleRepository> _visitorGroupRoleRepository;
        private readonly object _padlock = new object();

        public DefaultUserRetriever(Lazy<HttpContextBase> httpContext,
            Lazy<UIRoleProvider> roleProvider,
            Lazy<IVisitorGroupRepository> visitorGroupRepository,
            Lazy<IVisitorGroupRoleRepository> visitorGroupRoleRepository)
        {
            _httpContext = httpContext;
            _roleProvider = roleProvider;
            _visitorGroupRepository = visitorGroupRepository;
            _visitorGroupRoleRepository = visitorGroupRoleRepository;
        }

        public virtual string GetUserId()
        {
            lock (_padlock)
            {
                // Default implementation that store a user id in a cookie
                if (_httpContext.Value.Request.Cookies[DefaultKeys.CookieName] != null)
                    return _httpContext.Value.Request.Cookies[DefaultKeys.CookieName].Value;

                var userId = Guid.NewGuid().ToString();
                var cookie = new HttpCookie(DefaultKeys.CookieName, userId) { Secure = true };
                _httpContext.Value.Response.Cookies.Add(cookie);
                return userId;
            }
        }

        public virtual UserAttributes GetUserAttributes()
        {
            lock (_padlock)
            {
                if (_httpContext.Value.Items.Contains("ExperimentationUserData"))
                {
                    var attributes = _httpContext.Value.Items["ExperimentationUserData"] as UserAttributes;
                    return attributes;
                }

                var userAttributes = new UserAttributes
                {
                    {DefaultKeys.VisitorGroup, string.Join(",", GetUserVisitorGroups())},
                    {DefaultKeys.UserRoles, string.Join(",", GetUserRoles())}
                };

                if (_httpContext?.Value.User?.Identity != null)
                    userAttributes.Add(DefaultKeys.UserLoggedIn, _httpContext.Value.User.Identity.IsAuthenticated);
                else
                    userAttributes.Add(DefaultKeys.UserLoggedIn, false);

                _httpContext.Value.Items.Add("ExperimentationUserData", userAttributes);
                return userAttributes;
            }
        }

        public virtual List<string> GetUserVisitorGroups()
        {
            var visitorGroupId = new List<string>();
            var user = _httpContext.Value.User;
            var visitorGroups = _visitorGroupRepository.Value.List();
            foreach (var visitorGroup in visitorGroups)
            {
                if (_visitorGroupRoleRepository.Value.TryGetRole(visitorGroup.Name, out var virtualRoleObject))
                {
                    if (virtualRoleObject.IsMatch(user, _httpContext.Value))
                        visitorGroupId.Add(visitorGroup.Name);
                }
            }

            return visitorGroupId;
        }

        public virtual List<string> GetUserRoles()
        {
            try
            {
                if (_httpContext?.Value.User?.Identity != null
                    && _httpContext.Value.User.Identity.IsAuthenticated
                    && !String.IsNullOrEmpty(_httpContext.Value.User.Identity.Name))
                {
                    var username = _httpContext.Value.User.Identity.Name;
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
