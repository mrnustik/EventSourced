using DotVVM.Framework.Binding;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace EventSourced.Diagnostics.Web.Controls
{
    public class CopyJsonButton : HtmlGenericControl
    {
        public static readonly DotvvmProperty JsonProperty = DotvvmProperty.Register<string, CopyJsonButton>(s => s.Json);
        
        public string? Json
        {
            get => GetValue<string>(JsonProperty);
            set => SetValue(JsonProperty, value);
        }

        public CopyJsonButton() : base("button")
        {
        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            base.AddAttributesToRender(writer, context);
            writer.AddKnockoutDataBind(nameof(CopyJsonButton), CreateBindingGroup());
        }

        protected override void OnInit(IDotvvmRequestContext context)
        {
            base.OnInit(context);
            context.ResourceManager.AddRequiredResource("copy-json-button-js");
        }

        private KnockoutBindingGroup CreateBindingGroup()
        {
            var bindingGroup = new KnockoutBindingGroup();
            bindingGroup.Add(nameof(Json), this, JsonProperty);
            return bindingGroup;
        }
    }
}