using System.Collections.Generic;
using System.Linq;
using SPMeta2.Definitions;
using SPMeta2.Standard.Services.Webparts.ContentByQueryWebPart;
using System;

namespace SPMeta2.Standard.Services.Webparts
{
    /// <summary>
    /// Enables fluent API to setup 'Content by Query' web part.
    /// </summary>
    public class ContentByQueryWebPartBindingService
    {
        #region constructors

        public ContentByQueryWebPartBindingService()
        {
           // Filters = new FilterValues();
        }

        #endregion

        #region properties

        private Dictionary<string, List<DataMappingValue>> dataMapping = new Dictionary<string, List<DataMappingValue>>();
        private List<DataMappingViewFieldValue> dataMappingView = new List<DataMappingViewFieldValue>();

        #endregion

        #region methods

        public ContentByQueryWebPartBindingService AddDataMapping(string slotName, FieldDefinition field)
        {
            AddDataMapping(slotName, new[] { field });

            return this;
        }

        public ContentByQueryWebPartBindingService AddDataMapping(string slotName, IEnumerable<FieldDefinition> fields)
        {
            foreach (var field in fields)
            {
                // slot processing
                if (!dataMapping.ContainsKey(slotName))
                    dataMapping.Add(slotName, new List<DataMappingValue>());

                var existingSlot = dataMapping[slotName];

                if (existingSlot.FirstOrDefault(m => m.Id.HasValue && m.Id.Value == field.Id) == null)
                {
                    existingSlot.Add(new DataMappingValue
                    {
                        SlotName = slotName,
                        Id = field.Id,
                        InternalName = field.InternalName,
                        FieldType = field.FieldType
                    });
                }


                AddDataMappingViewField(field);
            }

            return this;
        }

        public ContentByQueryWebPartBindingService AddDataMappingViewField(FieldDefinition field)
        {
            return AddDataMappingViewFields(new[] { field });
        }

        public ContentByQueryWebPartBindingService AddDataMappingViewFields(IEnumerable<FieldDefinition> fields)
        {
            foreach (var field in fields)
            {
                // data mapping view processing
                if (dataMappingView.FirstOrDefault(m => m.Id.HasValue && m.Id.Value == field.Id) == null)
                {
                    dataMappingView.Add(new DataMappingViewFieldValue
                    {
                        Id = field.Id,
                        InternalName = field.InternalName,
                        FieldType = field.FieldType
                    });
                }
            }

            return this;
        }

        public string DataMapping
        {
            get { return ComposeDataMapping(); }
        }

        public string DataMappingViewFields
        {
            get { return ComposeDataMappingViewFields(); }
        }

        private string ComposeDataMappingViewFields()
        {
            var resultValues = new List<string>();

            foreach (var value in dataMappingView)
            {
                resultValues.Add(string.Format("{0},{1}",
                    value.Id.HasValue ? value.Id.Value.ToString("B") : string.Empty,
                    //value.InternalName ?? string.Empty,
                    value.FieldType ?? string.Empty));
            }

            return string.Join(";", resultValues);
        }

        private string ComposeDataMapping()
        {
            var resultValues = new List<string>();

            foreach (var slotName in dataMapping.Keys)
            {
                var topValue = dataMapping[slotName];
                var currentValues = new List<string>();

                foreach (var value in topValue)
                {
                    if (value == topValue.First())
                    {
                        currentValues.Add(string.Format("{0}{1}{2}{3}",
                            new string[]
                            {
                                value.SlotName,
                                value.Id.HasValue ? ":" + value.Id.Value.ToString("B") : string.Empty,
                                !string.IsNullOrEmpty(value.InternalName) ? "," + value.InternalName : string.Empty,
                                !string.IsNullOrEmpty(value.FieldType) ? "," + value.FieldType : string.Empty
                            }));
                    }
                    else
                    {
                        currentValues.Add(string.Format("{0}{1}{2}",
                           new string[]
                            {
                                value.Id.HasValue ?  value.Id.Value.ToString("B") : string.Empty,
                                !string.IsNullOrEmpty(value.InternalName) ? "," + value.InternalName : string.Empty,
                                !string.IsNullOrEmpty(value.FieldType) ? "," + value.FieldType : string.Empty
                            }));
                    }

                }

                resultValues.Add(string.Join(";", currentValues));
            }

            return string.Join(";|", resultValues) + ";|";
        }

        public List<FilterPair> Pairs { get; set; }

        public ContentByQueryWebPartBindingService ClearFilter()
        {
            //Filters.FilterValue1 = null;
            //Filters.FilterValue2 = null;
            //Filters.FilterValue3 = null;

            return this;
        }

        public ContentByQueryWebPartBindingService Filter(Guid? fieldIs, string value, string displayName, string type, string filtertOperator)
        {
           // action(Filters);

            return this;
        }

        #endregion

    }

    public class FilterPair
    {
        public FilterValue FilterValue1 { get; set; }
        public FilterValueChainingOperator FilterValue1ChainOperator { get; set; }
    }
}
