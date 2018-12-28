using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Extensions;
using System;
using System.Collections.Generic;
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

        public string NestPath(string path, string destination)
        {
            var lastSegment = path
                .TrimEnd(PathSeparator)
                .LastSegment(PathSeparator);

            return JoinPath(destination, lastSegment);
        }

        public string JoinPath(string path, string other)
        {
            return string.Concat(
                path.TrimEnd(PathSeparator),
                PathSeparator,
                other.Trim(PathSeparator),
                PathSeparator)
                .ToLower();
        }

        public string RenameLeaf(string path, string newName)
        {
            return path.ReplaceLastSegment(PathSeparator, newName)
                .EnsureSuffix(PathSeparator)
                .ToLower();
        }

        public string AddChild(string path, string name)
        {
            return path.AppendSegment(PathSeparator, name)
                .EnsureSuffix(PathSeparator)
                .ToLower();
        }

        public string GetParentPath(string path)
        {
            return path
                .RemoveLastSegment(PathSeparator)
                .EnsureSuffix(PathSeparator)
                .ToLower();
        }

        public string GetLeafName(string path)
        {
            return path
                .GetLastSegment(PathSeparator)
                .ToLower();
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
            return !value.EnsureSuffix(PathSeparator).Equals(ancestorPath.EnsureSuffix(PathSeparator), StringComparison.OrdinalIgnoreCase) &&
                value.EnsureSuffix(PathSeparator).StartsWith(ancestorPath.EnsureSuffix(PathSeparator), StringComparison.OrdinalIgnoreCase);
        }
    }
}
