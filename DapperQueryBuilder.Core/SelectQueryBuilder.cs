using System;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DapperQueryBuilder.Core
{
    public class SelectQueryBuilder : BaseQueryBuilder
    {
        private int _top;
        private bool _isDistinct;

        public void Select(string tableName)
        {
            Query.Append($"SELECT * FROM {tableName}");
        }

        public void Join(string tableName)
        {
            joinTable = tableName;
            Query.Append($" INNER JOIN {joinTable} ON");
        }

        public void LeftKey(string[] keys)
        {
            keys.ToList().ForEach(k => LeftKeys.Add(k));

        }

        public void RightKey(string[] keys)
        {
            keys.ToList().ForEach(k => RightKeys.Add(k));
        }

        public void Top(int top)
        {
            _top = top;
        }

        public void Distinct()
        {
            _isDistinct = true;
        }

        public void WithColumn(string columnName)
        {
            Columns.Add(columnName);
        }

        public void Where(string condition)
        {
            Query.Append($" WHERE {condition}");
        }

        public void And(string condition)
        {
            Query.Append($" AND {condition}");
        }

        public void Or(string condition)
        {
            Query.Append($" OR {condition}");
        }

        public string ShowQuery()
        {
            return BuildQuery();
        }

        public void GetRowsPaged(string fieldToOrderBy, int page,
            int resultSetCount)
        {
            if (string.IsNullOrWhiteSpace(fieldToOrderBy))
                throw new ArgumentNullException("The field to ordering is required");

            Query.Append($" ORDER {fieldToOrderBy}");
            Query.Append($" OFFSET {page} ROWS");
            Query.Append($" FETCH NEXT {resultSetCount} ROWS ONLY");
        }

        public IEnumerable<T> GoQuery<T>(string connectionString) where T : class
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = BuildQuery();
                connection.Open();
                return connection.Query<T>(query);
            }
        }

        protected override string BuildQuery()
        {
            var query = Query.ToString();
            query = BuildQueryWithDistinct(query);
            query = BuildQueryWithTop(query);
            query = BuildQueryWithColumns(query);
            return query;
        }

        public void BuildJoin()
        {
            if (!LeftKeys.Any() || !RightKeys.Any()) return;

            var unionKeys = LeftKeys.Zip(RightKeys, (l, r) => new { Left = l, Right = r }).ToList();

            var join = new StringBuilder();
            unionKeys.ForEach(item =>
            {
                if (join.Length > 0)
                    join.Append($" AND {item.Left} = {item.Right}");
                else
                    join.Append($"{item.Left} = {item.Right}");

            });
           
            Query = Query.Replace($"{joinTable} ON", $"{joinTable} ON {join.ToString()}");
            ClearJoiVariable();
          
        }

        private void ClearJoiVariable()
        {
            LeftKeys.Clear();
            RightKeys.Clear();
            joinTable = string.Empty;
        }

        private string BuildQueryWithColumns(string query)
        {
            if (Columns.Any())
            {
                var columns = Columns.Aggregate(new StringBuilder(), (currentColumn, nextColumn) =>
                {
                    if (currentColumn.Length > 0) currentColumn.Append(",");
                    currentColumn.Append(nextColumn);
                    return currentColumn;
                }).ToString();

                query = query.Replace("*", columns);
            }
            return query;
        }

        private string BuildQueryWithTop(string query)
        {
            if (_top <= 0) return query;

            if (_isDistinct) return query.Replace("SELECT DISTINCT", $"SELECT DISTINCT TOP {_top}");

            return query.Replace("SELECT", $"SELECT TOP {_top}");
        }

        private string BuildQueryWithDistinct(string query)
        {
            if (_isDistinct)
            {
                query = query.Replace("SELECT", "SELECT DISTINCT");
            }
            return query;
        }
    }
}
