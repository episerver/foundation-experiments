using OptimizelyFullStackContentProvider;
using System;
using System.Linq;

namespace Optimizely.DeveloperFullStack
{
    internal class FullStackResourceId
    {
        public FullStackResourceId()
        {
        }

        public FullStackResourceId(long projectId, FullStackType fullStackType, long objectId)
          : this(projectId.ToString(), fullStackType, objectId.ToString())
        {
        }

        public FullStackResourceId(string projectId, FullStackType fullStackType, string objectId)
        {
            ProjectId = projectId;
            FullStackType = fullStackType;
            Id = objectId;
        }

        public FullStackResourceId(string projectId, FullStackType fullStackType, string objectId, string key)
            : this(projectId, fullStackType, objectId)
        {
            Key = key;
        }

        public string ProjectId { get; set; }

        public FullStackType FullStackType { get; set; } = FullStackType.Folder;

        public string Id { get; set; }

        public string Key { get; set; }

        public static implicit operator string(FullStackResourceId id) =>
            id.ToString();

        public static explicit operator FullStackResourceId(Uri uri)
        {
            if (uri == null)
                return null;

            var resource = new FullStackResourceId()
            {
                ProjectId = RemoveTrailingSlash(uri.Segments[1]),
                FullStackType = (FullStackType)Enum.Parse(typeof(FullStackType), RemoveTrailingSlash(uri.Segments[2])),
                Id = RemoveTrailingSlash(uri.Segments[3])
            };

            if (uri.Segments.Count() == 5)
                resource.Key = uri.Segments[4];

            return resource;
        }

        public override string ToString() =>
            string.IsNullOrWhiteSpace(Key) ? $"{this.ProjectId}/{this.FullStackType}/{Id}" : $"{ProjectId}/{FullStackType}/{Id}/{Key}";

        private static string RemoveTrailingSlash(string virtualPath) =>
            !string.IsNullOrEmpty(virtualPath) && virtualPath[virtualPath.Length - 1] == '/' ? virtualPath.Substring(0, virtualPath.Length - 1) : virtualPath;
    }
}