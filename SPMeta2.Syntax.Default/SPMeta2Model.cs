using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Syntax.Default
{
    public static class SPMeta2Model
    {
        public static ModelNode NewModel()
        {
            return new ModelNode();
        }

        public static ModelNode DummySite(this ModelNode node)
        {
            node.Value = new SiteDefinition { RequireSelfProcessing = false };

            return node;
        }

        public static ModelNode DummyWeb(this ModelNode node)
        {
            node.Value = new WebDefinition { RequireSelfProcessing = false };

            return node;
        }
    }
}
