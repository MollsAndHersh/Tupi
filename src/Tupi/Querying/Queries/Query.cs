﻿using System.Collections.Generic;
using Tupi.Indexing;

namespace Tupi.Querying.Queries
{
    public abstract class Query
    {
        public abstract IEnumerable<int> Execute(InvertedIndex index);

        public static Query Term(string term) =>
            TermQuery.From(term);

        public static Query All(params string[] terms) =>
            AllQuery.From(terms);

        public static Query All(IEnumerable<Query> queries) =>
            new AllQuery(queries);

        public static Query And(Query left, Query right) =>
            new AllQuery(left, right);

        public static Query Any(params string[] terms) =>
            AnyQuery.From(terms);

        public static Query Or(Query left, Query right) =>
            new AnyQuery(left, right);

        public static Query Not(Query innerQuery) => 
            new NotQuery(innerQuery);

        public static Query Distance(string term1, string term2, int maxDistance) =>
            new DistanceQuery(term1, term2, maxDistance);
    }
}
