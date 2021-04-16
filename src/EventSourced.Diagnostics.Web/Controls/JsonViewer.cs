using DotVVM.Framework.Binding;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace EventSourced.Diagnostics.Web.Controls
{
    public class JsonViewer : HtmlGenericControl
    {
        public static readonly DotvvmProperty JsonProperty = DotvvmProperty.Register<string, JsonViewer>(s => s.Json);
        public string? Json
        {
            get => GetValue<string>(JsonProperty);
            set => SetValue(JsonProperty, value);
        }

        public JsonViewer()
            : base("div")
        {
        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddKnockoutDataBind(nameof(JsonViewer), CreateKnockoutBindingGroup());
            base.AddAttributesToRender(writer, context);
        }

        protected override void OnInit(IDotvvmRequestContext context)
        {
            context.ResourceManager.AddRequiredResource("json-viewer-control-js");
            context.ResourceManager.AddRequiredResource("json-viewer-css");
            base.OnInit(context);
        }

        private KnockoutBindingGroup CreateKnockoutBindingGroup()
        {
            var bindingGroup = new KnockoutBindingGroup();
            bindingGroup.Add(nameof(Json), this, JsonProperty, () => { });
            return bindingGroup;
        }
    }
}