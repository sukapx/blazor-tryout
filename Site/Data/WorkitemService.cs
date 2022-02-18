using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Site.Data.Model;

namespace Site.Data;

public class WorkitemService
{

    private readonly ILogger _logger;
    private readonly WorkitemContext _workitemContext;

    public WorkitemService(ILogger<WorkitemService> logger, WorkitemContext workitemContext){
        _logger = logger;
        _workitemContext = workitemContext;
    }

    public async Task<List<Workitem>> GetAllWorkitems(){
        _logger.LogInformation($"Called GetAllWorkitems");
        return await _workitemContext.Workitem.ToListAsync();
    }

    public async Task<List<Workitem>> GetWorkitems(int Category){
        _logger.LogInformation($"Called GetAllWorkitems");
        return await _workitemContext.Workitem.Where(e => e.Category == Category).ToListAsync();
    }

    public async Task<bool> InsertWorkitem(Workitem workitem){
        _logger.LogInformation($"Called InsertWorkitem #{workitem.ID} \"{workitem.Title}\"");
        await _workitemContext.Workitem.AddAsync(workitem);
        await _workitemContext.SaveChangesAsync();
        return true;
    }
}
