using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs_commitizen.Tests.TestAttributes;
using vs_commitizen.vs.Extensions;
using Xunit;

namespace vs_commitizen.Tests.Extensions
{
    public class StringExtensionsTests
    {
        const string LP = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis efficitur risus nec erat varius, ornare accumsan diam ornare. Etiam ornare augue ut tempor vestibulum. Nullam libero metus, porta sollicitudin sapien sed, sollicitudin malesuada mauris. Curabitur hendrerit fermentum faucibus. Nullam turpis felis, interdum eu feugiat non, iaculis eget nisi. Praesent finibus volutpat nulla, quis accumsan metus congue eu. Praesent pulvinar auctor metus, in efficitur tortor congue a. Pellentesque sollicitudin mattis metus, quis mollis lectus tempor sed. Vivamus a gravida lacus. Vivamus volutpat ac erat nec rutrum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae;";

        [Fact, TestConventions]
        public void Split_At_Words_End()
        {
            var res = LP.ChunkBySizePreverveWords(50);
            res.First().ShouldBe("Lorem ipsum dolor sit amet, consectetur adipiscing");
        }

        [Fact, TestConventions]
        public void Remove_Leading_Space()
        {
            var res = LP.ChunkBySizePreverveWords(50);
            res.First().ShouldBe("Lorem ipsum dolor sit amet, consectetur adipiscing");
            res.Skip(1).First().ShouldStartWith("elit. Duis");
        }

        [Fact, TestConventions]
        public void Split_At_LeadingSpace_If_Word_Too_Long()
        {
            var res = "Lorem ipsum dolor sit amet, consectetur adipiscingzomgitstoolong!".ChunkBySizePreverveWords(50);
            res.ShouldBe(new[] { "Lorem ipsum dolor sit amet, consectetur", "adipiscingzomgitstoolong!" });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Returns_Empty_For_Null_Empty(string str)
        {
            StringExtensions.ChunkBySizePreverveWords(str, 0).ShouldBeEmpty();
        }

        [Fact, TestConventions]
        public void Split_By_1()
        {
            var res = "Lorem ipsum ".ChunkBySizePreverveWords(1);
            res.ShouldBe(new[] { "L", "o", "r", "e", "m", "i", "p", "s", "u", "m" });
        }
    }
}
