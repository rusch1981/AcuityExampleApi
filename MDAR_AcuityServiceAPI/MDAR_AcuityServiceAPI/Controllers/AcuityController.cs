using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using MDAR_AcuityServiceAPI.Classes;
using MDAR_AcuityServiceAPI.Models;

namespace MDAR_AcuityServiceAPI.Controllers
{
    public class AcuityController : ApiController
    {
        private readonly HookManager _responseManager = new HookManager();
        private readonly Logging _serviceHistory = new Logging();

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form)
        {
            try
            {
                var notification = new Notification(form);

                _responseManager.Setlogger(_serviceHistory);
                _responseManager.ValidateRequest(HttpContext.Current, notification.GetString());

                if (!_responseManager.IsvalidRequest)
                {
                    return Request.CreateResponse(_responseManager.StatusCode, _responseManager.Message);
                }

                _responseManager.ProcessTasks(notification);


                _responseManager.Terminate();

                var message = Request.CreateResponse(_responseManager.StatusCode, _responseManager.Message);
                message.Headers.Location = new Uri(Request.RequestUri.ToString());

                _serviceHistory.LogLog(_responseManager.Message);

                return message;

            }
            catch (Exception e)
            {
                try
                {
                    _serviceHistory.LogFatalError(e);
                }
                catch (Exception ex)
                {
                    _serviceHistory.LogMessage(Environment.NewLine
                                               + "**********************************************************************************"
                                               + Environment.NewLine
                                               + "Intervnal Server Error- Email Failure");
                    _serviceHistory.LogMessage(ex.ToString());
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Service Error.");
            }
        }
    }
}