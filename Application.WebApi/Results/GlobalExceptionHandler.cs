using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Application.WebAPI.Results
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public virtual Task HandleAsync(ExceptionHandlerContext context,
                                    CancellationToken cancellationToken)
        {
            //if (!ShouldHandle(context))
            //{
            //    return Task.FromResult(0);
            //}

            return HandleAsyncCore(context, cancellationToken);
        }

        public virtual Task HandleAsyncCore(ExceptionHandlerContext context,
                                           CancellationToken cancellationToken)
        {
            HandleCore(context);
            return Task.FromResult(0);
        }

        public virtual void HandleCore(ExceptionHandlerContext context)
        {
            string content = string.Empty;
            if (context.Exception.GetType().Name == "CustomException")
            {
                CustomException ex = (CustomException)context.Exception;
                if (ex.ReferenceKey != null)
                    content = string.Concat(ex.ErrorMessage, "\r\n Please use this reference key ", ex.ReferenceKey , " to contact system administrator.");
                else
                    content = string.Concat(ex.ErrorMessage);
            }
            else
            {
                content = "Oops! Sorry! Something went wrong. Please contact system administrator. so we can try to fix it.";
            }
            context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = content
            };
        }

        public virtual bool ShouldHandle(ExceptionHandlerContext context)
        {
            return context.CatchBlock.IsTopLevel;
        }





        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }

      

    }


}
