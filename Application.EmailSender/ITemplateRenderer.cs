using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailSender
{
    public interface ITemplateRenderer
    {
        string Parse<T>(string template, T model, bool isHtml = true);
    }
}
