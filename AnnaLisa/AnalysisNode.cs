using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnnaLisa
{
    public class AnalysisNode
    {
        public object? Data{ get; private set; }
        public Task Calculation { get; } = Task.CompletedTask;
        public bool HasData => Data is {};

        private List<IDataSource> _dataSources = new List<IDataSource>();
        public IReadOnlyCollection<IDataSource> DataSources => _dataSources;

        public void AddSource(IDataSource dataSource)
        {
            _dataSources.Add(dataSource);
            Data = dataSource.Data;
        }
    }
}