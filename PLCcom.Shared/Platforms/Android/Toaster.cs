using Android.Widget;
using PLCcom.Helpers;

namespace PLCcom.PlatformImplementations;

public class Toaster : IToast
{
    public void MakeToast(string message)
    {
        Toast.MakeText(Platform.CurrentActivity, "Internet I have!", ToastLength.Long).Show();

    }
}
