/*
  Hi!
  I made this component to make it easier to have consistent and quick styling across the web app,
  but honestly it might just be better to not use these, and opt for typical html while using set
  style classes. But hey, maybe this is more convenient; I don't know.
*/

using Microsoft.AspNetCore.Components;

namespace WizWebComponents.Data
{
  partial class Text
  {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Size { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? SplatValues { get; set; }
    string TextSizeClass = "wiz-font-body";

    protected override Task OnParametersSetAsync()
    {
      switch (Size)
      {
        case "Body":
          TextSizeClass = "wiz-font-body";
          break;
        case "Subtitle":
          TextSizeClass = "wiz-font-subtitle";
          break;
        case "Title":
          TextSizeClass = "wiz-font-title";
          break;
        case "Title-L":
          TextSizeClass = "wiz-font-title-l";
          break;
        case "Display":
          TextSizeClass = "wiz-font-display";
          break;
        default:
          TextSizeClass = "wiz-font-body";
          break;
      }

      return base.OnParametersSetAsync();
    }
  }
}
