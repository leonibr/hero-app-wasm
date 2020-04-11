using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeroApp.Domain
{
    public class ApiResponse
    {
        public ApiResponse()
        {
              
        }
        internal ApiResponse(string message, bool succeeded, IEnumerable<string> errors)
        {
            Message = message;
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public string Message { get; set; }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }


        public static ApiResponse Success(string text = "Success")
        {
            return new ApiResponse(text, true, new string[] { });
        }


        public static ApiResponse Failure(IEnumerable<string> errors)
        {
            return new ApiResponse("Error", false, errors);
        }
        public static ApiResponse Failure(string errorMessage)
        {
            return new ApiResponse("Error", false, new string[] { errorMessage });
        }

        public static ApiResponse Failure(IDictionary<string, string[]> errors, string message = "Failure")
        {
            var sb = new StringBuilder();

            foreach (var item in errors)
            {
                foreach (var valueError in item.Value)
                {
                    sb.AppendLine($"{item.Key}: {valueError}");
                }
            }

            var arrString = sb.Length > 0 ? sb.ToString().Split(Environment.NewLine).Where(s => s.Length > 0).ToArray() : new string[] { };

            return new ApiResponse(message, false, arrString);
        }
    }

    public class ApiResponse<TResult> : ApiResponse where TResult : class
    {
        public ApiResponse() : base() { }
        ApiResponse(TResult result, string message = "Success") : base(message, true, new string[] { })
        {
            Result = result;
        }

        public static ApiResponse<TResult> SuccessFrom(TResult result)
        {
            return new ApiResponse<TResult>(result);
            
        }
        public static ApiResponse<TResult> FailureFrom(string errorMessage)
        {
            return new ApiResponse<TResult>(null, errorMessage);
            
        }

        public static ApiResponse<TResult> FailureFrom(IEnumerable<string> errors)
        {
            return new ApiResponse<TResult>(null, errors.Aggregate((a,p) => $"{a}{Environment.NewLine}{p}"));            
        }


        public TResult Result { get; set; }

    }
}
