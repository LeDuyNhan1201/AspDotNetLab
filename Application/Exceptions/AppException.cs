using System;

namespace Application.Exceptions
{
    public class AppException(AppError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public AppError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
