namespace Infraestructure
{
    public static class Messages
    {
        public static string CheckStringParams((string, string)[] fieldsName = null)
        {
            var result = string.Empty;
            if (fieldsName?.Length > 0)
                foreach (var (f, v) in fieldsName)
                    if (result.Length == 0)
                        result += $@"{{""{f}"":""{v}""}}";
                    else
                        result += $@",{{""{f}"":""{v}""}}";
            return $"{result}";
        }

        public static string Exists(string entityType, string info)
        => $@"EXISTS |{{""{entityType}"":""{info}""}}";
        public static string Updated(string entityType, string id)
        => $@"UPDATED |{{""{entityType}"":""{id}""}}";

        public static string NotUpdated(string entityType, string id)
        => $@"NOT_UPDATED |{{""{entityType}"":""{id}""}}";
        public static string AlreadyExists(string entityType, string info)
        => $@"ALREADY_EXISTS |{{""{entityType}"":""{info}""}}";

        public static string NotExists(string entityType, string info)
        => $@"NOT_EXISTS |{{""{entityType}"":""{info}""}}";

        public static string NotAssociated(string entityType, string info)
        => $@"NOT_ASSOCIATED |{{""{entityType}"":""{info}""}}";

        public static string Created(string entityType, string info)
        => $@"CREATED |{{""{entityType}"":""{info}""}}";
        public static string CannotBeCreated(string entityType, string info)
        => $@"CANNOT_BE_CREATED |{{""{entityType}"":""{info}""}}";

        public static string NotFound(string entityType, string info)
        => $@"NOT_FOUND |{{""{entityType}"":""{info}""}}";
        public static string InvalidRequest(string entityType, params (string,string)[] fieldsName)
        => $@"INVALID_REQUEST ""{entityType}""|{CheckStringParams(fieldsName)}";
        public static string InvalidEmail(string[] email) => $@"INVALID_EMAIL |{{emails:""{string.Join(",",email)}""}}";

        public static string CheckOperation(string opName, string result = null)
        => @$"CHECK_OPERATION |{{{opName}:{(result != null ? $@"""{result}""" : null)}}}";
    }
}
