using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace monitor.infra.Attributes
{
    public class SkipJwtTokenAttribute : Attribute, IFilterMetadata
    {
    }
}
