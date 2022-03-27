namespace SMS.NET.Models.Enums
{
    public enum ActivationStatus
    {
        STATUS_WAIT_CODE = 1,
        STATUS_WAIT_RETRY = -1,
        STATUS_WAIT_RESEND = 3,
        STATUS_CANCEL = 8,
        STATUS_OK = 6
    }
}
