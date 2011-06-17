namespace MI.Services
{
    public class ServiceConfiguration
    {
#if DEBUG
        public const bool Secure = false;
#else
        public const bool Secure = true;
#endif
    }
}