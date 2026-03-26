namespace Dashboard.Application.Slice.ExternalData
{
    public interface IExternalDataService
    {
        Task<string> GetDataFromYFinance(string[] symbols);
    }
}
