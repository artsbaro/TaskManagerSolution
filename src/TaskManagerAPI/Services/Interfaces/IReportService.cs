namespace TaskManagerAPI.Services.Interfaces
{
    public interface IReportService
    {
        Task<Dictionary<string, double>> GetAverageCompletedTasksByUserAsync(string userId);
    }
}
