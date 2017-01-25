using System;
using System.Collections.Generic;

namespace DapperQueryBuilder.Core
{
    public class DapperQueryBuilder : IQueryBuilder
    {
        public readonly string ConnectionString;

        private SelectQueryBuilder _selectQueryBuilder;

        public DapperQueryBuilder And(string condition)
        {
            _selectQueryBuilder.And(condition);
            return this;
        }

        public DapperQueryBuilder Distinct()
        {
            _selectQueryBuilder.Distinct();
            return this;
        }

        public IEnumerable<T> GoQuery<T>() where T : class
        {
            return _selectQueryBuilder.GoQuery<T>(ConnectionString);
        }

        public DapperQueryBuilder Or(string condition)
        {
            _selectQueryBuilder.Or(condition);
            return this;
        }

        public DapperQueryBuilder Select(string tableName)
        {
            if (_selectQueryBuilder == null) _selectQueryBuilder = new SelectQueryBuilder();

            _selectQueryBuilder.Select(tableName);
            return this;
        }

        public string ShowQuery()
        {
            return _selectQueryBuilder.ShowQuery();
        }

        public DapperQueryBuilder Top(int top)
        {
            _selectQueryBuilder.Top(top);
            return this;
        }

        public DapperQueryBuilder Where(string condition)
        {
            _selectQueryBuilder.Where(condition);
            return this;
        }

        public DapperQueryBuilder WithColumn(string columnName)
        {
            _selectQueryBuilder.WithColumn(columnName);
            return this;
        }
    }
}
