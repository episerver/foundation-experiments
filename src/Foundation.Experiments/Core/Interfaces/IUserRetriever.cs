using System.Collections;
using System.Web;
using OptimizelySDK.Entity;

namespace Foundation.Experiments.Core.Interfaces
{
    /// <summary>
    /// Used to get the user Id and attributes of the user to be used in roll outs and experimentation
    /// </summary>
    public interface IUserRetriever
    {
        /// <summary>
        /// Returns the user Id of the current user if one is found, String.Empty if none found
        /// </summary>
        /// <returns></returns>
        string GetUserId(HttpContextBase httpContext);

        /// <summary>
        /// Returns attributes of the current user
        /// </summary>
        /// <returns></returns>
        UserAttributes GetUserAttributes(HttpContextBase httpContext);

        /// <summary>
        /// Returns attributes of the current user
        /// </summary>
        /// <returns></returns>
        UserAttributes GetUserAttributes(HttpContextBase httpContext, bool includeVisitorGroups);
    }
}