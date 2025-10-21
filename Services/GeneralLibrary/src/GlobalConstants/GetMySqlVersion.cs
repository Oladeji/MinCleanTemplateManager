using Microsoft.EntityFrameworkCore;


namespace GlobalConstants
{
    public class GeneralUtils
    {

        public static MySqlServerVersion GetMySqlVersion()
        {
            return new MySqlServerVersion(new Version(9, 0));
        }

    }
}
