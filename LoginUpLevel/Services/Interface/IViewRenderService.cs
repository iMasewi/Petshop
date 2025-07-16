using Microsoft.AspNetCore.Mvc;

namespace LoginUpLevel.Services.Interface
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model, ControllerContext actionContext);
    }
}
