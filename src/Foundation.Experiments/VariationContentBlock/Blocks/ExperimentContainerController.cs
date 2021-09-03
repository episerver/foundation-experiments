using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Experiments.ExperimentContentArea.Blocks
{
    public class ExperimentContainerController : BlockController<ExperimentContainer>
    {
        private readonly IExperimentBlockVariationSelector _experimentBlockVariationSelector;

        private readonly ContentAreaRenderer _contentAreaRenderer;

        public ExperimentContainerController(IExperimentBlockVariationSelector experimentBlockVariationSelector, ContentAreaRenderer contentAreaRenderer)
        {
            _experimentBlockVariationSelector = experimentBlockVariationSelector;
            _contentAreaRenderer = contentAreaRenderer;
        }

        public override ActionResult Index(ExperimentContainer currentBlock)
        {
            var item = _experimentBlockVariationSelector.GetVariationContent(Request.RequestContext.HttpContext, currentBlock);

            var clone = currentBlock.CreateWritableClone() as ExperimentContainer;
            clone.DisplayContentArea = item.ContentArea;
            using (var textWriter = new StringWriter())
            {
                var htmlHelper = CreateHtmlHelper(this, textWriter);
                if (item.Variables.Any())
                {
                    ViewData["ExperimentVariables"] = item.Variables;
                }
                // Set them for the helper as well.
                htmlHelper.ViewData["ExperimentVariables"] = item.Variables;

                _contentAreaRenderer.Render(htmlHelper, item.ContentArea);
                var blockHtml = textWriter.ToString();

                return Content(blockHtml);
            }
        }

        private HtmlHelper CreateHtmlHelper(ControllerBase controller, StringWriter textWriter)
        {
            var viewContext = new ViewContext(controller.ControllerContext, new WebFormView(controller.ControllerContext, "tempviewpath"), controller.ViewData, controller.TempData, textWriter);
            return new HtmlHelper(viewContext, new ViewPage());
        }
    }
}