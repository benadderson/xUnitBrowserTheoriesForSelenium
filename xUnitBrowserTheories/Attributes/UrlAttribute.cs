using System;

namespace xUnitBrowserTheories
{
    /// <summary>
    /// Required when using [Browser(string browser)]
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    class URLAttribute: Attribute
    {
        public string Url;

        public URLAttribute(string testStartPageUrl)
        {
            Url = testStartPageUrl;
        }
    }
}
