using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WizWebComponents.Input
{
  partial class TagsInput : InputBase<List<string>>
  {
    [Inject] IJSRuntime? JS { get; set; }
    private ElementReference InputArea { get; set; }
    private ElementReference NewTagInput { get; set; }
    private string InputId = Guid.NewGuid().ToString();

    public async Task<bool> Input_OnKeyDown(KeyboardEventArgs e)
    {
      if (e.Key == "Enter" || e.Key == " ")
      {
        if (JS is null) return false;

        var tag = await JS.InvokeAsync<string>("window.TagsInput.makeNewTag", InputId);
        tag = tag.Replace("<br>", "").Replace("&nbsp;", "").Trim().ToLower();
        if (tag.Length > 0 && Regex.IsMatch(tag, @"^[a-zA-Z0-9]*$"))
        {
          if (Value!.Any(x => x == tag)) return false;

          Value!.Add(tag);
          await OnChanged();

          return false;
        }
      }
      return true;
    }

    async Task OnChanged()
    {
      //Value = Tags.ToArray();
      await ValueChanged.InvokeAsync(Value!);
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out List<string> result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
      Console.WriteLine("Booty");

      if (value is null)
      {
        validationErrorMessage = null;
        result = [];
        return true;
      }

      validationErrorMessage = null;
      result = JsonSerializer.Deserialize<List<string>>(value) ?? [];
      return true;
    }
  }
}
