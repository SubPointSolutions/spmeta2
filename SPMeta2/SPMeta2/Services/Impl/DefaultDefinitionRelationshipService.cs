﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Common;

namespace SPMeta2.Services.Impl
{
    public class DefaultDefinitionRelationshipService : DefinitionRelationshipServiceBase
    {
        public override IEnumerable<DefinitionRelationship> GetDefinitionRelationships()
        {
            return DefaultDefinitionRelationship.Relationships;
        }
    }
}
