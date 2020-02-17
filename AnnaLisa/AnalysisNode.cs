using System.Collections.Generic;
using System.Linq;
using AnnaLisa.DataStructures;

namespace AnnaLisa
{
    public class AnalysisNode : IDataSource
    {
        public AnalysisNode()
        {
            Data = Analyzed(SourceData());
        }

        public IAnalysisData Data { get; private set; }

        private IAnalysisOperation _operation;

        private IAnalysisData Analyzed(IAnalysisData data)
        {
            return _operation is null ? data : _operation.Perform(data);
        }

        private IAnalysisData SourceData()
        {
            if (!_dataSources.Any()) return AnalysisData.Empty();
            if (_dataSources.Count == 1) return _dataSources.Single().Data;
            return AccumulatedDataFrom(_dataSources);
        }

        private static IAnalysisData AccumulatedDataFrom(IEnumerable<IDataSource> sources) 
            => new MergedAnalysisData(sources.Select(x => x.Data).ToArray());

        private readonly List<IDataSource> _dataSources = new List<IDataSource>();

        public void AddSource(IDataSource dataSource)
        {
            _dataSources.Add(dataSource);
            Data = Analyzed(SourceData());
        }

        public void Set(IAnalysisOperation operation)
        {
            _operation = operation;
            Data = Analyzed(SourceData());
        }
    }
}