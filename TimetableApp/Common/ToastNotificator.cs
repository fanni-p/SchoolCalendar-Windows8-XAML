using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TimetableApp.Common
{
    public static class ToastNotificator
    {
        public static void RaiseBackupToastNotification(string username, int homewokrsPerDay)
        {
            string text;
            bool isSilent = true;
            string imagePath = "/Assets/homework.gif";
            if (homewokrsPerDay != 0)
            {
                text = String.Format("Welcome back, {0}!\r\nYou have {1} homeworks for today!"
                                            , username, homewokrsPerDay);
            }
            else
            {
                text = String.Format("Welcome back, {0}!\r\nNo homeworks for today!", username);
            }

            string toastXmlString =
                "<toast>" +
                "<visual version='1'>" +
                "<binding template='toastImageAndText01'>" +
                "<image id='1' src='" + imagePath + "'/>" +
                "<text id='1'>" + text + "</text>" +
                "</binding>" +
                "</visual>" +
                "<audio silent='" + isSilent.ToString().ToLower() + "'/>" +
                "</toast>";

            XmlDocument toastDom = new XmlDocument();
            toastDom.LoadXml(toastXmlString);
            ToastNotification toast = new ToastNotification(toastDom);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
