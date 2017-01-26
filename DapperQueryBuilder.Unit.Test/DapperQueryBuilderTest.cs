using FluentAssertions;
using Xunit;

namespace DapperQueryBuilder.Unit.Test
{
    public class DapperQueryBuilderTest
    {
        private Core.DapperQueryBuilder _dapperQueryBuilder;

        public DapperQueryBuilderTest()
        {
            _dapperQueryBuilder = new Core.DapperQueryBuilder();
        }

        [Fact]
        public void WhenCallSelectMethodShoulCreateCorrectQuery()
        {
            _dapperQueryBuilder.Select("User");
            _dapperQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT * FROM User");
        }

        [Fact]
        public void WhenCallSelectWithOneColumnNameShouldCreateCorrectQuery()
        {
            _dapperQueryBuilder.Select("User")
                .WithColumn("Name");

            _dapperQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT Name FROM User");
        }

        [Fact]
        public void WhenCallSelectWithTwoColumnNameShouldCreateCorrectQuery()
        {
            _dapperQueryBuilder.Select("User")
                .WithColumn("Name")
                .WithColumn("Birthdate");

            _dapperQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT Name,Birthdate FROM User");
        }

        [Fact]
        public void WhenCallSelectWithDistinctShoulCreateCorrectQuery()
        {
            _dapperQueryBuilder.Select("User")
                .Distinct()
                .WithColumn("Name");

            _dapperQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT DISTINCT Name FROM User");
        }

        [Fact]
        public void WhenCallSelectWithTopShouldCreateCorrectQuery()
        {
            _dapperQueryBuilder.Select("User")
                .Top(10)
                .WithColumn("Name");

            _dapperQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT TOP 10 Name FROM User");
        }

        [Fact]
        public void WhenCallSelectWithPaginationShouldCreateCorrectQuery()
        {
            _dapperQueryBuilder
                .Select("User")
                .WithColumn("Name")
                .GetRowsPaged("Name", 1, 10);

            _dapperQueryBuilder
                .ShowQuery()
                .Should()
                .Be("SELECT Name FROM User ORDER Name OFFSET 1 ROWS FETCH NEXT 10 ROWS ONLY");
        }
    }
}
