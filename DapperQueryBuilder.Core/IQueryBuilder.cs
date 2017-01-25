using System.Collections.Generic;

namespace DapperQueryBuilder.Core
{
    public interface IQueryBuilder
    {
        DapperQueryBuilder Select(string tableName);
        DapperQueryBuilder Top(int top);
        DapperQueryBuilder Distinct();
        DapperQueryBuilder WithColumn(string columnName);
        DapperQueryBuilder Where(string condition);
        DapperQueryBuilder And(string condition);
        DapperQueryBuilder Or(string condition);
        string ShowQuery();
        IEnumerable<T> GoQuery<T>() where T : class;
    }
}
