using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace RetainerNotice
{
    public static class Toasty
    {
        public static async Task Toast(string message)
        {
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");
            if (currentVersion.CompareTo(compareToVersion) >= 0)
            {
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                for (int i = 0; i < stringElements.Length; i++)
                {
                    stringElements[i].AppendChild(toastXml.CreateTextNode(message));
                }

                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier("移动雇员铃").Show(toast);
            }
            else
            {
                throw new Exception("Unsupported OS");
            }
        }
    }
}