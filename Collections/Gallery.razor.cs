using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WizWebComponents.Collections
{
  partial class Gallery
  {
    [Parameter] public string[]? Uris { get; set; }
    [Parameter] public string Size { get; set; } = "15rem";
    [Parameter] public bool FlowHorizontal { get; set; } = true;
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? SplatStuff { get; set; }

    string itemStyle { get
      {
        string style = "";
        if (FlowHorizontal)
        {
          style += "height: 100%; margin-right: 8px;";
        }
        else
        {
          style += "width: 100%; margin-bottom: 8px;";
        }
        return style;
      } }
    string galleryStyle { get
      {
        string style = "";

        if (FlowHorizontal)
        {
          style += $"flex-direction: row; overflow-x: auto; overflow-y: hidden; height: {Size}; white-space: nowrap; max-width: 100%; scrollbar-width: thin;";
        }
        else
        {
          style += $"flex-direction: column; overflow-x: hidden; overflow-y: auto; width: {Size}";
        }
        return style;
      } }
  }
}
