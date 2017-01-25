using System.Collections.Generic;
using System.Text;

namespace DapperQueryBuilder.Core
{
    public abstract class BaseQueryBuilder
    {
        protected StringBuilder Query;
        protected List<string> Columns;

        protected BaseQueryBuilder()
        {
            Query = new StringBuilder();
            Columns = new List<string>();
        }

        protected virtual string BuildQuery()
        {
            return Query.ToString();
        }
    }
}
