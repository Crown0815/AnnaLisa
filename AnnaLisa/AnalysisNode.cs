using System;
using System.Collections.Generic;
using System.Linq;
using AnnaLisa.DataStructures;

namespace AnnaLisa
{
    public class AnalysisNode : IDataSource
    {
        public AnalysisNode() => _data = LazyData();
        
        private Lazy<IAnalysisData> _data;
        public IAnalysisData Data => _data.Value;

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
            _data = LazyData();
        }

        public void Set(IAnalysisOperation operation)
        {
            _operation = operation;
            _data = LazyData();
        }
        
        private Lazy<IAnalysisData> LazyData() => new Lazy<IAnalysisData>(() => Analyzed(SourceData()));
    }
}