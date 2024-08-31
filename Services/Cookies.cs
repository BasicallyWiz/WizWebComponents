using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WizWebComponents.Services
{
  public class CookieList : List<Cookie>
  {
    public IJSRuntime JS { get; set; }
    public string GetCookieMethod { get; set; }
    public string SetCookieMethod { get; set; }

    public CookieList(IJSRuntime JS, string? GetCookiemethod, string? SetCookieMethod) : base()
    {
      this.JS = JS;
      this.GetCookieMethod = GetCookiemethod ?? "window.CookieAccessor.Get";
      this.SetCookieMethod = SetCookieMethod ?? "window.CookieAccessor.Set";
    }

    public async Task PopulateFromCookies(IJSRuntime JS)
    {
      var cookies = await JS.InvokeAsync<string>(GetCookieMethod ?? "window.CookieAccessor.get");

      if (cookies.Contains(';'))
      {
        foreach (Cookie cookie in cookies.Split(';').Select(x => new Cookie(x.Split('=')[0], x.Split('=')[1])))
        {
          base.Add(cookie);
        }
      }
      else if (cookies.Contains('='))
      {
        Cookie cookie = new Cookie(cookies.Split('=')[0], cookies.Split('=')[1]);
        base.Add(cookie);
      }
    }
    public Cookie? this[Cookie cookie]
    {
      get
      {
        return this.FirstOrDefault(x => x?.Name == cookie.Name, null);
      }
      set
      {
        if (value is null) return;
        if (this.Contains(cookie)) this[this.IndexOf(cookie)] = value;
        else this.Add(value);
      }
    }
    public Cookie? this[string cookieName]
    {
      get
      {
        return this.FirstOrDefault(x => x?.Name == cookieName, null);
      }
      set
      {
        if (value is null) return;
        if (this.Any(x => x.Name == value.Name)) { Remove(this.First(x => x.Name == value.Name)); Add(value); }
        else this.Add(value);
      }
    }

    public new void Add(Cookie cookie)
    {
      _ = JS.InvokeVoidAsync(SetCookieMethod, cookie.ToCookieString());
      base.Add(cookie);
    }
  }
  public class Cookie
  {
    public string Name { get; set; } = "Name";
    public string Value { get; set; } = "Value";
    public string? Domain { get; set; }
    public string? Path { get; set; }
    public DateTime? Expires { get; set; }
    //bool Secure { get; set; } = true;
    //bool HttpOnly { get; set; } = true;
    public string? SameSite { get; set; }

    public Cookie() { }
    public Cookie(string Name, string Value)
    {
      this.Name = Name;
      this.Value = Value;
    }
    public Cookie(string Name, string Value, string Domain, DateTime Expires/*, bool Secure = true, bool HttpOnly = true*/, string SameSite = "Strict", string Path = "/")
    {
      this.Name = Name;
      this.Value = Value;
      this.Domain = Domain;
      this.Expires = Expires;
      //this.Secure = Secure;
      //this.HttpOnly = HttpOnly;
      this.SameSite = SameSite;
      this.Path = Path;
    }

    public override string ToString()
    {
      return ToCookieString();
    }
    public string ToCookieString()
    {
      string cookie = $"{Name}={Value}; Secure;";
      if (Domain is not null) cookie += $"Domain={Domain};";
      if (Expires is not null) cookie += $"Expires={Expires};";
      if (Path is not null) cookie += $"Path={Path};";
      if (SameSite is not null) cookie += $"SameSite={SameSite ?? "Strict"}";

      return cookie;
    }
    /// <summary>
    /// Returns a new Cookie object from a string representing a single cookie.
    /// </summary>
    /// <param name="cookieString"></param>
    /// <returns>A Cookie.</returns>
    public static Cookie FromCookieString(string cookieString)
    {
      return new Cookie() { Name = cookieString.Split('=')[0], Value = cookieString.Split('=')[1] };
    }
  }
}