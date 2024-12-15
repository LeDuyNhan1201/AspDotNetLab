namespace Application.Exceptions;

public record AppError
{

    public ErrorType Type { get; protected set; }

    public string Code { get; protected set; }

    protected AppError(ErrorType type, string code)
    {
        Type = type;
        Code = code;
    }

    // Validation Errors
    public static readonly AppError VALIDATION_ERROR = new(ErrorType.ValidationError, ErrorType.ValidationError.ToString());
    public static readonly AppError CAN_NOT_PARSE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.CanNotParse");

    // Not found Errors
    public static readonly AppError CART_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Cart");
    public static readonly AppError CART_ITEM_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.CartItem");
    public static readonly AppError BOOK_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Book");
    public static readonly AppError POST_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Post");
    public static readonly AppError COMMENT_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Comment");
    public static readonly AppError REACTION_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Reaction");
    public static readonly AppError FILE_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.File");
    public static readonly AppError NOTIFICATION_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Notification");


}
