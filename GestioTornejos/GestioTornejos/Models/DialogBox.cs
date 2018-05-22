using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace GestioTornejos.Models
{
    public static class DialogBox
    {
        public static async void Show(String title, String message)
        {
            var dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }

        public static async Task<bool> ShowConfirm(String tit, String msg, bool bOnlyCancel)
        {
            bool res = false;

            if (tit == null || msg == null) return res;

            ContentDialog locationPromptDialog;
            if (bOnlyCancel)
            {
                locationPromptDialog = new ContentDialog
                {
                    Title = tit,
                    Content = msg,
                    CloseButtonText = "Tancar"
                };
            }
            else
            {
                locationPromptDialog = new ContentDialog
                {
                    Title = tit,
                    Content = msg,
                    CloseButtonText = "Cancelar",
                    PrimaryButtonText = "Acceptar"
                };
            }

            ContentDialogResult result = await locationPromptDialog.ShowAsync();

            res = (result == ContentDialogResult.Primary) ? true : false;

            return res;
        }
    }
}
