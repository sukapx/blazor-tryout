using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Site.Data.Model
{
  public class WorkitemContext
  {
    private readonly ILogger _logger;
    private List<Workitem> Workitems = new()
    {
      new()
      {
        ID = 1,
        Creation = DateTime.UtcNow,
        LastChange = DateTime.UtcNow,
        Description = "Some description",
        Title = "Some Title"
      },
      new()
      {
        ID = 2,
        Creation = DateTime.UtcNow,
        LastChange = DateTime.UtcNow,
        Description = "Some other description",
        Title = "Some other Title"
      }
    };

    public WorkitemContext(ILogger<WorkitemContext> logger)
    {
      _logger = logger;
    }

    public async Task<List<Workitem>> GetWorkitems()
    {
      return await Task.Run(async () =>
      {
        await Task.Delay(1000);
        _logger.LogInformation($"Returning all {Workitems.Count} Workitems");
        return Workitems.ToList();
      });
    }

    public async Task<bool> InsertWorkitem(Workitem workitem)
    {
      _logger.LogInformation($"Called InsertWorkitem #{workitem.ID} \"{workitem.Title}\"");
      var existing = Workitems.FindAll(x => (x.ID == workitem.ID));
      if (existing.Count > 0){
        _logger.LogInformation($"Removing for update #{existing[0].ID} \"{existing[0].Title}\"");
        Workitems.Remove(existing[0]);
      }

      return await Task<bool>.Run(() =>
      {
        Workitems.Add(workitem);
        _logger.LogInformation($"Workitems count {Workitems.Count}");
        return true;
      });
    }
  }
}