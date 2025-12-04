using PaypalServerSdk.Standard;

namespace Models.Interface;

public interface IPayPalHelper
{
    PaypalServerSdkClient Client { get; }
}
