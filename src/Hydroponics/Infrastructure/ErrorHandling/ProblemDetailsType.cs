namespace Hydroponics.Api.Infrastructure.ErrorHandling
{
    /// <summary>
    /// A URI reference [RFC3986] that identifies the
    /// problem type.This specification encourages that, when
    /// dereferenced, it provide human-readable documentation for the
    /// problem type (e.g., using HTML [W3C.REC-html5-20141028]).  When
    /// this member is not present, its value is assumed to be "about:blank".
    /// </summary>
    internal static class ProblemDetailsType
    {
        public const string BadRequest = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
        public const string InternalServerError = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1";
        public const string ServiceUnavailable = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.4";
        public const string NotFound = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4";
        public const string Unauthorized = "https://www.rfc-editor.org/rfc/rfc7235#section-3.1";
    }
}

