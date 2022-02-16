using System.ComponentModel.DataAnnotations;
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
        return await _workitemContext.GetWorkitems();
    }

    public async Task<bool> InsertWorkitem(Workitem workitem){
        _logger.LogInformation($"Called InsertWorkitem #{workitem.ID} \"{workitem.Title}\"");
        return await _workitemContext.InsertWorkitem(workitem);
    }
}
