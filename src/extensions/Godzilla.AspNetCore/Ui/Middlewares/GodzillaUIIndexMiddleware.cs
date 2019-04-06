using Godzilla.AspNetCore.Ui.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Middlewares
{
    public class GodzillaUIIndexMiddleware : AngularUIPluginMiddleware
    {
        public GodzillaUIIndexMiddleware(RequestDelegate next, GodzillaUIOptions options)
            : base(next, options)
        {
        }
    }
}
