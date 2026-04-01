namespace Dashboard.Application.Slice.ExternalData
{
    public interface IExternalDataService
    {
        Task<string> UpsertDailyTickerFromPolygon(string[] symbols);
    }
}
