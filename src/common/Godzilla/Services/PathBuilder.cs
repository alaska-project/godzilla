using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Godzilla.Services
{
    internal class PathBuilder<TContext> : IPathBuilder<TContext>
        where TContext : EntityContext
    {
        private readonly IGodzillaOptions<TContext> _options;

        public PathBuilder(IGodzillaOptions<TContext> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string PathSeparator => _options.PathSeparator;
        public string RootPath => PathSeparator.ToLower();

        public string NormalizePath(string path)
        {
            return path
                .EnsurePrefix(PathSeparator)
                .EnsureSuffix(PathSeparator)
                .ToLower();
        }

        public string MovePath(string path, string source, string destination)
        {
            var parent = GetParentPath(source);
            return path = destination + path.Substring(parent.Length);
        }

        public string NestPath(string path, string destination)
        {
            var lastSegment = path
                .TrimEnd(PathSeparator)
                .LastSegment(PathSeparator);

            return JoinPath(destination, lastSegment);
        }

        public string JoinPath(string path, string other)
        {
            return NormalizePath(
                string.Concat(
                    path.TrimEnd(PathSeparator),
                    PathSeparator,
                    other.Trim(PathSeparator),
                    PathSeparator));
        }

        public string RenameLeaf(string path, string newName)
        {
            return NormalizePath(
                path.ReplaceLastSegment(PathSeparator, newName)
                );
        }

        public string AddChild(string path, string name)
        {
            return NormalizePath(
                path.AppendSegment(PathSeparator, name)
                );
        }

        public string GetParentPath(string path)
        {
            return NormalizePath(path
                .RemoveLastSegment(PathSeparator)
                );
        }

        public string GetLeafName(string path)
        {
            return path
                .GetLastSegment(PathSeparator)
                .ToLower();
        }

        public IEnumerable<string> GetSegments(string path)
        {
            return path
                .Split(PathSeparator)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
        }

        public int GetPathLevel(string path)
        {
            return Regex.Matches(path.TrimEnd(PathSeparator), PathSeparator).Count;
        }

        public Regex GetDescendantsRegex(string path, int depth)
        {
            var normalizedPath = path.ToLower().EnsureSuffix(PathSeparator);
            return new Regex(string.Format("^{0}([^{1}]+{1}){{0,{2}}}",
                normalizedPath,
                PathSeparator,
                depth));
        }

        public bool IsAncestorPath(string value, string descendantPath)
        {
            return IsDescendantPath(descendantPath, value);
        }

        public bool IsDescendantPath(string value, string ancestorPath)
        {
            return !NormalizePath(value).Equals(NormalizePath(ancestorPath)) &&
                NormalizePath(value).StartsWith(NormalizePath(ancestorPath));
        }
    }
}
