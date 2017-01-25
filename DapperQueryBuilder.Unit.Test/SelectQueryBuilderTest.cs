using DapperQueryBuilder.Core;
using FluentAssertions;
using Xunit;

namespace DapperQueryBuilder.Unit.Test
{
    public class SelectQueryBuilderTest
    {
        private readonly SelectQueryBuilder _selectQueryBuilder;

        public SelectQueryBuilderTest()
        {
            _selectQueryBuilder = new SelectQueryBuilder();
        }

        [Fact]
        public void WhenCreateQueryBuilderQueryShoulNotBeEmpty()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.ShowQuery().Should().NotBeEmpty();
        }

        [Fact]
        public void WhenCallSelectMethodShouldCreateSelectQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.ShowQuery().Should().Be("SELECT * FROM User");
        }

        [Fact]
        public void WhenCallWithColumnMethodShouldAddOneColumnNameToSelectQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.WithColumn("Name");
            _selectQueryBuilder.ShowQuery().Should().Be("SELECT Name FROM User");
        }

        [Fact]
        public void WhenCallWithColumnMethodShouldAddTwoColumnNamesToSelectQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.WithColumn("Name");
            _selectQueryBuilder.WithColumn("BirthDate");
            _selectQueryBuilder.ShowQuery().Should().Be("SELECT Name,BirthDate FROM User");
        }

        [Fact]
        public void WhenCalledWhereMethodShouldAddWhereCriteriaToQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.Where("Id = 1");
            _selectQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT * FROM User WHERE Id = 1");
        }

        [Fact]
        public void WhenCallAndMethodShouldAddAndCriteriaToQuery()
        {
            const string name = "Nicolas";
            const int active = 1;

            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.Where($"Name = {name}");
            _selectQueryBuilder.And($"Active = {active}");
            _selectQueryBuilder.ShowQuery()
                .Should()
                .Be($"SELECT * FROM User WHERE Name = {name} AND Active = {active}");
        }

        [Fact]
        public void WhenCallOrMethodShouldAddOrCriteriaToQuery()
        {
            const string name = "Nicolas";

            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.Where($"Name = {name}");
            _selectQueryBuilder.Or($"Name = {name}");
            _selectQueryBuilder.ShowQuery()
                .Should()
                .Be($"SELECT * FROM User WHERE Name = {name} OR Name = {name}");
        }

        [Fact]
        public void WhenCallTopMethodShoulAddTopCriteriaToQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.Top(10);
            _selectQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT TOP 10 * FROM User");
        }

        [Fact]
        public void WhenCallDistinctMethodShouldAddDistinctCriteriaToQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.Distinct();
            _selectQueryBuilder.WithColumn("Name");
            _selectQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT DISTINCT Name FROM User");


        }

        [Fact]
        public void WhenCallDistinctAndTopMethodShoulAddDistinctAndTopToQuery()
        {
            _selectQueryBuilder.Select("User");
            _selectQueryBuilder.Distinct();
            _selectQueryBuilder.Top(10);
            _selectQueryBuilder.WithColumn("Name");
            _selectQueryBuilder.ShowQuery()
                .Should()
                .Be("SELECT DISTINCT TOP 10 Name FROM User");

        }

    }
}
