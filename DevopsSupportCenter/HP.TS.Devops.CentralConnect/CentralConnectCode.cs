using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HP.TS.Devops.CentralConnect
{
    public enum CentralConnectCode
    {
        Success = 0,
        ClientNotFound = 1,
        PrimaryContactNotFound = 2,
        SecondaryContactNotFound = 3,
        CompanyNotFound = 4,
        ClientRegistrationFail = 5,
        RegisterClientMessageExist = 6,
        AddRegisterClientMessageFail = 7,
        RegisterClientMessageNotExist = 8,
        UpdateHPPIDAndPasswordFail = 9,
        VisibleClientFail = 10,
        InvisibleClientFail = 11,
        CreateDeviceFail = 12,
        CreateClientFail = 13,
        RegisterDeviceMessageExist = 14,
        DeviceNotFound = 15,
        ClientNotRegister = 16,
        RegisterDeviceClientError = 17,
        RegisterDeviceReRegisterError = 18,
        RegisterDeviceTransientError = 19,
        AddRegisterDeviceMessageFail = 20,
        ClientNotFoundAssociatedDevice = 21,
        RegisterDeviceMessageNotExist = 22,
        CreateMetricsFail = 23,
        SendMetricsClientError = 24,
        SendMetricsReRegisterError = 25,
        SendMetricsTransientError = 26,
        PluginMapNotExist = 27,
        ResourceFileNotExist = 28,
        LoadResourceFileFail = 29,
        CreateMetricsInstanceFail = 30,
        EnhancementArgumentsNotFound = 31,
        AddMetricsMessageFail = 32
    }
}
