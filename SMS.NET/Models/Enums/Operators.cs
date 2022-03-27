namespace SMS.NET.Models.Enums
{
    /// <summary>
    /// Only use <see cref="any"/> unless your country is set to <see cref="Countries.Russia"/> (0), <see cref="Countries.Ukraine"/> (1), or <see cref="Countries.Kazakhstan"/> (2)
    /// </summary>
    public enum Operators
    {
        megafon,
        mts,
        beeline,
        tele2,
        rostelecom,
        any,
        kyivstar,
        life,
        utel,
        vodafone,
        aktiv,
        altel
    }
}
