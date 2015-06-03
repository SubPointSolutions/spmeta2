using System;
using System.Collections.Generic;
using System.Linq;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class ManagedPropertyDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ManagedPropertyDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = string.Format("aM2{0}", Rnd.String());
                def.Description = Rnd.String();

                var types = new List<string>
                {
                    "Text",
                    "Integer",
                    "Decimal",
                    "DateTime",
                    "YesNo",
                    "Binary",
                    "Double"
                };

                def.ManagedType = Rnd.RandomFromArray(types);

                def.Searchable = Rnd.NullableBool();
                def.Queryable = Rnd.NullableBool();
                def.Retrievable = Rnd.NullableBool();
                def.Refinable = Rnd.NullableBool();
                def.Sortable = Rnd.NullableBool();
                def.SafeForAnonymous = Rnd.NullableBool();
                def.TokenNormalization = Rnd.NullableBool();

                var crawledProps = new List<string>()
                {
                    "ows_Name",
                    "ows_URL",
                    "ows_Body",
                    "ows_DocIcon",
                    "ows_DocEmail"
                };


                def.Mappings.AddRange(Rnd.RandomArrayFromArray(crawledProps)
                                         .Select(i => new ManagedPropertyMappping
                                         {
                                             CrawledPropertyName = i
                                         }));
            });
        }
    }
}
