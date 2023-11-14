namespace Tanji.Core.Habbo.Canvas;

[Flags]
public enum HPatches
{
    None = 0,

    FlashDefaults = DisableHostChecks | DisableEncryption | InjectKeyShouter | InjectAddressShouter | InjectAddress,

    InjectAddress = 1,
    InjectRSAKeys = 2,

    InjectKeyShouter = 4,
    InjectAddressShouter = 8,

    DisableHostChecks = 16,
    DisableEncryption = 32,
}