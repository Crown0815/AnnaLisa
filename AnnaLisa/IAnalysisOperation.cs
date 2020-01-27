namespace AnnaLisa
{
    public interface IAnalysisOperation
    {
        IAnalysisData Perform(IAnalysisData data);
    }
}