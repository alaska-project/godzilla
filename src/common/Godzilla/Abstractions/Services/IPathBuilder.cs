using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Godzilla.Abstractions.Services
{
    public interface IPathBuilder<TContext> : IPathBuilder
        where TContext : EntityContext
    { }

    public interface IPathBuilder
    {
        string PathSeparator { get; }
        string RootPath { get; }
        string NormalizePath(string path);
        string MovePath(string path, string source, string destination);
        string NestPath(string path, string destination);
        string JoinPath(string path, string other);
        string RenameLeaf(string path, string newName);
        string AddChild(string path, string name);
        string GetParentPath(string path);
        string GetLeafName(string path);
        IEnumerable<string> GetSegments(string path);
        int GetPathLevel(string path);
        Regex GetDescendantsRegex(string path, int depth);
        bool IsAncestorPath(string value, string descendantPath);
        bool IsDescendantPath(string value, string ancestorPath);
    }
}
