using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WCFServiceWebRole1
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // Informacje dotyczące obsługi zmian konfiguracji
            // zawiera temat w witrynie MSDN pod adresem https://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
