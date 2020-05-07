using System;
using System.Collections.Generic;
using System.Text;

namespace PluginInterface
{
    public interface IWrapperPlugin
    {
        bool LogIn();
        ////void ListRFQ();
        bool LoadRFQTable(List<string> slProcessRFQ, string DownloadPath);
        List<string> GetProcessedItems(eActions _eAction);
        bool ProcessRFQ(List<string> slProcessRFQ,string DownloadPath);
        void ProcessQuote(string MailFilePath, string Mail_Template,string FROM_EMAIL_ID,string MAIL_BCC);
    }
}

