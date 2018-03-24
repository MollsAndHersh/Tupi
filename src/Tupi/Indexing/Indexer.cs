﻿using System.IO;
using Tupi.Indexing.Filters;

namespace Tupi.Indexing
{
    public class StringIndexer
    {
        private readonly IAnalyzer _analyzer;

        public StringIndexer(IAnalyzer analyzer = null)
        {
            _analyzer = analyzer ?? DefaultAnalyzer.Instance;
        }

        public InvertedIndex CreateIndex(
            params string[] documents
        )
        {
            var result = new InvertedIndex();

            for (var i = 0; i < documents.Length; i++)
            {
                using (var reader = new StringReader(documents[i]))
                {
                    var tokenSource = new TokenSource(reader);

                    while (tokenSource.Next())
                    {
                        if (_analyzer.Process(tokenSource))
                        {

                            result.Append(
                                new CharArraySegmentKey(tokenSource.Buffer, tokenSource.Size), 
                                i, tokenSource.Position);
                        }
                    }
                }
            }

            return result;
        }
    }
}
