﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcesLambda.Services.Resources
{
    public class ResourceHistoryViewModel
    {
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string RequestDate { get; set; }
        public string Result { get; set; }
    }
}
