using Godzilla.Abstractions;
using Godzilla.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Services
{
    public class PathBuilder_Tests
    {
        private Mock<IGodzillaOptions<FakeEntityContext>> _options = new Mock<IGodzillaOptions<FakeEntityContext>>();
        private PathBuilder<FakeEntityContext> _pathBuilder;

        public PathBuilder_Tests()
        {
            _options
                .Setup(x => x.PathSeparator)
                .Returns("/");

            _pathBuilder = new PathBuilder<FakeEntityContext>(_options.Object);
        }

        [Fact]
        public void Test_root_path()
        {
            var rootPath = _pathBuilder.RootPath;

            Assert.Equal("/", rootPath);
        }

        [Fact]
        public void Test_join_path()
        {
            var path = _pathBuilder.JoinPath("/root/child1", "child2");

            Assert.Equal("/root/child1/child2/", path);
        }

        [Fact]
        public void Test_nest_path()
        {
            var path = _pathBuilder.NestPath("/root/child1/child2", "/root-new/child-new");

            Assert.Equal("/root-new/child-new/child2/", path);
        }

        [Fact]
        public void Test_rename_leaf()
        {
            var path = _pathBuilder.RenameLeaf("/root/child1/child2", "child2-new");

            Assert.Equal("/root/child1/child2-new/", path);
        }

        [Fact]
        public void Test_add_child()
        {
            var path = _pathBuilder.AddChild("/root/child1/child2", "child3");

            Assert.Equal("/root/child1/child2/child3/", path);
        }

        [Fact]
        public void Test_get_parent_path()
        {
            var path = _pathBuilder.GetParentPath("/root/child1/child2");

            Assert.Equal("/root/child1/", path);
        }

        [Fact]
        public void Test_get_leaf_name()
        {
            var name = _pathBuilder.GetLeafName("/root/child1/child2");

            Assert.Equal("child2", name);
        }

        [Fact]
        public void Test_get_path_level()
        {
            var level = _pathBuilder.GetPathLevel("/root/child1/child2");

            Assert.Equal(3, level);
        }

        [Fact]
        public void Test_is_ancestor_path()
        {
            var value = _pathBuilder.IsAncestorPath("/root/child1", "/root/child1/child2");

            Assert.True(value);
        }

        [Fact]
        public void Test_is_not_ancestor_path()
        {
            var value = _pathBuilder.IsAncestorPath("/root/child1", "/root/child1-new/child2");

            Assert.False(value);
        }

        [Fact]
        public void Test_is_descendant_path()
        {
            var value = _pathBuilder.IsDescendantPath("/root/child1/child2", "/root/child1");

            Assert.True(value);
        }

        [Fact]
        public void Test_is_not_descendant_path()
        {
            var value = _pathBuilder.IsDescendantPath("/root/child1-new/child2", "/root/child1");

            Assert.False(value);
        }
    }
}
