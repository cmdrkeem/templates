using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using __NAME__.DataAccess;
using FubuMVC.Core;

namespace __NAME__
{
    public class __NAME__Registry : FubuRegistry 
    {
        public __NAME__Registry()
        {
            IncludeDiagnostics(true);
   
            Applies.ToThisAssembly();
   
            Actions
                .IncludeTypesNamed(x => x.EndsWith("Controller"));

            Routes
                .IgnoreControllerNamespaceEntirely();

            Views.TryToAttach(x => 
            {
                x.by_ViewModel_and_Namespace_and_MethodName();
                x.by_ViewModel_and_Namespace();
                x.by_ViewModel();
            });
        }
    }
}
