namespace INNO.Domain.Utils
{
    public static class StringUtils
    {
        public static bool ValidateZipCode(string? zipcode)
        {
            if(string.IsNullOrWhiteSpace(zipcode))
            {
                return false;
            }

            return true;
        }

        public static bool ValidateDocument(string? document)
        {
            if (string.IsNullOrWhiteSpace(document))
            {
                return false;
            }

            return true;
        }
    }
}
