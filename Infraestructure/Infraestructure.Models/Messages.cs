namespace Infraestructure
{
    public static class Messages
    {
        public static string CheckStringParams(string[] fieldsName = null)
        => fieldsName?.Length > 0 ? $",{string.Join("|", fieldsName)}" : "";

        public static string Exists(string entityType, string info)
        => $@"EXISTS ""{entityType}|{info}""";
        public static string AlreadyExists(string entityType, string info)
        => $@"ALREADY_EXISTS ""{entityType}|{info}""";

        public static string NotExists(string entityType, string info)
        => $@"NOT_EXISTS ""{entityType}|{info}""";

        public static string Created(string entityType, string info)
        => $@"CREATED ""{entityType}|{info}""";
        public static string CannotBeCreated(string entityType, string info)
        => $@"CANNOT_BE_CREATED ""{entityType}|{info}""";

        public static string NotFound(string entityType, string info)
        => $@"NOT_FOUND ""{entityType}|{info}""";
        public static string InvalidRequest(string entityType, params string[] fieldsName)
        => $@"INVALID_REQUEST ""{entityType}{CheckStringParams(fieldsName)}""";
        public static string InvalidEmail(string[] email) => $@"INVALID_EMAIL ""{string.Join(",",email)}""";
    }
}
