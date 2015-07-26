using System;
using System.Collections.Generic;

namespace Linqify
{
    /// <summary>
    ///     Provides an structure to hold the query parameters
    /// </summary>
    public class QueryParameter : IComparable<QueryParameter>, IComparable
    {
        public static IComparer<QueryParameter> DefaultComparer = new QueryParameterComparer();
        private readonly string _name;

        public QueryParameter(string name, string value)
        {
            this._name = name;
            this.Value = value;
        }

        public string Name
        {
            get { return this._name; }
        }

        public string Value { get; internal set; }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return ReferenceEquals(this, null) ? 0 : 1;
            }

            var other = obj as QueryParameter;
            return CompareTo(other);
        }

        public int CompareTo(QueryParameter other)
        {
            return DefaultComparer.Compare(this, other);
        }
    }

    /// <summary>
    ///     Comparer class used to perform the sorting of the query parameters
    /// </summary>
    public class QueryParameterComparer : IComparer<QueryParameter>
    {
        public int Compare(QueryParameter x, QueryParameter y)
        {
            if (x.Name.Equals(y.Name))
            {
                return string.Compare(x.Value, y.Value);
            }
            return string.Compare(x.Name, y.Name);
        }
    }
}