using System.Collections.Generic;

namespace GameCreator.Editor.Search
{
    internal class Indexer
    {
        public Dictionary<string, List<int>> TermFieldIndex { get; }
        
        public Indexer(Domain domain)
        {
            this.TermFieldIndex = new Dictionary<string, List<int>>();

            foreach (KeyValuePair<int, Field> fieldEntry in domain.Fields)
            {
                foreach (string uniqueTerm in fieldEntry.Value.TermsPositions.Keys)
                {
                    this.TermFieldIndex.TryAdd(uniqueTerm, new List<int>());
                    this.TermFieldIndex[uniqueTerm].Add(fieldEntry.Key);
                }
            }
        }
    }
}