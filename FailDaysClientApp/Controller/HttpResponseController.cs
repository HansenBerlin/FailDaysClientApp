using System.Net;

namespace FailDaysClientApp.Controller
{
    public class HttpResponseController
    {
        public string ConvertResponseCodeToMessage(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.OK => "DATENBANK AKTUALISIERT",
                HttpStatusCode.Unauthorized => "NICHT AUTORISIERT (401)",
                HttpStatusCode.BadRequest => "BAD REQUEST (400)",
                HttpStatusCode.GatewayTimeout => "TIMEOUT (504)",
                HttpStatusCode.InternalServerError => "INTERNER SERVERFEHLER (500)",
                HttpStatusCode.MethodNotAllowed => "UNERLAUBTE ANFRAGE (405)",
                HttpStatusCode.TooManyRequests => "ZU VIELE ANFRAGEN (429)",
                HttpStatusCode.UnsupportedMediaType => "FALSCHER MEDIATYPE (415)",
                HttpStatusCode.RequestUriTooLong => "ANFRAGE URI ZU LANG (414)",
                _ => "UNBEKANNTER FEHLER"
            };
        }
    }
}