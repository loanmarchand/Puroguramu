using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Porugumaru.App.Core;

namespace Porugumaru.App.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<object> _logger;
    private readonly IAssessCode _assessor;

    public static readonly IAssessmentResult NoResult = new NoResult();

    public IAssessmentResult Result = NoResult;
    [BindProperty] public string CodeTextArea { get; set; } = string.Empty;
    
    
    public IndexModel(ILogger<object> logger, IAssessCode assessor)
    {
        _logger = logger;
        _assessor = assessor;
    }
    
    public async Task OnGetAsync()
    {
        _logger.LogInformation("Got Get");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("Got Post with Code equal to ["+CodeTextArea+"]");
        Result = _assessor.Assess(Guid.Empty, CodeTextArea);
        return Page();
    }
}

public class NoResult : IAssessmentResult
{
    public AssessmentStatus Status => AssessmentStatus.None;
    public string Proposal => "";
    public IEnumerable<string> Tests => Array.Empty<string>();
}