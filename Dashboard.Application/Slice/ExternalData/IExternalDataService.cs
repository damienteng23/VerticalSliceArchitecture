using Dashboard.Domain.Entities;

namespace Dashboard.Application.Slice.ExternalData
{
    public interface IExternalDataService
    {
        Task UpsertDailyTickerHistoricalDataFromPolygon(string symbol);
    }
}
