using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using LoginUpLevel.Services.Interface;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace LoginUpLevel.Services
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderToStringAsync(string viewPath,
            object model, ControllerContext actionContext)
        {
            var viewEngineResult = _razorViewEngine.GetView(null, "~/Views/Email/EmailBody.cshtml", false);

            if (viewEngineResult.View == null || (!viewEngineResult.Success))
            {
                throw new ArgumentNullException($"Unable to find view '{viewPath}'");
            }

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), actionContext.ModelState);
            viewDictionary.Model = model;

            var view = viewEngineResult.View;
            var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

            using var sw = new StringWriter();
            var viewContext =
                new ViewContext(actionContext, view, viewDictionary, tempData, sw, new HtmlHelperOptions());
            await view.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
